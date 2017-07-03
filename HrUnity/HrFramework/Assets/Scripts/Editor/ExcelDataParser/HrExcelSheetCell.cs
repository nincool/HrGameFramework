using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hr.Editor
{
    public class HrExcelSheetCell
    {
        public string SheetName
        {
            get;
            private set;
        }

        public Type SystemType
        {
            get;
            protected set;
        }

        public int RowIndex
        {
            get;
            protected set;
        }

        public int ColunmIndex
        {
            get;
            protected set;
        }

        protected object m_data;

        public HrExcelSheetCell(string strSheetName, object data, int nRowIndex, int nColunmIndex)
        {
            SheetName = strSheetName;
            m_data = data;
            SystemType = m_data.GetType();
            if (m_data.GetType().FullName == "System.Double")
            {
                //源数据是double，目标数据是int的直接拆箱装箱转换成int
                m_data = (int)(double)m_data;
                SystemType = typeof(int);
            }
        }


        public T GetData<T>()
        {
            Assert.IsTrue(SystemType == typeof(T));

            return ParseType<T>(m_data);
        }


        protected T ParseType<T>(object data)
        {
            try
            {
                return (T)(data);
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("PaseType error! message '{0}' sheet name {1} source '{2}' destination '{3}' data '{4}' datatype '{5}'", e.Message, SheetName, SystemType.FullName, typeof(T).FullName, data, data.GetType().FullName));

                return default(T);
            }
        }
    }

    public enum HrExcelSheetCellType
    {
        CELL_TYPE_TITLE,
        CELL_TYPE_COMMENT,
        CELL_TYPE_TYPE,
        CELL_TYPE_DATA,
    }

    public class HrExcelSheetConfCell : HrExcelSheetCell
    {
        public HrExcelSheetCellType CellType
        {
            get;
            private set;
        }
        public string StrType
        {
            get;
            private set;
        }


        public HrExcelSheetConfCell(string strSheetName, HrExcelSheetCellType cellType, string strType, object data, int nRowIndex, int nColunmIndex) : base(strSheetName, data, nRowIndex, nColunmIndex)
        {
            CellType = cellType;
            StrType = strType;
            if (IsValidType(StrType))
            {
                SystemType = MapType(StrType);
            }
            else
            {
                StrType = "null";
                SystemType = null;
            }
            if (SystemType != null)
            {
                if (m_data.GetType().FullName != SystemType.FullName)
                {
                    Debug.LogWarning(string.Format("excel cell data type is not match! ConfType[{0}] DataType[{1}] Data[{2}]", SystemType.FullName, m_data.GetType().FullName, m_data));

                    if (m_data.GetType().FullName == "System.Double" && (strType == "int" || strType == "uint"))
                    {
                        //源数据是double，目标数据是int的直接拆箱装箱转换成int
                        m_data = (int)(double)m_data;
                    }
                    else if (m_data.GetType().FullName == "System.Double" && strType == "float")
                    {
                        m_data = (float)(double)m_data;
                    }
                }
            }
        }

        private Type MapType(string strType)
        {
            if (strType.Equals("int"))
            {
                return typeof(int);
            }
            else if (strType.Equals("uint"))
            {
                return typeof(uint);
            }
            if (strType.Equals("byte"))
            {
                return typeof(byte);
            }
            else if (strType.Equals("string"))
            {
                return typeof(string);
            }
            else if (strType.Equals("float"))
            {
                return typeof(float);
            }

            return null;
        }

        private bool IsValidType(string strType)
        {
            if (strType.Equals("int")
                || strType.Equals("uint")
                || strType.Equals("string")
                || strType.Equals("float")
                || strType.Equals("byte"))
            {
                return true;
            }

            return false;
        }
    }


}
