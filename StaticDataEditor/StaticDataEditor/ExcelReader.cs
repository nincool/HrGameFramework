using System;
using System.Collections.Generic;
using System.Diagnostics;
using NPOI.SS.UserModel;
using System.Windows.Forms;

namespace MSExcelReader
{
    class ExcelHeaderAttribute : Attribute
    {
        public ExcelHeaderAttribute(string headName)
        {
            m_HeadName = headName;
        }

        private string m_HeadName = string.Empty;
        public string HeadName
        {
            get
            {
                return m_HeadName;
            }
            set
            {
                m_HeadName = value;
            }
        }
    }

    class ExcelReader
    {
        public ExcelReader(String excelName)
        {
            if (!System.IO.File.Exists(excelName))
            {
                excelName = Environment.CurrentDirectory + excelName;
            }
            m_bIsOpen = OpenExcel(excelName);

            if (!m_bIsOpen)
                throw new Exception("读取" + excelName + "文件失败！");
        }

        ~ExcelReader()
        {
            m_WorkBook = null;
            if (m_ExcelStream != null)
            {
                m_ExcelStream.Close();
                m_ExcelStream = null;
            }
        }

        private System.IO.FileStream m_ExcelStream = null;
        private IWorkbook m_WorkBook = null;

        public int SheetCount
        {
           get
            {
                if (m_WorkBook != null)
                {
                    return m_WorkBook.NumberOfSheets;
                }

                return 0;
            }
        }

        public  ISheet GetSheet(int index)
        {
            return m_WorkBook.GetSheetAt(index);
        }

        public bool OpenExcel(string excelName)
        {
            try
            {
                m_ExcelStream = System.IO.File.OpenRead(excelName);
                if (excelName.EndsWith(".xls"))
                {
                    m_WorkBook = new NPOI.HSSF.UserModel.HSSFWorkbook(m_ExcelStream);
                }
                else if (excelName.EndsWith(".xlsx"))
                {
                    m_WorkBook = new NPOI.XSSF.UserModel.XSSFWorkbook(m_ExcelStream);
                }

                if (m_WorkBook == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool m_bIsOpen = false;
        public bool IsOpen
        {
            get
            {
                return m_bIsOpen;
            }
        }

        public void Close()
        {
            try
            {
                m_ExcelStream.Close();
                m_WorkBook = null;
            }
            catch (Exception)
            {

            }
        }

        private int GetSheetNameIndex(ISheet sheet, string headerName)
        {
            IRow rowHeader = sheet.GetRow(0);
            for (int i = 0; i < rowHeader.Cells.Count; i++)
            {
                ICell cell = rowHeader.Cells[i];
                if (string.IsNullOrWhiteSpace(cell.StringCellValue))
                {
                    break;
                }
                if (cell.StringCellValue.ToLower() == headerName.ToLower())
                {
                    return i;
                }
            }
            return -1;
        }

        Dictionary<int, string> GetSheetHeader(string sheetName, Type t)
        {
            try
            {
                if (m_WorkBook == null)
                {
                    return null;
                }

                Dictionary<int, string> headerList = new Dictionary<int, string>();
                ISheet sheet = m_WorkBook.GetSheet(sheetName);
                var properties = t.GetProperties();
                foreach (var propert in properties)
                {
                    var attributes = propert.GetCustomAttributes(typeof(ExcelHeaderAttribute), false);
                    if (attributes != null && attributes.Length > 0)
                    {
                        foreach (var attr in attributes)
                        {
                            if (attr is ExcelHeaderAttribute)
                            {
                                ExcelHeaderAttribute ehAttr = attr as ExcelHeaderAttribute;
                                int nIndex = GetSheetNameIndex(sheet, ehAttr.HeadName);
                                if (nIndex != -1)
                                {
                                    headerList.Add(nIndex, propert.Name);
                                }
                            }
                        }
                    }
                }

                return headerList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static void SetDataPropertyValue(object objInstance, string propertyName, string strValue)
        {
            if (objInstance == null || string.IsNullOrWhiteSpace(strValue))
            {
                return;
            }
            var property = objInstance.GetType().GetProperty(propertyName);
            if (property == null)
            {
                throw new Exception("Invalid property : " + propertyName);
            }
            if (property.PropertyType == typeof(int))
            {
                property.SetValue(objInstance, int.Parse(strValue), new object[] { });
                return;
            }
            else if (property.PropertyType == typeof(string))
            {
                property.SetValue(objInstance, strValue, new object[] { });
                return;
            }
            else if (property.PropertyType == typeof(uint))
            {
                property.SetValue(objInstance, uint.Parse(strValue), new object[] { });
                return;
            }
            else if (property.PropertyType == typeof(short))
            {
                property.SetValue(objInstance, short.Parse(strValue), new object[] { });
                return;
            }
            else if (property.PropertyType == typeof(char))
            {
                property.SetValue(objInstance, char.Parse(strValue), new object[] { });
                return;
            }
            else if (property.PropertyType == typeof(ushort))
            {
                property.SetValue(objInstance, ushort.Parse(strValue), new object[] { });
                return;
            }
            else if (property.PropertyType == typeof(byte))
            {
                property.SetValue(objInstance, byte.Parse(strValue), new object[] { });
                return;
            }
            else if (property.PropertyType == typeof(float))
            {
                property.SetValue(objInstance, float.Parse(strValue), new object[] { });
                return;
            }
            else if (property.PropertyType == typeof(double))
            {
                property.SetValue(objInstance, double.Parse(strValue), new object[] { });
                return;
            }
            else if (property.PropertyType == typeof(bool))
            {
                property.SetValue(objInstance, int.Parse(strValue) != 0, new object[] { });
                return;
            }
            throw new Exception("Invalid property type : " + property.PropertyType.ToString());
        }

        public List<T> GetSheetData<T>(string sheetName) where T : new()
        {
            if (m_WorkBook == null)
            {
                return null;
            }
            Type t = typeof(T);
            Dictionary<int, string> headerDic = GetSheetHeader(sheetName, t);
            if (headerDic == null || headerDic.Count <= 0)
            {
                return null;
            }

            ISheet sheet = m_WorkBook.GetSheet(sheetName);

            List<T> sheetData = new List<T>();
            bool bSaveable = t.IsSubclassOf(typeof(ISaveable));
            for (int row = 3; row <= sheet.LastRowNum; row++)
            {
                T data = new T();
                IRow curRow = sheet.GetRow(row);
                if (curRow == null)
                {
                    continue;
                }

                ICell cell0 = curRow.Cells[0];
                if (cell0 == null || (cell0.CellType != CellType.Numeric && cell0.CellType != CellType.String && cell0.CellType != CellType.Formula))
                {
                    break;
                }
                foreach (var kv in headerDic)
                {
                    ICell cell = curRow.GetCell(kv.Key);
                    if (cell == null)
                    {
                        continue;
                    }
                    String cellValue = String.Empty;
                    switch (cell.CellType)
                    {
                        case CellType.Blank:
                            break;
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
                    try
                    {
                        SetDataPropertyValue(data, kv.Value, cellValue);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.StackTrace);
                        return null;
                    }
                }

                if (bSaveable)
                {
                    ISaveable iSaveable = data as ISaveable;
                    if (!iSaveable.CheckValid())
                    {
                        string error = string.Format("表格 [\"{0}\"] 中第 {1} 行数据有误！！！", sheetName, row);
                        MessageBox.Show(error, "数据错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        throw new Exception(error);
                    }
                }

                sheetData.Add(data);
            }

            return sheetData;
        }
    }
}
