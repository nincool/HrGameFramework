using StaticData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExcelToBinary
{
    class Mediator
    {
        public static string m_strExcelFile = null;
        public static string m_strDestPath = null;

        private static string m_strDestFile = "E:\\Workspace\\HrProjects\\WPF\\ExcelToBinary\\ExcelToBinary\\bin\\Config.byte";
        public static void Translate()
        {
            Excel2Bytes excel = new Excel2Bytes();
            string result = string.Empty;
            string strExcelFileName = Path.GetFileNameWithoutExtension(m_strExcelFile);
            m_strDestFile = m_strDestPath + "\\" + strExcelFileName + ".byte";
            bool success = excel.Handle(m_strExcelFile, m_strDestFile, ref result);

            if (success)
            {
                if (System.Windows.MessageBox.Show("转换成功", "通知", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                }
            }
        }

        public static void ReadData()
        {
            HrConfData cfData = new HrConfData();
            cfData.Load(m_strDestFile);
        }
    }
}
