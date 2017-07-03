using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Hr.Utility;



namespace Hr.Editor
{
    public class HrExcelBinaryReader
    {
        private string m_strBinaryFilePath;

        public string FileName
        {
            get;
            private set;
        }

        public int SheetCount
        {
            get;
            private set;
        }

        private List<string> m_lisSheetName;
        public List<string> SheetNames
        {
            get
            {
                return m_lisSheetName;
            }
        }

        public HrExcelBinaryReader(string strBinaryFilePath)
        {
            m_strBinaryFilePath = strBinaryFilePath;
            FileName = Path.GetFileName(strBinaryFilePath);
        }

        public void ReadBinary()
        {
            m_lisSheetName = new List<string>();

            HrByteBufferReader reader = new HrByteBufferReader(m_strBinaryFilePath);
            string strHrFlag = reader.ReadString();
            SheetCount = reader.ReadInt();

            for (int i = 0; i < SheetCount; ++i)
            {
                string strSheetName = reader.ReadString();
                m_lisSheetName.Add(strSheetName);

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

                //读取Excel数据
                for (int nRowIndex = 3; nRowIndex < nRowsCount; ++nRowIndex)
                {
                    for (int nColIndex = 0; nColIndex < nColumnsCount; ++nColIndex)
                    {
                        string strType = lisHeadData[2][nColIndex];
                        if (strType.Equals("int"))
                        {
                            reader.ReadInt();
                        }
                        else if (strType.Equals("uint"))
                        {
                            reader.ReadUInt();
                        }
                        if (strType.Equals("byte"))
                        {
                            reader.ReadByte();
                        }
                        else if (strType.Equals("string"))
                        {
                            reader.ReadString();
                        }
                        else if (strType.Equals("float"))
                        {
                            reader.ReadFloat();
                        }
                    }
                }
            }

            reader.Destory();


        }
    }
}



