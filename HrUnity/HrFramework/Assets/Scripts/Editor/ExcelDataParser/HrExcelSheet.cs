using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hr.Editor
{
    public class HrExcelSheet
    {

        public string SheetName
        {
            get;
            private set;
        }

        public int ColumnsCount
        {
            get;
            private set;
        }

        public int RowsCount
        {
            get;
            private set;
        }

        public bool IsSheetValid
        {
            get;
            private set;
        }

        protected object[,] m_sheetData = null;
        /// <summary>
        /// 全部数据
        /// </summary>
        protected List<List<HrExcelSheetCell>> m_lisExcelSheetCell = new List<List<HrExcelSheetCell>>();

        public HrExcelSheet(string strSheetName, int nColumnsCount, int nRowsCount, object[,] data)
        {
            SheetName = strSheetName;
            ColumnsCount = nColumnsCount;
            RowsCount = nRowsCount;
            m_sheetData = data;

        }

        public virtual void InitSheetCellData()
        {
            MakeSheetCellData();
        }

        protected virtual void MakeSheetCellData()
        {
            for (int nRowIndex = 0; nRowIndex < RowsCount; ++nRowIndex)
            {
                List<HrExcelSheetCell> lisExcelSheetRowCell = new List<HrExcelSheetCell>();
                for (int nColumnIndex = 0; nColumnIndex < ColumnsCount; ++nColumnIndex)
                {
                    object data = m_sheetData[nRowIndex, nColumnIndex];

                    HrExcelSheetCell sheetCell = new HrExcelSheetCell(data, nRowIndex, nColumnIndex);
                    lisExcelSheetRowCell.Add(sheetCell);
                }
                m_lisExcelSheetCell.Add(lisExcelSheetRowCell);
            }

        }

        protected bool CheckTypeValid<T>(object obj)
        {
            if (obj.GetType() == typeof(T))
            {
                return true;
            }
            return false;
        }
        public virtual HrExcelSheetCell GetCellData(int nRowIndex, int nColumnIndex)
        {
            Assert.IsTrue(nRowIndex < RowsCount && nColumnIndex < ColumnsCount);

            return m_lisExcelSheetCell[nRowIndex][nColumnIndex];
        }
    }

    /// <summary>
    /// 去除冗余数据，重新组织数据
    /// </summary>
    public class HrExcelSheetConf : HrExcelSheet
    {
        private const int m_c_nHeadIndex = 0;
        private const int m_c_nCommentIndex = 1;
        private const int m_c_nTypeRowIndex = 2;
        private const int m_c_nHeadRowCount = 3;

        public int ConfRowsCount
        {
            get;
            private set;
        }

        public int ConfColumnsCount
        {
            get;
            private set;
        }

        private List<string> m_lisStringType = null;
        /// <summary>
        /// 去除头的数据
        /// </summary>
        protected List<List<HrExcelSheetCell>> m_lisConfSheetCell = new List<List<HrExcelSheetCell>>();

        public List<T> GetRowData<T>(int nRowIndex)
        {
            Assert.IsTrue(nRowIndex < RowsCount);

            List<T> lisRowData = new List<T>();
            for (int i = 0; i < ColumnsCount; ++i)
            {
                if (CheckTypeValid<T>(m_sheetData[nRowIndex, i]))
                {
                    lisRowData.Add((T)m_sheetData[nRowIndex, i]);
                }
            }

            return lisRowData;
        }

        public HrExcelSheetConf(string strSheetName, int nColumnsCount, int nRowsCount, object[,] data) : base(strSheetName, nColumnsCount, nRowsCount, data)
        {
            CheckSheetDataType();

        }

        public override void InitSheetCellData()
        {
            MakeSheetCellData();
            MakeConciseData();
        }

        public HrExcelSheetCell GetConciseCell(int nRowIndex, int nColumnIndex)
        {
            try
            {
                if (nRowIndex < ConfRowsCount && nColumnIndex < ConfColumnsCount)
                {
                    if (nRowIndex >= m_lisConfSheetCell.Count || nColumnIndex >= m_lisConfSheetCell[0].Count)
                    {
                        Debug.LogError("GetConciseCell Error!");
                    }
                    return m_lisConfSheetCell[nRowIndex][nColumnIndex];
                }
                else
                {
                    Debug.LogError(string.Format("GetConciseCell error! rowIndex[{0}] rowCount[{1}] columnIndex[{2}] columnCount[{3}]", nRowIndex, ConfRowsCount, nColumnIndex, ConfColumnsCount));
                    return null;
                }
            }
            catch
            {
                Debug.LogError(string.Format("GetConciseCell error! rowIndex[{0}] rowCount[{1}] columnIndex[{2}] columnCount[{3}]", nRowIndex, ConfRowsCount, nColumnIndex, ConfColumnsCount));
                return null;
            }
        }

        protected void CheckSheetDataType()
        {
            for (int i = 0; i < ColumnsCount; ++i)
            {
                if (!CheckTypeValid<string>(m_sheetData[0, i]))
                {
                    Debug.LogError(string.Format("sheet '{0}' row'{1}' col'{1}' data's type is error!", SheetName, 0, i));
                    break;
                }
                if (!CheckTypeValid<string>(m_sheetData[1, i]))
                {
                    Debug.LogError(string.Format("sheet '{0}' row'{1}' col'{1}' data's type is error!", SheetName, 0, i));
                    break;
                }
                if (!CheckTypeValid<string>(m_sheetData[2, i]))
                {
                    Debug.LogError(string.Format("sheet '{0}' row'{1}' col'{1}' data's type is error!", SheetName, 0, i));
                    break;
                }
            }
        }


        protected override void MakeSheetCellData()
        {
            for (int nRowIndex = 0; nRowIndex < RowsCount; ++nRowIndex)
            {
                List<HrExcelSheetCell> lisExcelSheetRowCell = new List<HrExcelSheetCell>();
                for (int nColumnIndex = 0; nColumnIndex < ColumnsCount; ++nColumnIndex)
                {
                    object data = m_sheetData[nRowIndex, nColumnIndex];

                    HrExcelSheetCell sheetCell = new HrExcelSheetConfCell(GetSheetCellType(nRowIndex), GetCellStringType(nRowIndex, nColumnIndex), data, nRowIndex, nColumnIndex);
                    lisExcelSheetRowCell.Add(sheetCell);
                }
                m_lisExcelSheetCell.Add(lisExcelSheetRowCell);
            }
            
        }

        private void MakeConciseData()
        {
            for (int nRowIndex = 0; nRowIndex < RowsCount; ++nRowIndex)
            {
                List<HrExcelSheetCell> lisExcelSheetRowCell = new List<HrExcelSheetCell>();
                for (int nColumnIndex = 0; nColumnIndex < ColumnsCount; ++nColumnIndex)
                {
                    if (IsColumnsValid(nColumnIndex))
                    {
                        HrExcelSheetConfCell sheetCell = GetCellData(nRowIndex, nColumnIndex) as HrExcelSheetConfCell;
                        if (sheetCell.StrType != "null")
                        {
                            lisExcelSheetRowCell.Add(sheetCell);
                        }
                    }
                }
                m_lisConfSheetCell.Add(lisExcelSheetRowCell);
            }
            ConfRowsCount = m_lisConfSheetCell.Count;
            ConfColumnsCount = m_lisConfSheetCell[m_c_nHeadIndex].Count;
        }

        private string GetCellStringType(int nRowIndex, int nColumnIndex)
        {
            if (nRowIndex < m_c_nHeadRowCount)
            {
                return "string";
            }
            else
            {
                if (m_lisStringType == null)
                {
                    m_lisStringType = GetRowData<string>(m_c_nTypeRowIndex);
                }

                return m_lisStringType[nColumnIndex];
            }
        }

        private bool IsColumnsValid(int nColumnIndex)
        {
            return GetCellStringType(m_c_nHeadRowCount, nColumnIndex) != "null";      
        }

        private HrExcelSheetCellType GetSheetCellType(int nRowIndex)
        {
            switch (nRowIndex)
            {
                case m_c_nHeadIndex:
                    {
                        return HrExcelSheetCellType.CELL_TYPE_TITLE;
                    }
                case m_c_nCommentIndex:
                    {
                        return HrExcelSheetCellType.CELL_TYPE_COMMENT;
                    }
                case m_c_nTypeRowIndex:
                    {
                        return HrExcelSheetCellType.CELL_TYPE_TYPE;
                    }
                default:
                    {
                        return HrExcelSheetCellType.CELL_TYPE_DATA;
                    }
            }
        }


    }
}
