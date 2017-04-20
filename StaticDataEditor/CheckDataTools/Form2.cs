using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

/*
 *
    [1] 不可重复
    [2] 不可为0
    [3:表名:列名:|] 本列以"|"为分隔符的内容在 表名对应列数中可以找到。（忽略0）“|”为“”代表无分割
    [4]本列不可为空

    [2],[1] 同时检查不可重复和非0

 */
namespace CheckDataTools
{
    public partial class Form2 : Form
    {
        private List<ExcelSheet> mListExcelSheet = new List<ExcelSheet>();

        public Form2()
        {
            InitializeComponent();          
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            m_TextExcelDir.Text = GetValue("Excel");
            m_TextResDir.Text = GetValue("Res");
        }

        private string configFile
        {
            get { return Environment.CurrentDirectory + "\\config.xml"; }
        }

        private string GetValue(string AppKey)
        {
            string value = string.Empty;

            if(File.Exists(configFile))
            {
                System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
                xDoc.Load(configFile);

                System.Xml.XmlNode xNode;
                System.Xml.XmlElement xElem1;
                xNode = xDoc.SelectSingleNode("//configuration");

                xElem1 = (System.Xml.XmlElement)xNode.SelectSingleNode("//add[@key='" + AppKey + "']");
                if (xElem1 != null)
                {
                    value = xElem1.GetAttribute("value");
                }
            }

            return value;
        }

        private void SetValue(string AppKey, string AppValue)
        {
            System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
            xDoc.Load(configFile);

            System.Xml.XmlNode xNode;
            System.Xml.XmlElement xElem1;
            System.Xml.XmlElement xElem2;
            xNode = xDoc.SelectSingleNode("//configuration");

            xElem1 = (System.Xml.XmlElement)xNode.SelectSingleNode("//add[@key='" + AppKey + "']");
            if (xElem1 != null) xElem1.SetAttribute("value", AppValue);
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", AppKey);
                xElem2.SetAttribute("value", AppValue);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(System.Windows.Forms.Application.ExecutablePath + ".config");
        }

        private void LoadData(string[] allExcels)
        {
            mListExcelSheet.Clear();
            foreach (var file in allExcels)
            {
                try
                {
                    ExcelReader reader = new ExcelReader(file);
                    string excelName = Path.GetFileNameWithoutExtension(file);

                    int sheetCount = reader.SheetCount;
                    for (int i = 0; i < sheetCount; ++i)
                    {
                        ISheet sheet = reader.GetSheet(i);
                        if ("null" == sheet.SheetName.ToLower())
                            continue;

                        ExcelSheet excelSheet = new ExcelSheet(sheet);
                        mListExcelSheet.Add(excelSheet);
                    }
                }
                catch (Exception ex)
                {
                    richTextBox1.Text = ex.ToString();
                }
            }
        }

        private void CheckAllSheet()
        {
            StringBuilder error = new StringBuilder();
            foreach (var v in mListExcelSheet)
            {
                Dictionary<int, SheetList> dic = v.DicData;
         
                foreach (var vv in dic)
                {
                    StringBuilder sb = new StringBuilder();
                    if (NeedCheck(vv.Value.Condition))
                    {
                        foreach (var vvv in vv.Value.ListData)
                        {
                            sb.Append(CheckCell(vv.Value.Condition, vvv, vv.Value));
                        }
                    }

                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        error.Append(sb.ToString());
                    }
                }
            }

            if(string.IsNullOrEmpty(error.ToString()))
            {
                error.Append("无错误");
            }

            richTextBox1.Text = error.ToString();
        }

