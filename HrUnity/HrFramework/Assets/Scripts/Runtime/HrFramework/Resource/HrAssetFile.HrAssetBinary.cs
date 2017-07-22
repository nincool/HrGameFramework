using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hr.Utility;


namespace Hr.Resource
{
    public class HrAssetBinary : HrAssetFile
    {
        private List<string> m_lisDataTable = new List<string>();

        public List<string> DataTables
        {
            get
            {
                return m_lisDataTable;
            }
        }

        private List<string> m_lisSheetName = new List<string>();
        public List<string> SheetNames
        {
            get
            {
                return m_lisSheetName;
            }
        }

        private Dictionary<string, List<List<object>>> m_dicSheetData = new Dictionary<string, List<List<object>>>();
        public Dictionary<string, List<List<object>>> SheetData
        {
            get
            {
                return m_dicSheetData;
            }
        }

        public HrAssetBinary(string strName, string strFullPath) : base(strName, strFullPath)
        {
        }

        public override void LoadSync()
        {
            HrSimpleTimeCounter timeCounter = new HrSimpleTimeCounter();

            HrByteBufferReader bufferReader = new HrByteBufferReader(m_strFullPath);
            string strHrFlag = bufferReader.ReadString();
            int nSheetCount = bufferReader.ReadInt();

            for (int i = 0; i < nSheetCount; ++i)
            {
                string strSheetName = bufferReader.ReadString();
                m_lisSheetName.Add(strSheetName);

                int nRowsCount = bufferReader.ReadInt();
                int nColumnsCount = bufferReader.ReadInt();

                //读取Excel头
                List<List<object>> lisSheetData = new List<List<object>>();
                for (int nRowIndex = 0; nRowIndex < 3; ++nRowIndex)
                {
                    List<object> lisRowData = new List<object>();
                    for (int nColIndex = 0; nColIndex < nColumnsCount; ++nColIndex)
                    {
                        lisRowData.Add(bufferReader.ReadString());
                    }
                    lisSheetData.Add(lisRowData);
                }

                //读取Excel数据
                for (int nRowIndex = 3; nRowIndex < nRowsCount; ++nRowIndex)
                {
                    List<object> lisRowData = new List<object>();
                    for (int nColIndex = 0; nColIndex < nColumnsCount; ++nColIndex)
                    {
                        string strType = lisSheetData[2][nColIndex] as string;
                        if (strType.Equals("int"))
                        {
                            lisRowData.Add(bufferReader.ReadInt());
                        }
                        else if (strType.Equals("uint"))
                        {
                            lisRowData.Add(bufferReader.ReadUInt());
                        }
                        if (strType.Equals("byte"))
                        {
                            lisRowData.Add(bufferReader.ReadByte());
                        }
                        else if (strType.Equals("string"))
                        {
                            lisRowData.Add(bufferReader.ReadString());
                        }
                        else if (strType.Equals("float"))
                        {
                            lisRowData.Add(bufferReader.ReadFloat());
                        }
                    }
                    lisSheetData.Add(lisRowData);
                }

                m_dicSheetData.Add(strSheetName, lisSheetData);
            }

            bufferReader.Destory();

            m_assetBundleLoadMode = EnumAssetBundleLoadMode.LOAD_SYNC;
            m_assetBundleStatus = EnumAssetBundleStatus.LOADED;

            if (m_loadAssetBundleEvent != null)
            {
                m_loadAssetBundleEvent.TriggerLoadSuccess(this, Name, this, timeCounter.GetTimeElapsed());
            }
        }

        public override IEnumerator LoadAsync()
        {
            yield break;
        }

    }
}
