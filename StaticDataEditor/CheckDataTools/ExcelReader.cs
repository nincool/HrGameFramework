using System;
using NPOI.SS.UserModel;

namespace CheckDataTools
{
    class ExcelReader
    {
        public ExcelReader(String excelName)
        {
            if (!System.IO.File.Exists(excelName))
            {
                excelName = Environment.CurrentDirectory + excelName;
            }
            m_bIsOpen = OpenExcel(excelName);

            if (!m_bIsOpen)
                throw new Exception("读取" + excelName + "文件失败！");
        }

        ~ExcelReader()
        {
            m_WorkBook = null;
            if (m_ExcelStream != null)
            {
                m_ExcelStream.Close();
                m_ExcelStream = null;
            }
        }

        private System.IO.FileStream m_ExcelStream = null;
        private IWorkbook m_WorkBook = null;

        public int SheetCount
        {
            get
            {
                if (m_WorkBook != null)
                {
                    return m_WorkBook.NumberOfSheets;
                }

                return 0;
            }
        }

        public ISheet GetSheet(int index)
        {
            return m_WorkBook.GetSheetAt(index);
        }

        public bool OpenExcel(string excelName)
        {
            try
            {
                m_ExcelStream = System.IO.File.OpenRead(excelName);
                if (excelName.EndsWith(".xls"))
                {
                    m_WorkBook = new NPOI.HSSF.UserModel.HSSFWorkbook(m_ExcelStream);
                }
                else if (excelName.EndsWith(".xlsx"))
                {
                    m_WorkBook = new NPOI.XSSF.UserModel.XSSFWorkbook(m_ExcelStream);
                }

                if (m_WorkBook == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool m_bIsOpen = false;
        public bool IsOpen
        {
            get
            {
                return m_bIsOpen;
            }
        }

        public void Close()
        {
            try
            {
                m_ExcelStream.Close();
                m_WorkBook = null;
            }
            catch (Exception)
            {

            }
        }
    }
}
