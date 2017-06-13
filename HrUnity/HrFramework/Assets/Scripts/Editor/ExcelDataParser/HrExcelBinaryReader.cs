using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Hr.Editor
{
    public class HrExcelBinaryReader
    {
        private string m_strBinaryFilePath;


        public HrExcelBinaryReader(string strBinaryFilePath)
        {
            m_strBinaryFilePath = strBinaryFilePath;
        }

        public void ReadBinary()
        {
            HrByteBufferReader reader = new HrByteBufferReader(m_strBinaryFilePath);
            string strHrFlag = reader.ReadString();
            int nSheetCount = reader.ReadInt();

            for (int i = 0; i < nSheetCount; ++i)
            {
                string strSheetName = reader.ReadString();
                int nRowsCount = reader.ReadInt();
                int nColumnsCount = reader.ReadInt();

                //读取Excel头
                List <List <string>> lisHeadData = new List<List<string>>();
                for (int nRowIndex = 0; nRowIndex < 3; ++nRowIndex)
                {
                    List<string> lisRowData = new List<string>();
                    for (int nColIndex = 0; nColIndex < nColumnsCount; ++nColIndex)
                    {
                        lisRowData.Add(reader.ReadString());
                    }
                    lisHeadData.Add(lisRowData);
                }

                Debug.Log(lisHeadData);

                //读取Excel数据
                
            }
            
        }
    }
}



