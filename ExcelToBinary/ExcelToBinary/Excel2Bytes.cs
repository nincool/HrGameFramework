using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MSExcelReader;
using System.IO;
using NPOI.SS.UserModel;

namespace StaticData
{
    class Excel2Bytes
    {
        public bool Handle(string strExcelFile, string strDstFile, ref string result)
        {
            bool success = true;

            string file = strExcelFile;
            ExcelReader reader = new ExcelReader(file);
            string excelName = Path.GetFileNameWithoutExtension(file);

            ByteBufferWrite buffer = new ByteBufferWrite(strDstFile, false);
            buffer.WriteInt(123456);

            int sheetCount = reader.SheetCount;
            List<StaticClassSheet> listSheet = new List<StaticClassSheet>();
            for (int i = 0; i < sheetCount; ++i)
            {
                ISheet sheet = reader.GetSheet(i);
                if ("null" == sheet.SheetName.ToLower())
                    continue;

                StaticClassSheet staticSheet = new StaticClassSheet(sheet);
                if (staticSheet.IsValid)
                {
                    listSheet.Add(staticSheet);
                }
            }

            buffer.WriteInt(listSheet.Count);
            foreach (var v in listSheet)
            {
                v.Save(buffer);
            }
            buffer.Close();

            reader.Close();


            return success;
        }
    }

    class StaticClassHeader
    {
        public string Header { get; set; }
        public string Type { get; set; }
    }

    class StaticRowData
    {
        private List<StaticClassCellData> mListCellData = new List<StaticClassCellData>();

        public void AddCell(StaticClassCellData cellData)
        {
            mListCellData.Add(cellData);
        }

        public void Save(ByteBufferWrite buffer)
        {
            foreach (var v in mListCellData)
            {
                v.Save(buffer);
            }
        }
    }

    class StaticClassCellData
    {
        public Type Type { get; set; }
        public string Value { get; set; }

        public void Save(ByteBufferWrite buffer)
        {
            if (Type == typeof(int))
            {
                int value = 0;
                int.TryParse(Value, out value);
                buffer.WriteInt(value);
            }
            else if (Type == typeof(string))
            {
                buffer.WriteString(Value);
            }
            else if(Type == typeof(byte))
            {
                byte value = 0;
                byte.TryParse(Value, out value);
                buffer.WriteByte(value);
            }
            else if (Type == typeof(float))
            {
                float value = 0;
                float.TryParse(Value, out value);
                buffer.WriteFloat(value);
            }
        }
    }

    class StaticClassSheet
    {
        private const int m_nNameRowIndex = 0;
        private const int m_nCommentRowIndex = 1;
        private const int m_nTypeRowIndex = 2;

        private const int m_nDataStartRowIndex = 3;


        private Dictionary<int, Type> mDicClassType = new Dictionary<int, Type>();
        private List<StaticClassHeader> mListHeader = new List<StaticClassHeader>();
        private List<StaticRowData> mListData = new List<StaticRowData>();


        public StaticClassSheet(ISheet sheet)
        {
            SheetName = sheet.SheetName.ToLower();
            HandleSheet(sheet);
        }

        private string SheetName { get; set; }

