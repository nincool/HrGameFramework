using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckDataTools
{
    struct SheetHeader
    {
        public string Header { get; set; }
        public string Type { get; set; }
    }

    class SheetCell
    {
        public Type Type { get; set; }
        public string Value { get; set; }
        public int Index { get; set; }

        public T GetValue<T>()
        {
            T t = default(T);

            object value = null;
            if(Type == typeof(int))
            {
                int intValue = 0;
                int.TryParse(Value, out intValue);
                value = intValue;
            }
            else if(Type == typeof(byte))
            {
                byte byteValue = 0;
                byte.TryParse(Value, out byteValue);
                value = byteValue;
            }
            else if(Type == typeof(float))
            {
                float floatValue = 0;
                float.TryParse(Value, out floatValue);
                value = floatValue;
            }
            else if(Type == typeof(string))
            {
                value = Value;
            }

            t = (T)value;
            return t;
        }
    }

    class SheetList
    {
        public void Add(SheetCell cell)
        {
            ListData.Add(cell);
        }

        public string SheetName { get; set; }
        public string Header { get; set; }
        public string Condition { get; set; }
        public Type Type { get; set; }
        public List<SheetCell> ListData = new List<SheetCell>();
    }

    class ExcelSheet
    {
        public string SheetName { get; set; }

        private List<SheetHeader> mListHeader = new List<SheetHeader>();

        public Dictionary<int, SheetList> DicData
        {
            get
            {
                return mDicColumn;
            }
        }

        private Dictionary<int, SheetList> mDicColumn = new Dictionary<int, SheetList>();

        public ExcelSheet(ISheet sheet)
        {
            SheetName = sheet.SheetName.ToLower();

            LoadSheet(sheet);
        }

        private string FixedString(string src)
        {
            return src.ToLower().Replace(" ", string.Empty);
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

        private Type GetType(string value)
        {
            Type t = null;

            string typeName = FixedString(value);

            if (typeName.Equals("int"))
            {
                t = typeof(int);
            }
            if (typeName.Equals("byte"))
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

        private void LoadSheet(ISheet sheet)
        {
            IRow rowType = sheet.GetRow(3);
            Dictionary<int, string> dicType = new Dictionary<int, string>();

            if (rowType != null)
            {
                for (int i = 0; i < rowType.Cells.Count; i++)
                {
                    ICell cell = rowType.GetCell(i);
                    if (IsValidType(GetCellString(cell)))
                    {
                        dicType.Add(i, GetCellString(cell));
                    }
                }
            }

            if (dicType.Count > 0)
            {
                LoadData(sheet, dicType);
            }
        }

        private void AddData(Type type,int index,string header,string condition,SheetCell cell)
        {
            if(!mDicColumn.ContainsKey(index))
            {
                mDicColumn[index] = new SheetList();
                mDicColumn[index].Condition = condition;
                mDicColumn[index].Header = header;
                mDicColumn[index].SheetName = SheetName;
                mDicColumn[index].Type = type;
            }

            mDicColumn[index].Add(cell);
        }

        public string GetCellString(ICell cell)
        {
            string value = string.Empty;
            if (cell != null)
            {
                 if (cell.CellType == CellType.Boolean)
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
            }

            return value.ToLower();
        }

        private void LoadData(ISheet sheet, Dictionary<int, string> dicType)
        {
            IRow rowCondition = sheet.GetRow(2);

            Dictionary<int, string> dicCondition = new Dictionary<int, string>();

            if (rowCondition != null)
            {
                for (int i = 0; i < rowCondition.Cells.Count; i++)
                {
                    ICell cell = rowCondition.GetCell(i);
                    dicCondition.Add(i, GetCellString(cell));
                }
            }

            IRow rowHead = sheet.GetRow(0);
            Dictionary<int, string> dicHeader = new Dictionary<int, string>();

            if (rowHead != null)
            {
                for (int i = 0; i < rowHead.Cells.Count; i++)
                {
                    ICell cell = rowHead.GetCell(i);
                    dicHeader.Add(i, GetCellString(cell));
                }
            }

            for (int row = 4; row <= sheet.LastRowNum; row++)
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

                foreach (var kvp in dicType)
                {
                    ICell cell = curRow.GetCell(kvp.Key);

                    string typeString = kvp.Value.ToString().ToLower();

                    Type type = GetType(kvp.Value);
                    SheetCell cellData = new SheetCell();
                    cellData.Index = row;
                    cellData.Type = type;
                    if (cell == null)
                    {
                        if (typeString.Equals("string"))
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
                        if (typeString.Equals("string"))
                        {
                            cellData.Value = GetCellString(cell);
                        }
                        else
                        {
                            cellData.Value = GetCellValue(cell);
                        }
                    }

                    if (dicCondition.ContainsKey(kvp.Key) && dicHeader.ContainsKey(kvp.Key))
                    {
                        string condition = dicCondition[kvp.Key];
                        string header = dicHeader[kvp.Key];
                        AddData(type, kvp.Key, header, condition, cellData);
                    }
                }
            }
        }
    }
}
