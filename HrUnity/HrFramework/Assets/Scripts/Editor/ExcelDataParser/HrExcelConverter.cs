using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hr.Utility;


namespace Hr.Editor
{
    public class HrExcelConverter
    {

        public bool Convert(string strExcelFilePath, string strBinaryDirectory)
        {
            ///todo
            HrExcelReader excelReader = new HrExcelExcelConfReader(strExcelFilePath);
            excelReader.ReadExcelFile();

            HrByteBufferWriter bufferWriter = new HrByteBufferWriter(strBinaryDirectory);

            string strHrBinaryFile = "World Of Passkey";
            bufferWriter.WriteString(strHrBinaryFile);
            bufferWriter.WriteInt(excelReader.SheetCount);
            ConvertSheetData(bufferWriter, excelReader.LisSheetData);

            bufferWriter.SaveFile();
            bufferWriter.Destory();
            return true;
        }

        private void ConvertSheetData(HrByteBufferWriter bufferWriter, List<HrExcelSheet> lisSheetData)
        {
            for (int i = 0; i < lisSheetData.Count; ++i)
            {
                CovertOneSheet(bufferWriter, lisSheetData[i]);
            }
        }

        private void CovertOneSheet(HrByteBufferWriter bufferWriter, HrExcelSheet sheetData)
        {
            HrExcelSheetConf confSheetData=  sheetData as HrExcelSheetConf;
            //SheetName
            bufferWriter.WriteString(confSheetData.SheetName);
            //行数
            bufferWriter.WriteInt(confSheetData.ConfRowsCount);
            //列数
            bufferWriter.WriteInt(confSheetData.ConfColumnsCount);

            for (int nRowIndex = 0; nRowIndex < confSheetData.ConfRowsCount; ++nRowIndex)
            {
                for (int nColumnIndex = 0; nColumnIndex < confSheetData.ConfColumnsCount; ++nColumnIndex)
                {
                    HrExcelSheetCell cellData = confSheetData.GetConciseCell(nRowIndex, nColumnIndex);
                    
                    if (cellData.SystemType == typeof(int))
                    {
                        bufferWriter.WriteInt(cellData.GetData<int>());
                    }
                    else if (cellData.SystemType == typeof(uint))
                    {
                        bufferWriter.WriteUInt(cellData.GetData<uint>());
                    }
                    else if (cellData.SystemType == typeof(byte))
                    {
                        bufferWriter.WriteByte(cellData.GetData<byte>());
                    }
                    else if (cellData.SystemType == typeof(float))
                    {
                        bufferWriter.WriteFloat(cellData.GetData<float>());
                    }
                    else if (cellData.SystemType == typeof(string))
                    {
                        bufferWriter.WriteString(cellData.GetData<string>());
                    }
                }
            }
        }
    }
}
