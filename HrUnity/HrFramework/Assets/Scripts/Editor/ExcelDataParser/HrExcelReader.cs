using Excel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Hr.Utility;


namespace Hr.Editor
{
    public class HrExcelReader
    {
        /// <summary>
        /// Excel文件名称
        /// </summary>
        public string ExcelName
        {
            get;
            set;
        }

        /// <summary>
        /// Excel文件路径
        /// </summary>
        public string ExcelFilePath
        {
            get;
            set;
        }

        public int SheetCount
        {
            get;
            protected set;
        }

        public bool IsValidFilePath
        {
            get
            {
                if (HrFileUtil.GetFileSuffix(ExcelFilePath) != "xlsx")
                {
                    Debug.LogError(string.Format("the file[{0}]'s type is not 'xlsx'!", ExcelFilePath));
                    return false;
                }
                if (!File.Exists(ExcelFilePath))
                {
                    Debug.LogError(string.Format("the file[{0}] is not existed!", ExcelFilePath));
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Excel 数据
        /// </summary>
        protected List<HrExcelSheet> m_lisSheetData = new List<HrExcelSheet>();

        public List<HrExcelSheet> LisSheetData
        {
            get
            {
                return m_lisSheetData;
            }
        }

        public HrExcelReader(string strExcelFilePath)
        {
            ExcelFilePath = strExcelFilePath;
            if (Path.GetExtension(ExcelFilePath) != ".xlsx")
            {
                Debug.LogError(string.Format("invalid file! {0}", strExcelFilePath));
            }
            ExcelName = HrFileUtil.GetFileSuffix(ExcelFilePath);
        }

        public virtual bool ReadExcelFile()
        {
            return true;
        }
    }

}