        private string  CheckCell(string condition,SheetCell cell, SheetList list)
        {
            StringBuilder sb = new StringBuilder();

            string[] conditions = condition.Split(',');

            if(condition.Length > 0)
            {
                foreach (var v in conditions)
                {
                    if(v.Equals("[1]"))
                    {
                        if(GetCount(cell, list) > 1)
                        {
                            sb.Append(ErrorFormat("有相同的元素", cell,list));
                            sb.Append(Environment.NewLine);
                        }
                    }

                    if (v.Equals("[2]"))
                    {
                        Type t = cell.Type;
                        bool valid = true;
                        if(t == typeof(int))
                        {
                            if (cell.GetValue<int>() == 0)
                            {
                                valid = false;
                            }
                        }
                        else if (t == typeof(byte))
                        {
                            if (cell.GetValue<byte>() == 0)
                            {
                                valid = false;
                            }
                        }
                        else if(t == typeof(float))
                        {
                            if (cell.GetValue<float>() == 0)
                            {
                                valid = false;
                            }
                        }

                        if (!valid)
                        {
                            sb.Append(ErrorFormat("不能为0", cell, list));
                            sb.Append(Environment.NewLine);
                        }
                    }

                    if(v.Equals("[4]"))
                    {
                        if(string.IsNullOrEmpty(cell.Value))
                        {
                            sb.Append(ErrorFormat("不能为空", cell, list));
                            sb.Append(Environment.NewLine);
                        }
                    }

                    if (v.Contains("[3"))
                    {
                        string str = v.Replace("]", string.Empty);
                        string[] checks = str.Split(':');
                        if(checks.Length == 3)
                        {
                            string sheetName = checks[1].ToLower();
                            string columnName = checks[2].ToLower();

                            if (!HasCell(cell.Value, sheetName, columnName))
                            {
                                sb.Append(ErrorFormat("查找不到指定的数据", cell, list));
                                sb.Append(Environment.NewLine);
                            }
                        }
                        else if (checks.Length == 4)
                        {
                            string sheetName = checks[1].ToLower();
                            string columnName = checks[2].ToLower();

                            string[] values = cell.Value.Split('|');
                            if(values.Length > 0)
                            {
                                foreach(var vv in values)
                                {
                                    if (!HasCell(vv, sheetName, columnName))
                                    {
                                        sb.Append(ErrorFormat("查找不到指定的数据", cell, list));
                                        sb.Append(Environment.NewLine);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return sb.ToString();
        }

        private string ErrorFormat(string errorString,SheetCell cell, SheetList list)
        {
            return string.Format("{0},表名:{1},列名:{2},Value:{3},行数{4}", errorString, list.SheetName, list.Header, cell.Value,cell.Index + 1);
        }

        private bool HasCell(string value,string sheetName,string columnName)
        {
            if (value.Equals("0") || string.IsNullOrEmpty((value)))
                return true;

            bool hasExist = false;

            SheetList list = GetSheet(sheetName, columnName);
            if(list != null)
            {
                foreach(var v in list.ListData)
                {
                    if(v.Value.Equals(value))
                    {
                        hasExist = true;
                    }
                }
            }


            return hasExist;
        }

        private SheetList GetSheet(string sheetName,string columnName)
        {
            SheetList list = null;
            foreach(var v in mListExcelSheet)
            {
                if(v.SheetName.Equals(sheetName))
                {
                    foreach(var vv in v.DicData)
                    {
                        if(vv.Value.Header.Equals(columnName))
                        {
                            list = vv.Value;
                        }
                    }
                }
            }

            return list;
        }

        private int GetCount(SheetCell cell,SheetList list)
        {
            int count = 0;
            Type t = cell.Type;

            foreach (var v in list.ListData)
            {
                if(t == typeof(int))
                {
                    if(v.GetValue<int>() == cell.GetValue<int>())
                    {
                        count++;
                    }
                }

                else if (t == typeof(byte))
                {
                    if (v.GetValue<byte>() == cell.GetValue<byte>())
                    {
                        count++;
                    }
                }

                else if(t == typeof(string))
                {
                    if (v.GetValue<string>() == cell.GetValue<string>())
                    {
                        count++;
                    }
                }

                else if(t == typeof(float))
                {
                    if (v.GetValue<float>() == cell.GetValue<float>())
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        //是否需要检测
        private bool NeedCheck(string condition)
        {
            bool needCheck = true;

            if(string.IsNullOrEmpty(condition))
            {
                needCheck = false;
            }

            return needCheck;
        }

        private void CheckRes()
        {
            SheetList fileList = GetSheet("resource", "file");
            SheetList typeList = GetSheet("resource", "type");
            string resPath = m_TextResDir.Text;

            StringBuilder sb = new StringBuilder();
            if(fileList != null && typeList != null)
            {
                foreach (var v in fileList.ListData)
                {
                    string type = GetFileType(v.Index, typeList);
                    string outPut = CheckFile(resPath, type, v.Value);
                    if (!string.IsNullOrEmpty(outPut))
                    {
                        sb.Append(outPut);
                        sb.Append(Environment.NewLine);
                    }
                }
            }
            else
            {
                sb.Append("资源表不存在！");
            }

            richTextBox1.Text = sb.ToString();
        }

        private string CheckFile(string resPath,string type,string name)
        {
            string errorOut = string.Empty;
            string filePath = string.Empty;

            if(resPath[resPath.Length -1] != '\\' && resPath[resPath.Length - 1] != '/')
            {
                resPath += "\\";
            }

            if (type.Equals("1"))
            {
                // dress
                filePath = resPath + "decorate\\dress\\" +  name + ".drs";
                if(!File.Exists(filePath))
                {
                    errorOut = name + ".drs";
                }
            }
            else if (type.Equals("2"))
            {
                // music
                filePath = resPath + "music\\" + name + ".mp3";
                if (!File.Exists(filePath))
                {
                    errorOut = name + ".mp3";
                }

                filePath = resPath + "music\\" + name + ".ogg";
                if (!File.Exists(filePath))
                {
                    errorOut += Environment.NewLine;
                    errorOut += name + ".ogg";
                }
            }
            else if(type.Equals("3"))
            {
                // 谱面文件
                filePath = resPath + "music\\stage\\osu\\" + name + ".osu";
                if (!File.Exists(filePath))
                {
                    errorOut = name + ".osu";
                }
            }

            else if (type.Equals("7"))
            {
                // 谱面文件
                filePath = resPath + "music\\stage\\danceaction\\" + name + ".bytes";
                if (!File.Exists(filePath))
                {
                    errorOut = name + ".bytes";
                }
            }

            else if (type.Equals("8"))
            {
                // 图片
                filePath = resPath + "icon\\" + name + ".texture";
                if (!File.Exists(filePath))
                {
                    errorOut = name + ".texture";
                }
            }

            else if (type.Equals("9"))
            {
                // 场景资源
                filePath = resPath + "scene\\" + name + ".scene";
                if (!File.Exists(filePath))
                {
                    errorOut = name + ".scene";
                }
            }

            else if (type.Equals("10"))
            {
                // 特效
                filePath = resPath + "decorate\\effect\\" + name + ".eff";
                if (!File.Exists(filePath))
                {
                    errorOut = name + ".eff";
                }
            }

            else if (type.Equals("11"))
            {
                // 动作
                filePath = resPath + "animation\\" + name + ".ani";
                if (!File.Exists(filePath))
                {
                    errorOut = name + ".ani";
                }
            }
            
            else if(type.Equals("12"))
            {
                // 音效
                filePath = resPath + "audio\\" + name + ".ab";
                if (!File.Exists(filePath))
                {
                    errorOut = name + ".ab";
                }
            }

            return errorOut;
        }

        private string GetFileType(int row,SheetList list)
        {
            string type = string.Empty;

            foreach(var v in list.ListData)
            {
                if(v.Index == row)
                {
                    type = v.Value;
                }
            }

            return type;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string dir = m_TextExcelDir.Text;
            if(Directory.Exists(dir))
            {
                string[] files = Directory.GetFiles(dir, "*.xls");
                LoadData(files);
                CheckAllSheet();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string dir = m_TextExcelDir.Text;
            if (Directory.Exists(dir))
            {
                string[] files = Directory.GetFiles(dir, "*.xls");
                LoadData(files);
                CheckRes();
            }
        }

        private void m_BtnSelectExcel_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                m_TextExcelDir.Text = dialog.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                m_TextResDir.Text = dialog.SelectedPath;
            }
        }

        private void m_TextExcelDir_TextChanged(object sender, EventArgs e)
        {
            SetValue("Excel", m_TextExcelDir.Text);
        }

        private void m_TextResDir_TextChanged(object sender, EventArgs e)
        {
            SetValue("Res", m_TextExcelDir.Text);
        }
    }
}
