using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExcelToBinary
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
    AllowMultiple = false, Inherited = true)]
    public class ByteMemberAttribute : Attribute
    {
        public string Name { get { return nName; } }
        private string nName = null;

        public ByteMemberAttribute(string t)
        {
            nName = t.ToLower();
        }
    }

    public class ByteSheetHeader
    {
        public string Name { get; set; }
        public Type Type { get; set; }

        public void Load(ByteBufferRead buffer)
        {
            // Parse Header
            Name = buffer.ReadString();
            Name = Name.ToLower();

            string type = buffer.ReadString();
            if (type.Equals("int"))
            {
                Type = typeof(int);
            }
            else if (type.Equals("float"))
            {
                Type = typeof(float);
            }
            else if (type.Equals("string"))
            {
                Type = typeof(string);
            }
            else if (type.Equals("byte"))
            {
                Type = typeof(byte);
            }

            // End Header
        }
    }
    public class ByteRowData
    {
        public int ID { get; set; }
        public Dictionary<string, ByteCellData> Cells { get { return DicCell; } }

        private Dictionary<string, ByteCellData> DicCell = new Dictionary<string, ByteCellData>();

        public void AddCell(ByteCellData cell)
        {
            if (!DicCell.ContainsKey(cell.Name))
            {
                DicCell.Add(cell.Name, cell);
            }
        }

        public T GetValue<T>(string colName)
        {
            colName = colName.ToLower();

            T value = default(T);
            object srcValue = null;
            if (DicCell.ContainsKey(colName))
            {
                srcValue = DicCell[colName].Value;
            }

            Type t = typeof(T);
            if (t == typeof(bool))
            {
                int iValue = 0;
                if (srcValue != null)
                {
                    iValue = Convert.ToInt32(srcValue);
                }

                srcValue = iValue > 0;
            }
            else if (t == typeof(byte))
            {
                byte bValue = 0;
                if (srcValue != null)
                {
                    bValue = Convert.ToByte(srcValue);
                }

                srcValue = bValue;
            }
            else if (t == typeof(float))
            {
                float fValue = 0;
                if (srcValue != null)
                {
                    fValue = Convert.ToSingle(srcValue);
                }

                srcValue = fValue;
            }

            if (srcValue != null)
            {
                try
                {
                    value = (T)(srcValue);
                }
                catch (System.Exception ex)
                {
                    //LogManager.Log(ex);
                }
            }

            return value;
        }

        public int GetInt(string colName)
        {
            return GetValue<int>(colName);
        }

        public float GetFloat(string colName)
        {
            return GetValue<float>(colName);
        }

        public string GetString(string colName)
        {
            return GetValue<string>(colName);
        }

        public int GetByte(string colName)
        {
            return GetValue<byte>(colName);
        }
    }

    public class ByteCellData
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    }


    public class ByteSheetData
    {
        public string SheetName { get; set; }

        private List<ByteRowData> mListRowData = new List<ByteRowData>();
        private List<ByteSheetHeader> mListHeader = new List<ByteSheetHeader>();

        public List<ByteRowData> Rows
        {
            get { return mListRowData; }
        }

        public List<ByteSheetHeader> Headers
        {
            get { return mListHeader; }
        }

        public void Load(ByteBufferRead buffer)
        {
            // 读取表名
            SheetName = buffer.ReadString();

            // 读取表头
            int count = buffer.ReadInt();

            for (int i = 0; i < count; ++i)
            {
                ByteSheetHeader header = new ByteSheetHeader();
                header.Load(buffer);
                mListHeader.Add(header);
            }

            // 读取数据
            count = buffer.ReadInt();
            for (int i = 0; i < count; ++i)
            {
                ByteRowData rowData = new ByteRowData();

                // 读取一行数据
                int index = 0;
                int keyID = 0;
                for (int nCellIndex = 0; nCellIndex < mListHeader.Count; ++nCellIndex)
                {
                    string name = mListHeader[nCellIndex].Name;
                    Type type = mListHeader[nCellIndex].Type;

                    ByteCellData cell = new ByteCellData();
                    cell.Name = name;
                    cell.Index = nCellIndex;

                    if (type == typeof(int))
                    {
                        cell.Value = buffer.ReadInt();
                    }
                    else if (type == typeof(float))
                    {
                        cell.Value = buffer.ReadFloat();
                    }
                    else if (type == typeof(string))
                    {
                        cell.Value = buffer.ReadString();
                    }
                    else if (type == typeof(byte))
                    {
                        cell.Value = buffer.ReadByte();
                    }

                    if (index == 0)
                    {
                        if (type == typeof(int))
                        {
                            keyID = (int)cell.Value;
                            rowData.ID = keyID;
                        }
                        else
                        {
                            throw new Exception("First Column Must be int !");
                        }
                        index++;
                    }

                    rowData.AddCell(cell);
                }
                mListRowData.Add(rowData);
            }
        }

        public ByteRowData GetRowData(int id)
        {
            ByteRowData rowData = null;
            if (0 <= id && id < mListRowData.Count)
            {
                rowData = mListRowData[id];
            }

            return rowData;
        }

        public ByteRowData GetRowData<T>(string colName, T value)
        {
            ByteRowData rowData = null;

            for (int i = 0; i < Rows.Count; ++i)
            {
                if (Rows[i].GetValue<T>(colName).Equals(value))
                {
                    rowData = Rows[i];
                    break;
                }
            }

            if (rowData != null)
            {
                value = rowData.GetValue<T>(colName);
            }

            return rowData;
        }
    }


    public class ByteDataHelper<T>
    {
        public Dictionary<string, PropertyInfo> m_dicProperty = new Dictionary<string, PropertyInfo>();
        public Dictionary<string, ByteMemberAttribute> m_dicByteMember = new Dictionary<string, ByteMemberAttribute>();

        public ByteDataHelper()
        {

        }
        public void CheckClass(List<ByteSheetHeader> listHeader)
        {
#if UNITY_EDITOR
            for (int i = 0; i < listHeader.Count; ++i)
            {
                PropertyInfo info = null;
                mDicProperty.TryGetValue(listHeader[i].Name, out info);
                if (info == null)
                    LogManager.LogWarning(string.Format("Class {0},Not Member :{1}", GetType().ToString(), listHeader[i].Name));
                else
                {
                    ByteMemberAttribute byteMember = null;
                    mDicByteMember.TryGetValue(listHeader[i].Name, out byteMember);

                    if(byteMember != null)
                    {
                        Type t = info.PropertyType;
                        Type srcType = listHeader[i].Type;
                        Type classType = typeof(T);
                        if (!t.Equals(srcType))
                        {
                            string error = string.Format(
                                "SrcType:{0},PropertyType:{1},Class Name: {2},PropertyName :{3}",
                                srcType.ToString(),
                                t.ToString(),
                                classType.ToString(),
                                listHeader[i].Name);
                            LogManager.LogError(error);
                        }
                    }
                }
            }

            Dictionary<string, ByteSheetHeader> headerMap = new Dictionary<string, ByteSheetHeader>();
            foreach (var v in listHeader)
            {
                headerMap[v.Name] = v;
            }

            foreach (var v in mDicProperty)
            {
                if (!headerMap.ContainsKey(v.Key))
                {
                    UnityEngine.Debug.LogWarningFormat("Unnecessary Property,Class: {0},PropertyName:{1}",
                        GetType().ToString(),
                        v.Key);
                }
            }
#endif
        }

        public void SetStaticDataInfo(T data, ByteRowData row, List<ByteSheetHeader> listHeader)
        {
            for (int i = 0; i < listHeader.Count; ++i)
            {
                Type t = listHeader[i].Type;
                object value = null;
                if (t == typeof(int))
                {
                    value = row.GetValue<int>(listHeader[i].Name);
                }
                else if (t == typeof(float))
                {
                    value = row.GetValue<float>(listHeader[i].Name);
                }
                else if (t == typeof(string))
                {
                    value = row.GetValue<string>(listHeader[i].Name);
                }

                PropertyInfo info = null;
                m_dicProperty.TryGetValue(listHeader[i].Name, out info);
                if (info != null)
                {
                    ByteMemberAttribute byteMember = null;
                    m_dicByteMember.TryGetValue(listHeader[i].Name, out byteMember);
                    if (byteMember != null)
                    {
                        try
                        {
                            info.SetValue(data, value, null);
                        }
                        catch (System.Exception ex)
                        {
                            throw new Exception(ex.ToString());
                        }
                    }
                }
            }
        }

        private void GetProperty()
        {
            Type type = typeof(T);
            PropertyInfo[] memberInfo = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < memberInfo.Length; ++i)
            {
                ByteMemberAttribute byteMember = Attribute.GetCustomAttribute(memberInfo[i], typeof(ByteMemberAttribute)) as ByteMemberAttribute;
                if (byteMember != null)
                {
                    string memberName = byteMember.Name;
                    if (!m_dicProperty.ContainsKey(memberName))
                    {
                        m_dicProperty.Add(memberName, memberInfo[i]);
                        m_dicByteMember.Add(memberName, byteMember);
                    }
                    else
                    {
                        throw new Exception("Duplicate MemberName" + type.Name);
                    }
                }
            }
        }
    }
}
