using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    public class HrAssetBundleController
    {
        private const string m_c_strConfigurationName = "HrFramework/Configs/AssetBundleBuilder.json";

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

        public string OutputPackagePath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return string.Format("{0}/Package/{1}_{2}/", OutputDirectory, ApplicableGameVersion.Replace('.', '_'), InternalResourceVersion.ToString());
            }
        }

        public string OutputFullPath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return string.Format("{0}/Full/{1}_{2}/", OutputDirectory, ApplicableGameVersion.Replace('.', '_'), InternalResourceVersion.ToString());
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

            m_assetBundleContainer.Load();

            return true;
        }

        public bool BuildAssetBundles()
        {
            if (!IsValidOutputDirectory)
            {
                return false;
            }


            if (Directory.Exists(OutputPackagePath))
            {
                Directory.Delete(OutputPackagePath, true);
            }

            Directory.CreateDirectory(OutputPackagePath);

            if (Directory.Exists(OutputFullPath))
            {
                Directory.Delete(OutputFullPath, true);
            }

            Directory.CreateDirectory(OutputFullPath);

            if (Directory.Exists(OutputPackedPath))
            {
                Directory.Delete(OutputPackedPath, true);
            }

            Directory.CreateDirectory(OutputPackedPath);

            if (Directory.Exists(BuildReportPath))
            {
                Directory.Delete(BuildReportPath, true);
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

            return true;
        }

        private bool BuildAssetBundles(BuildTarget buildTarget, List<AssetBundleBuild> lisBuildMap, BuildAssetBundleOptions buildAssetBundleOptions)
        {
            string strBuildTargetName = GetBuildTargetName(buildTarget);
            string strWorkingPath = string.Format("{0}{1}/", WorkingPath, strBuildTargetName);

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
