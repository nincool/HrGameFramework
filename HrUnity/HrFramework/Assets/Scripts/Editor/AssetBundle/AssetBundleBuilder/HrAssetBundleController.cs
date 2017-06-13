using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hr.Editor
{
    public class HrAssetBundleController
    {
        private const string m_c_strConfigurationName = "HrFramework/Configs/AssetBundleBuilder.json";
        private const string m_c_strAssetsConfigName = "HrFramework/Configs/AssetsList.xlsx";
        private const string m_c_strAssetsListJsonName = "AssetsList.json";

        private HrAssetBundleContainer m_assetBundleContainer = new HrAssetBundleContainer();


        public string ProductName
        {
            get
            {
                return PlayerSettings.productName;
            }
        }

        public string CompanyName
        {
            get
            {
                return PlayerSettings.companyName;
            }
        }


        public string GameIdentifier
        {
            get
            {
#if UNITY_5_6_OR_NEWER
                return PlayerSettings.applicationIdentifier;
#else
                return PlayerSettings.bundleIdentifier;
#endif
            }
        }


        #region Build Target
        public bool WindowsSelected
        {
            get;
            set;
        }

        public bool MacOSXSelected
        {
            get;
            set;
        }

        public bool IOSSelected
        {
            get;
            set;
        }

        public bool AndroidSelected
        {
            get;
            set;
        }

        public bool WindowsStoreSelected
        {
            get;
            set;
        }
        #endregion

        #region Build Options
        public bool UncompressedAssetBundleSelected
        {
            get;
            set;
        }

        //ChunkBasedCompression是一种基于Chunk的LZ4压缩方式
        public bool ChunkBasedCompressionSelected
        {
            get;
            set;
        }

        public bool DisableWriteTypeTreeSelected
        {
            get;
            set;
        }

        public bool IgnoreTypeTreeChangesSelected
        {
            get;
            set;
        }

        public bool DeterministicAssetBundleSelected
        {
            get;
            set;
        }

        public bool ForceRebuildAssetBundleSelected
        {
            get;
            set;
        }

        public bool AppendHashToAssetBundleNameSelected
        {
            get;
            set;
        }

        #endregion

        public bool ZipSelected
        {
            get;
            set;
        }

        public string ApplicableGameVersion
        {
            get
            {
                return Application.version;
            }
        }

        public int InternalResourceVersion
        {
            get;
            set;
        }

        public string UnityVersion
        {
            get
            {
                return Application.unityVersion;
            }
        }

        public string OutputDirectory
        {
            get;
            set;
        }

        public bool IsValidOutputDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(OutputDirectory))
                {
                    return false;
                }

                if (!Directory.Exists(OutputDirectory))
                {
                    return false;
                }

                return true;
            }
        }

        public string WorkingPath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return string.Format("{0}/Working/", OutputDirectory);
            }
        }

        public string AssetsListPath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return string.Format("{0}/AssetList/{1}_{2}/", OutputDirectory, ApplicableGameVersion.Replace('.', '_'), InternalResourceVersion.ToString());
            }
        }

        public string AllAssetBundlesPath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return string.Format("{0}/AllAssetBundle/{1}_{2}/", OutputDirectory, ApplicableGameVersion.Replace('.', '_'), InternalResourceVersion.ToString());
            }
        }

        public string OutputPackedPath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return string.Format("{0}/Packed/{1}_{2}/", OutputDirectory, ApplicableGameVersion.Replace('.', '_'), InternalResourceVersion.ToString());
            }
        }

        public string ModifiedAssetBundlesPath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return string.Format("{0}/ModifiedAssetBundles/{1}_{2}", OutputDirectory, ApplicableGameVersion.Replace('.', '_'), InternalResourceVersion.ToString());
            }
        }

        public string BuildReportPath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return string.Format("{0}/BuildReport/{1}_{2}/", OutputDirectory, ApplicableGameVersion.Replace('.', '_'), InternalResourceVersion.ToString());
            }
        }

        public string EditorResourcePath
        {
            get;
            set;
        }

        public HrAssetBundleController()
        {
            Load();
        }

        public bool Save()
        {
            string strConfigurationName = HrFileUtil.GetCombinePath(Application.dataPath, m_c_strConfigurationName);
            try
            {
                JsonWriter writer = new JsonWriter();

                writer.WriteObjectStart();

                writer.WritePropertyName("InternalResourceVersion");
                writer.Write(InternalResourceVersion);

                writer.WritePropertyName("WindowsSelected");
                writer.Write(WindowsSelected);
                writer.WritePropertyName("MacOSXSelected");
                writer.Write(MacOSXSelected);
                writer.WritePropertyName("IOSSelected");
                writer.Write(IOSSelected);
                writer.WritePropertyName("AndroidSelected");
                writer.Write(AndroidSelected);
                writer.WritePropertyName("WindowsStoreSelected");
                writer.Write(WindowsStoreSelected);

                writer.WritePropertyName("UncompressedAssetBundleSelected");
                writer.Write(UncompressedAssetBundleSelected);
                writer.WritePropertyName("DisableWriteTypeTreeSelected");
                writer.Write(DisableWriteTypeTreeSelected);
                writer.WritePropertyName("DeterministicAssetBundleSelected");
                writer.Write(DeterministicAssetBundleSelected);
                writer.WritePropertyName("ForceRebuildAssetBundleSelected");
                writer.Write(ForceRebuildAssetBundleSelected);
                writer.WritePropertyName("IgnoreTypeTreeChangesSelected");
                writer.Write(IgnoreTypeTreeChangesSelected);
                writer.WritePropertyName("AppendHashToAssetBundleNameSelected");
                writer.Write(AppendHashToAssetBundleNameSelected);
                writer.WritePropertyName("ChunkBasedCompressionSelected");
                writer.Write(ChunkBasedCompressionSelected);

                writer.WritePropertyName("OutputDirectory");
                writer.Write(OutputDirectory);

                writer.WritePropertyName("EditorResourcePath");
                writer.Write(EditorResourcePath);

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

            InternalResourceVersion = (int)jsonData["InternalResourceVersion"];

            WindowsSelected = (bool)jsonData["WindowsSelected"];
            MacOSXSelected = (bool)jsonData["MacOSXSelected"];
            IOSSelected = (bool)jsonData["IOSSelected"];
            AndroidSelected = (bool)jsonData["AndroidSelected"];
            WindowsStoreSelected = (bool)jsonData["WindowsStoreSelected"];

            UncompressedAssetBundleSelected = (bool)jsonData["UncompressedAssetBundleSelected"];
            DisableWriteTypeTreeSelected = (bool)jsonData["DisableWriteTypeTreeSelected"];
            DeterministicAssetBundleSelected = (bool)jsonData["DeterministicAssetBundleSelected"];
            ForceRebuildAssetBundleSelected = (bool)jsonData["ForceRebuildAssetBundleSelected"];
            IgnoreTypeTreeChangesSelected = (bool)jsonData["IgnoreTypeTreeChangesSelected"];
            AppendHashToAssetBundleNameSelected = (bool)jsonData["AppendHashToAssetBundleNameSelected"];
            ChunkBasedCompressionSelected = (bool)jsonData["ChunkBasedCompressionSelected"];

            OutputDirectory = jsonData["OutputDirectory"].ToString();
            EditorResourcePath = jsonData["EditorResourcePath"].ToString();

            m_assetBundleContainer.Load();

            return true;
        }

        public bool BuildAssetBundles()
        {
            if (!IsValidOutputDirectory)
            {
                return false;
            }

            //删除文件夹下的所有文件 
            if (Directory.Exists(WorkingPath))
            {
                HrFileUtil.DelectDir(WorkingPath);
            }

            BuildAssetBundleOptions buildAssetBundleOptions = GetBuildAssetBundleOptions();

            var lisBuildMap = GetBuildMap();

            BuildTarget buildTarget = BuildTarget.StandaloneWindows;
            if (WindowsSelected)
            {
                buildTarget = BuildTarget.StandaloneWindows;
                BuildAssetBundles(buildTarget, lisBuildMap, buildAssetBundleOptions);
            }

            if (MacOSXSelected)
            {
                buildTarget = BuildTarget.StandaloneOSXIntel;
                BuildAssetBundles(buildTarget, lisBuildMap, buildAssetBundleOptions);
            }

            if (IOSSelected)
            {
                buildTarget = BuildTarget.iOS;
                BuildAssetBundles(buildTarget, lisBuildMap, buildAssetBundleOptions);
            }

            if (AndroidSelected)
            {
                buildTarget = BuildTarget.Android;
                BuildAssetBundles(buildTarget, lisBuildMap, buildAssetBundleOptions);
            }

            if (WindowsStoreSelected)
            {
                buildTarget = BuildTarget.WSAPlayer;
                BuildAssetBundles(buildTarget, lisBuildMap, buildAssetBundleOptions);
            }

            CopyAssetBundlesToAllAssetBundlePath();
            CreateAssetsListInfo();
            CopyAssetBundlesToEditorPath();

            return true;
        }

        private void CopyAssetBundlesToEditorPath()
        {
            if (string.IsNullOrEmpty(EditorResourcePath))
            {
                return;
            }
            if (Directory.Exists(EditorResourcePath))
            {
                Directory.Delete(EditorResourcePath, true);
            }

            if (!Directory.Exists(AllAssetBundlesPath))
            {
                Directory.CreateDirectory(AllAssetBundlesPath);
            }

            HrFileUtil.CopyDirectory(AllAssetBundlesPath, EditorResourcePath);
            HrFileUtil.CopyDirectory(AssetsListPath, EditorResourcePath);
        }

        /// <summary>
        /// 拷贝AssetBundles到AllAssetBundlePath中
        /// </summary>
        private void CopyAssetBundlesToAllAssetBundlePath()
        {
            if (!Directory.Exists(AllAssetBundlesPath))
            {
                Directory.CreateDirectory(AllAssetBundlesPath);
            }

            EditorUtility.DisplayProgressBar("build", "copy assetbundles to all assetbundlepath", 0.5f);
            {
                HrFileUtil.CopyDirectory(WorkingPath, AllAssetBundlesPath);
            }
            EditorUtility.ClearProgressBar();
        }

        private void ReadAssetsListExcelFile(out Dictionary<string, Dictionary<int, string>> dicAssetsListExcel)
        {
            dicAssetsListExcel = new Dictionary<string, Dictionary<int, string>>();
            string strAssetsListFile = HrFileUtil.GetCombinePath(Application.dataPath, m_c_strAssetsConfigName);
            if (File.Exists(strAssetsListFile))
            {
                HrExcelExcelReader excelReader = new HrExcelExcelReader(strAssetsListFile);
                excelReader.ReadExcelFile();
                int nSheetCount = excelReader.SheetCount;
                for (int nSheetIndex = 0; nSheetIndex < nSheetCount; ++nSheetIndex)
                {
                    HrExcelSheet sheetData = excelReader.LisSheetData[nSheetIndex];
                    string strSheetName = sheetData.SheetName;
                    int nRowsCount = sheetData.RowsCount;
                    int nColumnsCount = sheetData.ColumnsCount;
                    Assert.IsTrue(nColumnsCount == 2);

                    Dictionary<int, string> dicAssetsListExcelSheet = new Dictionary<int, string>();

                    //RowIndex = 0 代表Title
                    for (int nRowIndex = 1; nRowIndex < nRowsCount; ++nRowIndex)
                    {
                        HrExcelSheetCell sheetCell = sheetData.GetCellData(nRowIndex, 0);
                        int nID = sheetCell.GetData<int>();

                        sheetCell = sheetData.GetCellData(nRowIndex, 1);
                        string strFilePath = sheetCell.GetData<string>();

                        dicAssetsListExcelSheet.Add(nID, strFilePath);
                    }

                    dicAssetsListExcel.Add(strSheetName, dicAssetsListExcelSheet);
                }
            }
            else
            {
                Debug.LogError(string.Format("assets list config file is not existed! filepath:{0}", strAssetsListFile));
            }
        }

        private void ReadAssetBundleManifestInfo(out Dictionary<string, List<string>> dicAssetBundleAssetsList
            , out Dictionary<string, string> dicAssetInAssetBundle
            , out Dictionary<string, List<string>> dicAssetBundleDependices)
        {
            dicAssetBundleAssetsList = new Dictionary<string, List<string>>();
            dicAssetInAssetBundle = new Dictionary<string, string>();
            dicAssetBundleDependices = new Dictionary<string, List<string>>();

            string strAssetBundlePlatFolderName = "";
            if (WindowsSelected)
            {
                strAssetBundlePlatFolderName = "windows";
            }
            else if (MacOSXSelected)
            {
            }
            else if (IOSSelected)
            {
            }
            else if (AndroidSelected)
            {
                strAssetBundlePlatFolderName = "android";
            }
            else if (WindowsStoreSelected)
            {
            }

            string strManifestFilePath = HrFileUtil.GetCombinePath(AllAssetBundlesPath, strAssetBundlePlatFolderName, strAssetBundlePlatFolderName);
            if (!File.Exists(strManifestFilePath))
            {
                Debug.LogError(string.Format("create assets list error! can not find the manifestfile! path:{0}", strManifestFilePath));
                return;
            }
            AssetBundle abManifest = AssetBundle.LoadFromFile(strManifestFilePath);
            AssetBundleManifest manifest = abManifest.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            abManifest.Unload(false);

            //获取所有的AssetBundle
            var lisAllAssetBundleNames = manifest.GetAllAssetBundles().ToList<string>();
            //查找每个AssetBundle的依赖项
            foreach (var strAssetBundleName in lisAllAssetBundleNames)
            {
                var lisDependicesArr = manifest.GetAllDependencies(strAssetBundleName).ToList<string>();
                if (lisDependicesArr.Count > 0)
                {
                    dicAssetBundleDependices.Add(strAssetBundleName, lisDependicesArr);
                }

                //读取每个AssetBundle的Manifest文件，读取里面的Assets Path
                string strAssetBundleManifest = HrFileUtil.GetCombinePath(Path.GetDirectoryName(strManifestFilePath), string.Format("{0}.manifest", strAssetBundleName));
                if (!File.Exists(strAssetBundleManifest))
                {
                    Debug.LogError(string.Format("can not fine the manifest file! {0}", strAssetBundleManifest));
                    continue;
                }

                string strAssetBundleFilePath = HrFileUtil.GetCombinePath(AllAssetBundlesPath, strAssetBundlePlatFolderName, strAssetBundleName);
                var assetBundle = AssetBundle.LoadFromFile(strAssetBundleFilePath);
                var lisAllAssetsList = assetBundle.GetAllAssetNames().ToList<string>();
                dicAssetBundleAssetsList.Add(strAssetBundleName, lisAllAssetsList);
                assetBundle.Unload(false);

                foreach (var strAssetName in lisAllAssetsList)
                {
                    dicAssetInAssetBundle.Add(strAssetName, strAssetBundleName);
                }
            }
        }

        private void CreateAssetsListJsonFile(Dictionary<string, Dictionary<int, string>> dicAssetsListExcel
            , Dictionary<string, List<string>> dicAssetBundleAssetsList
            , Dictionary<string, string> dicAssetInAssetBundle
            , Dictionary<string, List<string>> dicAssetBundleDependencies)
        {
            string strAssetsListJsonFilePath = HrFileUtil.GetCombinePath(AssetsListPath, m_c_strAssetsListJsonName);
            JsonWriter writer = new JsonWriter();
            writer.WriteObjectStart();
            {
                //先写入AssetBundle信息
                int nTempIndex = 0;
                foreach (var assetBundleItem in dicAssetBundleAssetsList)
                {
                    string strKey = string.Format("AssetBundle_{0}", nTempIndex);
                    writer.WritePropertyName(strKey);
                    writer.WriteObjectStart();
                    {
                        writer.WritePropertyName("AssetBundle");
                        writer.Write(assetBundleItem.Key);
                        writer.WritePropertyName("Dependencies");
                        writer.WriteArrayStart();
                        {
                            var lisAssetBundleDependencies = dicAssetBundleDependencies.HrTryGet(assetBundleItem.Key);
                            if (lisAssetBundleDependencies != null)
                            {
                                foreach (var strDependice in lisAssetBundleDependencies)
                                {
                                    writer.Write(strDependice);
                                }
                            }
                        }
                        writer.WriteArrayEnd();
                    }
                    writer.WriteObjectEnd();
                    ++nTempIndex;

                }

                nTempIndex = 0;
                foreach (var sheetAssetInfo in dicAssetsListExcel)
                {
                    foreach (var assetInfo in sheetAssetInfo.Value)
                    {
                        string strAssetBundleName = dicAssetInAssetBundle.HrTryGet(assetInfo.Value.ToLower());
                        if (strAssetBundleName == null)
                        {
                            Debug.LogError(string.Format("check assets list error! can not find the asset '{0}'", assetInfo.Value));
                        }
                        else
                        {
                            string strKey = string.Format("Asset_{0}", nTempIndex);
                            writer.WritePropertyName(strKey);
                            writer.WriteObjectStart();
                            {
                                writer.WritePropertyName("ID");
                                writer.Write(assetInfo.Key);
                                writer.WritePropertyName("FilePath");
                                writer.Write(assetInfo.Value.ToLower());
                                writer.WritePropertyName("AssetBundle");
                                writer.Write(strAssetBundleName);
                            }
                            writer.WriteObjectEnd();
                        }
                        ++nTempIndex;
                    }
                }
            }
            writer.WriteObjectEnd();
            File.WriteAllText(strAssetsListJsonFilePath, writer.ToString(), Encoding.UTF8);
        }

        /// <summary>
        ///  读取AssetsList Excel配置 然后根据AssetBundle的Manifest生成资源AssetsList Json或者Binary
        /// </summary>
        private void CreateAssetsListInfo()
        {
            //1.读取AssetsList.xlsx
            Dictionary<string, Dictionary<int, string>> dicAssetsListExcel; 
            ReadAssetsListExcelFile(out dicAssetsListExcel);

            //2.解析AssetBundle Manifest文件，解析资源文件和依赖项
            Dictionary<string, List<string>> dicAssetBundleAssetsList;
            Dictionary<string, string> dicAssetInAssetBundle;
            Dictionary<string, List<string>> dicAssetBundleDependencies;
            ReadAssetBundleManifestInfo(out dicAssetBundleAssetsList, out dicAssetInAssetBundle, out dicAssetBundleDependencies);

            //3.校验资源 创建Json文件
            CreateAssetsListJsonFile(dicAssetsListExcel, dicAssetBundleAssetsList, dicAssetInAssetBundle, dicAssetBundleDependencies);
        }

        private bool BuildAssetBundles(BuildTarget buildTarget, List<AssetBundleBuild> lisBuildMap, BuildAssetBundleOptions buildAssetBundleOptions)
        {
            string strBuildTargetName = GetBuildTargetName(buildTarget);
            string strWorkingPath = string.Format("{0}{1}/", WorkingPath, strBuildTargetName);

            if (Directory.Exists(strWorkingPath))
            {
                Directory.Delete(strWorkingPath, true);
            }
            if (!Directory.Exists(strWorkingPath))
            {
                Directory.CreateDirectory(strWorkingPath);
            }

            var assetBundleManifest = BuildPipeline.BuildAssetBundles(strWorkingPath, lisBuildMap.ToArray(), buildAssetBundleOptions, buildTarget);
            if (assetBundleManifest == null)
            {
                Debug.LogError(string.Format("Build AssetBundles for '{0}' failure.", buildTarget.ToString()));
                return false;
            }

            
            return true;
        } 

        private BuildAssetBundleOptions GetBuildAssetBundleOptions()
        {
            BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.None;

            if (UncompressedAssetBundleSelected)
            {
                buildOptions |= BuildAssetBundleOptions.UncompressedAssetBundle;
            }

            if (DisableWriteTypeTreeSelected)
            {
                buildOptions |= BuildAssetBundleOptions.DisableWriteTypeTree;
            }

            if (DeterministicAssetBundleSelected)
            {
                buildOptions |= BuildAssetBundleOptions.DeterministicAssetBundle;
            }

            if (ForceRebuildAssetBundleSelected)
            {
                buildOptions |= BuildAssetBundleOptions.ForceRebuildAssetBundle;
            }

            if (IgnoreTypeTreeChangesSelected)
            {
                buildOptions |= BuildAssetBundleOptions.IgnoreTypeTreeChanges;
            }

            if (AppendHashToAssetBundleNameSelected)
            {
                buildOptions |= BuildAssetBundleOptions.AppendHashToAssetBundleName;
            }

            if (ChunkBasedCompressionSelected)
            {
                buildOptions |= BuildAssetBundleOptions.ChunkBasedCompression;
            }

            return buildOptions;
        }

        private List<AssetBundleBuild> GetBuildMap()
        {
            List<AssetBundleBuild> lisAssetBundleBuild = new List<AssetBundleBuild>();

            HrAssetBundle[] assetBundles = m_assetBundleContainer.GetAssetBundles();

            foreach (var assetBundle in assetBundles)
            {
                AssetBundleBuild buildMap = new AssetBundleBuild();
                buildMap.assetBundleName = assetBundle.Name;
                buildMap.assetBundleVariant = assetBundle.Variant;
                buildMap.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundle.FullName);

                lisAssetBundleBuild.Add(buildMap);
            }

            return lisAssetBundleBuild;
        }

        private string GetBuildTargetName(BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows:
                    return "windows";
                case BuildTarget.StandaloneOSXIntel:
                    return "osx";
                case BuildTarget.iOS:
                    return "ios";
                case BuildTarget.Android:
                    return "android";
                case BuildTarget.WSAPlayer:
                    return "winstore";
                default:
                    return "notsupported";
            }
        }
    }
}
