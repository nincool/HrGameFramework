using Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hr.Editor
{
    public class HrExcelExcelReader : HrExcelReader
    {
        public HrExcelExcelReader(string strExcelFilePath) : base(strExcelFilePath)
        {

        }

        public override bool ReadExcelFile()
        {
            if (!IsValidFilePath)
            {
                return false;
            }
            try
            {
                FileStream fileStream = File.Open(ExcelFilePath, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);

                SheetCount = excelReader.ResultsCount;

                System.Data.DataSet result = excelReader.AsDataSet();
                for (int i = 0; i < SheetCount; ++i)
                {
                    System.Data.DataTable table = result.Tables[i];
                    string strTableName = table.TableName;
                    int nColumns = table.Columns.Count;
                    int nRows = table.Rows.Count;

                    object[,] sheetData = new object[nRows, nColumns]; 
                    for (int nRowIndex = 0; nRowIndex < nRows; ++nRowIndex)
                    {
                        for (int nColumIndex = 0; nColumIndex < nColumns; ++nColumIndex)
                        {
                            object cellData = table.Rows[nRowIndex][nColumIndex];
                            sheetData[nRowIndex, nColumIndex] = cellData;
                        }
                    }

                    HrExcelSheet hrSheet = new HrExcelSheet(strTableName, nColumns, nRows, sheetData);
                    hrSheet.InitSheetCellData();
                    m_lisSheetData.Add(hrSheet);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return true;
            
        }
    }

    public class HrExcelExcelConfReader : HrExcelReader
    {
        public HrExcelExcelConfReader(string strExcelFilePath) : base(strExcelFilePath)
        {

        }

        public override bool ReadExcelFile()
        {
            if (!IsValidFilePath)
            {
                return false;
            }
            try
            {
                FileStream fileStream = File.Open(ExcelFilePath, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);

                SheetCount = excelReader.ResultsCount;

                System.Data.DataSet result = excelReader.AsDataSet();
                for (int i = 0; i < SheetCount; ++i)
                {
                    System.Data.DataTable table = result.Tables[i];
                    string strTableName = table.TableName;
                    int nColumns = table.Columns.Count;
                    int nRows = table.Rows.Count;

                    object[,] sheetData = new object[nRows, nColumns];
                    for (int nRowIndex = 0; nRowIndex < nRows; ++nRowIndex)
                    {
                        for (int nColumIndex = 0; nColumIndex < nColumns; ++nColumIndex)
                        {
                            object cellData = table.Rows[nRowIndex][nColumIndex];
                            sheetData[nRowIndex, nColumIndex] = cellData;
                        }
                    }

                    HrExcelSheet hrSheet = new HrExcelSheetConf(strTableName, nColumns, nRows, sheetData);
                    hrSheet.InitSheetCellData();
                    m_lisSheetData.Add(hrSheet);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return true;

        }
    }
}
