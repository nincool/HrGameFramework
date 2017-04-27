using StaticData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExcelToBinary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnClickOpenExcel(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (!string.IsNullOrEmpty(Mediator.m_strExcelFile))
            {
                ofd.InitialDirectory = Directory.GetCurrentDirectory();
            }
            else
            {
                ofd.InitialDirectory = System.IO.Path.GetDirectoryName(Mediator.m_strExcelFile);
            }

            ofd.Filter = "Excel文件(*.xls;*.xlsx)|*.xls;*.xlsx|所有文件|*.*";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Mediator.m_strExcelFile = ofd.FileName;
                labelSrcExcel.Content = ofd.FileName;
            }
        }

        private void OnClickSelectSavePath(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(Mediator.m_strDestPath))
            {
                ofd.SelectedPath = Mediator.m_strDestPath;
            }
            else if (!string.IsNullOrEmpty(Mediator.m_strExcelFile))
            {
                ofd.SelectedPath = System.IO.Path.GetDirectoryName(Mediator.m_strExcelFile);
            }

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            Mediator.m_strDestPath = ofd.SelectedPath.Trim();
            textSavePath.Text = Mediator.m_strDestPath;
        }

        private void OnClickTranslate(object sender, RoutedEventArgs e)
        {

            Mediator.Translate();
        }

        private void OnClickReadByteData(object sender, RoutedEventArgs e)
        {
            Mediator.ReadData();
        }
    }
}
