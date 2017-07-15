using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Resource
{

    public sealed class HrResourceBinary : HrResource
    {
        private List<List<object>> m_lisObjectData;

        private List<string> m_lisHeadName = new List<string>();
        public List<string> HeadNames
        {
            get
            {
                return m_lisHeadName;
            }
        }

        private List<string> m_lisStringType = new List<string>();
        public List<string> StringTypes
        {
            get
            {
                return m_lisStringType;
            }
        }

        public List<List<object>> DataContent
        {
            get
            {
                return m_lisObjectData;
            }
        }

        public int ColumnCount
        {
            get
            {
                return m_lisHeadName.Count;
            }
        }

        public int DataRowCount
        {
            get
            {
                return m_lisObjectData.Count - 3;
            }
        }



        public HrResourceBinary(string strAssetName, UnityEngine.Object o, HrAssetFile assetFile) : base(strAssetName, o, assetFile)
        {
            ParseData();
        }

        private void ParseData()
        {
            if (RefAssetFile == null)
            {
                HrLogger.LogError(string.Format("parse binary file error! the AssetBinary ref is invalid! [{0}]", AssetName));
                return;
            }
            HrAssetBinary assetBinary = RefAssetFile as HrAssetBinary;
            m_lisObjectData = assetBinary.SheetData.HrTryGet(AssetName);

            for (int i = 0; i < m_lisObjectData[0].Count; ++i)
            {
                m_lisHeadName.Add(m_lisObjectData[0][i] as string);
                m_lisStringType.Add(m_lisObjectData[2][i] as string);
            }
        }

        public object GetDataValue(int nRow, int nCol)
        {
            if (nRow + 3 < m_lisObjectData.Count && nCol < ColumnCount)
            {
                return m_lisObjectData[nRow + 3][nCol];

            }
            else
            {
                HrLogger.LogError(string.Format("get data error! row[{0}][{1}] col[{2}][{3}]", nRow + 3, m_lisObjectData.Count, nCol, ColumnCount));
                return null;
            }
        }
        
    }
}
