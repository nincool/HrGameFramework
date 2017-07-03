using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Hr.Utility;


namespace Hr.Editor
{
    public class HrExcelDataManager
    {
        private const string m_c_strConfigurationName = "HrFramework/Configs/ExcelData.json";
        

        public string SourceDirectory
        {
            get;
            set;
        }

        public bool IsValidSourceDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(SourceDirectory))
                {
                    return false;
                }

                if (!Directory.Exists(SourceDirectory))
                {
                    return false;
                }

                return true;
            }
        }

        public string DestinationDirectory
        {
            get;
            set;
        }

        public bool IsValidDestinationDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(DestinationDirectory))
                {
                    return false;
                }

                if (!Directory.Exists(DestinationDirectory))
                {
                    return false;
                }

                return true;
            }
        }

        public string DataTableJsonPath
        {
            get;
            set;
        }

        public string EditorResourcePath
        {
            get;
            set;
        }

        public HrExcelDataManager()
        {
            Load();
        }

        public bool Load()
        {
            string strConfigurationName = HrFileUtil.GetCombinePath(Application.dataPath, m_c_strConfigurationName);
            if (!File.Exists(strConfigurationName))
            {
                return false;
            }

            string strData = File.ReadAllText(strConfigurationName);
            JsonData jsonData = JsonMapper.ToObject(strData);
            IDictionary dicJsonData = jsonData as IDictionary;
            if (dicJsonData == null)
            {
                return false;
            }

            SourceDirectory = jsonData["SourceDirectory"].ToString();
            DestinationDirectory = jsonData["DestinationDirection"].ToString();
            EditorResourcePath = jsonData["EditorResourcePath"].ToString();
            DataTableJsonPath = jsonData["DataTableJsonPath"].ToString();

            return true;
        }

        public bool Save()
        {
            string strConfigurationName = HrFileUtil.GetCombinePath(Application.dataPath, m_c_strConfigurationName);
            try
            {
                JsonWriter writer = new JsonWriter();

                writer.WriteObjectStart();

                writer.WritePropertyName("SourceDirectory");
                writer.Write(SourceDirectory);
                writer.WritePropertyName("DestinationDirection");
                writer.Write(DestinationDirectory);
                writer.WritePropertyName("EditorResourcePath");
                writer.Write(EditorResourcePath);
                writer.WritePropertyName("DataTableJsonPath");
                writer.Write(DataTableJsonPath);
                
                writer.WriteObjectEnd();

                File.WriteAllText(strConfigurationName, writer.ToString(), Encoding.UTF8);
            }
            catch
            {
                Debug.LogError(string.Format("AssetBundleController SaveConfiguration '{0}' failed!", m_c_strConfigurationName));
                return false;
            }

            return true;
        }

        public bool TranslateExcel2Binary()
        {
            var strFilePathArr = HrFileUtil.GetAllFilePathsInFolder(SourceDirectory);
            foreach (var item in strFilePathArr)
            {
                if (HrFileUtil.GetFileSuffix(item) == "xlsx")
                {
                    HrExcelConverter excelConvert = new HrExcelConverter();

                    string strDestinationFilePath = DestinationDirectory + "/" + Path.GetFileNameWithoutExtension(item) + ".hrbytes";
                    excelConvert.Convert(item, strDestinationFilePath);
                }
            }

            CreateDataTableInfoJson();

            return true;
        }

        private void CreateDataTableInfoJson()
        {
            JsonWriter writer = new JsonWriter();

            writer.WriteObjectStart();

            var strFilePathArr = HrFileUtil.GetAllFilePathsInFolder(DestinationDirectory);

            int nTempIndex = 0;
            foreach (var item in strFilePathArr)
            {
                if (HrFileUtil.GetFileSuffix(item) == "hrbytes")
                {
                    HrExcelBinaryReader reader = new HrExcelBinaryReader(item);
                    reader.ReadBinary();

                    string strSheetName = string.Format("SheetName_{0}", nTempIndex);

                    writer.WritePropertyName(strSheetName);
                    writer.WriteObjectStart();

                    writer.WritePropertyName("Name");
                    writer.Write(reader.FileName);

                    writer.WritePropertyName("SheetName");
                    writer.WriteArrayStart();
                    for (int i = 0; i < reader.SheetNames.Count; ++i)
                    {
                        writer.Write(reader.SheetNames[i]);
                    }
                    writer.WriteArrayEnd();

                    writer.WriteObjectEnd();

                    ++nTempIndex;
                }
            }

            writer.WriteObjectEnd();

            
            File.WriteAllText(DataTableJsonPath + "DataTableConfig.json", writer.ToString(), Encoding.UTF8);
        }

        public bool Read()
        {
            var strFilePathArr = HrFileUtil.GetAllFilePathsInFolder(DestinationDirectory);
            foreach (var item in strFilePathArr)
            {
                if (HrFileUtil.GetFileSuffix(item) == "hrbytes")
                {
                    HrExcelBinaryReader reader = new HrExcelBinaryReader(item);
                    reader.ReadBinary();
                }
            }

            return true;
        }

        public void CopyDataTableToEditorFolder()
        {
            string strDestinationFilePath = DestinationDirectory + "/";
            HrFileUtil.CopyDirectory(strDestinationFilePath, EditorResourcePath);

            HrFileUtil.CopyDirectory(DataTableJsonPath, EditorResourcePath);
        }

    }
}
