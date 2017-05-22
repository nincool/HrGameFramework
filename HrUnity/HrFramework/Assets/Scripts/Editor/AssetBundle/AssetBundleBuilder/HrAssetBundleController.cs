using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Hr.Editor
{
    public class HrAssetBundleController
    {

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

        //public bool BuildAssetBundles()
        //{
        //    if (!IsValidOutputDirectory)
        //    {
        //        return false;
        //    }

        //    if (Directory.Exists(OutputPackagePath))
        //    {
        //        Directory.Delete(OutputPackagePath, true);
        //    }

        //    Directory.CreateDirectory(OutputPackagePath);

        //    if (Directory.Exists(OutputFullPath))
        //    {
        //        Directory.Delete(OutputFullPath, true);
        //    }

        //    Directory.CreateDirectory(OutputFullPath);

        //    if (Directory.Exists(OutputPackedPath))
        //    {
        //        Directory.Delete(OutputPackedPath, true);
        //    }

        //    Directory.CreateDirectory(OutputPackedPath);

        //    if (Directory.Exists(BuildReportPath))
        //    {
        //        Directory.Delete(BuildReportPath, true);
        //    }

        //    Directory.CreateDirectory(BuildReportPath);

        //    BuildAssetBundleOptions buildAssetBundleOptions = GetBuildAssetBundleOptions();

 

        //}

        //private BuildAssetBundleOptions GetBuildAssetBundleOptions()
        //{
        //    BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.None;

        //    if (UncompressedAssetBundleSelected)
        //    {
        //        buildOptions |= BuildAssetBundleOptions.UncompressedAssetBundle;
        //    }

        //    if (DisableWriteTypeTreeSelected)
        //    {
        //        buildOptions |= BuildAssetBundleOptions.DisableWriteTypeTree;
        //    }

        //    if (DeterministicAssetBundleSelected)
        //    {
        //        buildOptions |= BuildAssetBundleOptions.DeterministicAssetBundle;
        //    }

        //    if (ForceRebuildAssetBundleSelected)
        //    {
        //        buildOptions |= BuildAssetBundleOptions.ForceRebuildAssetBundle;
        //    }

        //    if (IgnoreTypeTreeChangesSelected)
        //    {
        //        buildOptions |= BuildAssetBundleOptions.IgnoreTypeTreeChanges;
        //    }

        //    if (AppendHashToAssetBundleNameSelected)
        //    {
        //        buildOptions |= BuildAssetBundleOptions.AppendHashToAssetBundleName;
        //    }

        //    if (ChunkBasedCompressionSelected)
        //    {
        //        buildOptions |= BuildAssetBundleOptions.ChunkBasedCompression;
        //    }

        //    return buildOptions;
        //}

        //private void BuildAssetBundles(AssetBundleBuild[] buildMap, BuildAssetBundleOptions buildOptions, bool zip, BuildTarget buildTarget)
        //{
        //    m_BuildReport.LogInfo("Start build AssetBundles for '{0}'...", buildTarget.ToString());

        //    string buildTargetUrlName = GetBuildTargetName(buildTarget);

        //    string workingPath = string.Format("{0}{1}/", WorkingPath, buildTargetUrlName);
        //    m_BuildReport.LogInfo("Working path is '{0}'.", workingPath);

        //    string outputPackagePath = string.Format("{0}{1}/", OutputPackagePath, buildTargetUrlName);
        //    Directory.CreateDirectory(outputPackagePath);
        //    m_BuildReport.LogInfo("Output package path is '{0}'.", outputPackagePath);

        //    string outputFullPath = string.Format("{0}{1}/", OutputFullPath, buildTargetUrlName);
        //    Directory.CreateDirectory(outputFullPath);
        //    m_BuildReport.LogInfo("Output full path is '{0}'.", outputFullPath);

        //    string outputPackedPath = string.Format("{0}{1}/", OutputPackedPath, buildTargetUrlName);
        //    Directory.CreateDirectory(outputPackedPath);
        //    m_BuildReport.LogInfo("Output packed path is '{0}'.", outputPackedPath);

        //    // Clean working path
        //    List<string> validNames = new List<string>();
        //    foreach (AssetBundleBuild i in buildMap)
        //    {
        //        string assetBundleName = GetAssetBundleFullName(i.assetBundleName, i.assetBundleVariant);
        //        validNames.Add(assetBundleName);
        //    }

        //    if (Directory.Exists(workingPath))
        //    {
        //        Uri workingUri = new Uri(workingPath, UriKind.RelativeOrAbsolute);
        //        string[] fileNames = Directory.GetFiles(workingPath, "*", SearchOption.AllDirectories);
        //        foreach (string fileName in fileNames)
        //        {
        //            if (fileName.EndsWith(".manifest"))
        //            {
        //                continue;
        //            }

        //            string relativeName = workingUri.MakeRelativeUri(new Uri(fileName)).ToString();
        //            if (!validNames.Contains(relativeName))
        //            {
        //                File.Delete(fileName);
        //            }
        //        }

        //        string[] manifestNames = Directory.GetFiles(workingPath, "*.manifest", SearchOption.AllDirectories);
        //        foreach (string manifestName in manifestNames)
        //        {
        //            if (!File.Exists(Path.GetFileNameWithoutExtension(manifestName)))
        //            {
        //                File.Delete(manifestName);
        //            }
        //        }

        //        Utility.Path.RemoveEmptyDirectory(workingPath);
        //    }

        //    if (!Directory.Exists(workingPath))
        //    {
        //        Directory.CreateDirectory(workingPath);
        //    }

        //    // Build AssetBundles
        //    m_BuildReport.LogInfo("Unity start build AssetBundles for '{0}'...", buildTarget.ToString());
        //    AssetBundleManifest assetBundleManifest = BuildPipeline.BuildAssetBundles(workingPath, buildMap, buildOptions, buildTarget);
        //    if (assetBundleManifest == null)
        //    {
        //        m_BuildReport.LogError("Build AssetBundles for '{0}' failure.", buildTarget.ToString());
        //        return;
        //    }

        //    m_BuildReport.LogInfo("Unity build AssetBundles for '{0}' complete.", buildTarget.ToString());

        //    // Process AssetBundles
        //    for (int i = 0; i < buildMap.Length; i++)
        //    {
        //        string assetBundleFullName = GetAssetBundleFullName(buildMap[i].assetBundleName, buildMap[i].assetBundleVariant);
        //        if (ProcessingAssetBundle != null)
        //        {
        //            if (ProcessingAssetBundle(assetBundleFullName, (float)(i + 1) / buildMap.Length))
        //            {
        //                m_BuildReport.LogWarning("The build has been canceled by user.");
        //                return;
        //            }
        //        }

        //        m_BuildReport.LogInfo("Start process '{0}' for '{1}'...", assetBundleFullName, buildTarget.ToString());

        //        ProcessAssetBundle(workingPath, outputPackagePath, outputFullPath, outputPackedPath, zip, buildTarget, buildMap[i].assetBundleName, buildMap[i].assetBundleVariant);

        //        m_BuildReport.LogInfo("Process '{0}' for '{1}' complete.", assetBundleFullName, buildTarget.ToString());
        //    }

        //    ProcessPackageList(outputPackagePath, buildTarget);
        //    m_BuildReport.LogInfo("Process package list for '{0}' complete.", buildTarget.ToString());

        //    VersionListData versionListData = ProcessVersionList(outputFullPath, buildTarget);
        //    m_BuildReport.LogInfo("Process version list for '{0}' complete.", buildTarget.ToString());

        //    ProcessReadOnlyList(outputPackedPath, buildTarget);
        //    m_BuildReport.LogInfo("Process readonly list for '{0}' complete.", buildTarget.ToString());

        //    m_VersionListDatas.Add(buildTarget, versionListData);

        //    if (ProcessAssetBundleComplete != null)
        //    {
        //        ProcessAssetBundleComplete(buildTarget, versionListData.Path, versionListData.Length, versionListData.HashCode, versionListData.ZipLength, versionListData.ZipHashCode);
        //    }

        //    m_BuildReport.LogInfo("Build AssetBundles for '{0}' success.", buildTarget.ToString());
        //}

        //private AssetBundleBuild[] GetBuildMap()
        //{
        //    m_AssetBundleDatas.Clear();

        //    AssetBundle[] assetBundles = m_AssetBundleCollection.GetAssetBundles();
        //    foreach (AssetBundle assetBundle in assetBundles)
        //    {
        //        m_AssetBundleDatas.Add(assetBundle.FullName.ToLower(), new AssetBundleData(assetBundle.Name.ToLower(), (assetBundle.Variant != null ? assetBundle.Variant.ToLower() : null), assetBundle.LoadType, assetBundle.Packed));
        //    }

        //    Asset[] assets = m_AssetBundleCollection.GetAssets();
        //    foreach (Asset asset in assets)
        //    {
        //        string assetName = asset.Name;
        //        if (string.IsNullOrEmpty(assetName))
        //        {
        //            m_BuildReport.LogError("Can not find asset by guid '{0}'.", asset.Guid);
        //            return null;
        //        }

        //        string assetFileFullName = Utility.Path.GetCombinePath(Application.dataPath, assetName.Substring(AssetsSubstringLength));
        //        if (!File.Exists(assetFileFullName))
        //        {
        //            m_BuildReport.LogError("Can not find asset '{0}'.", assetFileFullName);
        //            return null;
        //        }

        //        byte[] assetBytes = File.ReadAllBytes(assetFileFullName);
        //        int assetHashCode = Utility.Converter.GetIntFromBytes(Utility.Verifier.GetCrc32(assetBytes));

        //        List<string> dependencyAssetNames = new List<string>();
        //        DependencyData dependencyData = m_AssetBundleAnalyzerController.GetDependencyData(assetName);
        //        Asset[] dependencyAssets = dependencyData.GetDependencyAssets();
        //        foreach (Asset dependencyAsset in dependencyAssets)
        //        {
        //            dependencyAssetNames.Add(dependencyAsset.Name);
        //        }

        //        if (RecordScatteredDependencyAssetsSelected)
        //        {
        //            dependencyAssetNames.AddRange(dependencyData.GetScatteredDependencyAssetNames());
        //        }

        //        dependencyAssetNames.Sort();

        //        m_AssetBundleDatas[asset.AssetBundle.FullName.ToLower()].AddAssetData(asset.Guid, assetName, assetBytes.Length, assetHashCode, dependencyAssetNames.ToArray());
        //    }

        //    foreach (AssetBundleData assetBundleData in m_AssetBundleDatas.Values)
        //    {
        //        if (assetBundleData.AssetCount <= 0)
        //        {
        //            m_BuildReport.LogError("AssetBundle '{0}' has no asset.", GetAssetBundleFullName(assetBundleData.Name, assetBundleData.Variant));
        //            return null;
        //        }
        //    }

        //    AssetBundleBuild[] buildMap = new AssetBundleBuild[m_AssetBundleDatas.Count];
        //    int index = 0;
        //    foreach (AssetBundleData assetBundleData in m_AssetBundleDatas.Values)
        //    {
        //        buildMap[index].assetBundleName = assetBundleData.Name;
        //        buildMap[index].assetBundleVariant = assetBundleData.Variant;
        //        buildMap[index].assetNames = assetBundleData.GetAssetNames();
        //        index++;
        //    }

        //    return buildMap;
        //}

    }
}