        public bool IsValid
        {
            get { return mListHeader.Count > 0; }
        }
        private void HandleSheet(ISheet sheet)
        {
            IRow rowType = sheet.GetRow(m_nTypeRowIndex);
            Dictionary<int, string> mDicType = new Dictionary<int, string>();

            if (rowType != null)
            {
                for (int i = 0; i < rowType.Cells.Count; i++)
                {
                    ICell cell = rowType.GetCell(i);
                    string type = FixedString(GetCellString(cell,true));
                    if (IsValidType(type))
                    {
                        mDicType.Add(i, type);
                        mDicClassType.Add(i, GetType(type));
                    }
                }
            }

            if (mDicType.Count > 0)
            {
                IRow rowHead = sheet.GetRow(m_nNameRowIndex);

                if (rowHead != null)
                {
                    foreach (var v in mDicType)
                    {
                        StaticClassHeader header = new StaticClassHeader();
                        header.Type = v.Value;
                        header.Header = FixedString(GetCellString(rowHead.GetCell(v.Key),true));
                        mListHeader.Add(header);
                    }
                }

                for (int row = m_nDataStartRowIndex; row <= sheet.LastRowNum; row++)
                {
                    IRow curRow = sheet.GetRow(row);

                    if (curRow == null)
                        continue;

                    ICell cell0 = curRow.Cells[0];
                    if (cell0 == null || (cell0.CellType != CellType.Numeric && cell0.CellType != CellType.String && cell0.CellType != CellType.Formula))
                    {
                        continue;
                    }

                    int id = 0;
                    int.TryParse(GetCellValue(cell0), out id);
                    if (id == 0)
                        continue;

                    StaticRowData rowData = new StaticRowData();
                    foreach (var kvp in mDicClassType)
                    {
                        ICell cell = curRow.GetCell(kvp.Key);

                        StaticClassCellData cellData = new StaticClassCellData();
                        cellData.Type = kvp.Value;
                        if (cell == null)
                        {
                            if (cellData.Type == typeof(string))
                            {
                                cellData.Value = string.Empty;
                            }
                            else
                            {
                                cellData.Value = "0";
                            }
                        }
                        else
                        {
                            if (cellData.Type == typeof(string))
                            {
                                 cellData.Value = GetCellString(cell);
                            }
                            else
                            {
                                cellData.Value = GetCellValue(cell);
                            }
                        }

                        rowData.AddCell(cellData);
                    }

                    mListData.Add(rowData);
                }
            }
        }

        public string GetCellString(ICell cell,bool tolower = false)
        {
            string value = string.Empty;
            if (cell.CellType == CellType.Blank)
            {
                value = string.Empty;
            }
            else if (cell.CellType == CellType.Boolean)
            {
                value = cell.BooleanCellValue.ToString();
            }
            else if (cell.CellType == CellType.String)
            {
                value = cell.StringCellValue;
            }
            else if (cell.CellType == CellType.Numeric)
            {
                value = cell.NumericCellValue.ToString();
            }
            else
            {
                value = string.Empty;
            }

            if(tolower)
            {
                value = value.ToLower();
            }
            return value;
        }

        private string GetCellValue(ICell cell)
        {
            try
            {
                string cellValue = string.Empty;
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        cellValue = cell.NumericCellValue.ToString();
                        break;
                    case CellType.String:
                        cellValue = cell.StringCellValue;
                        break;
                    case CellType.Boolean:
                        cellValue = cell.BooleanCellValue ? "1" : "0";
                        break;
                    case CellType.Formula:
                        cellValue = cell.NumericCellValue.ToString();
                        break;
                    default:
                        break;
                }

                return cellValue;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return "";
        }

        private Type GetType(string value)
        {
            Type t = null;

            string typeName = FixedString(value);

            if (typeName.Equals("int"))
            {
                t = typeof(int);
            }
            if(typeName.Equals("byte"))
            {
                t = typeof(byte);
            }
            else if (typeName.Equals("string"))
            {
                t = typeof(string);
            }
            else if (typeName.Equals("float"))
            {
                t = typeof(float);
            }

            return t;
        }

        private bool IsValidType(string value)
        {
            bool valid = false;
            string typeName = FixedString(value);

            if (typeName.Equals("int")
             || typeName.Equals("string")
             || typeName.Equals("float")
             || typeName.Equals("byte"))
            {
                valid = true;
            }

            return valid;
        }

        public void Save(ByteBufferWrite buffer)
        {
            buffer.WriteString(SheetName);

            int count = mListHeader.Count;
            buffer.WriteInt(count);

            foreach (var v in mListHeader)
            {
                buffer.WriteString(v.Header);
                buffer.WriteString(v.Type);
            }

            count = mListData.Count;
            buffer.WriteInt(count);
            foreach (var v in mListData)
            {
                v.Save(buffer);
            }
        }

        private string FixedString(string src)
        {
            return src.ToLower().Replace(" ", string.Empty);
        }
    }
}
