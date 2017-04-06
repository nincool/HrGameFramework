using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hr.CommonUtility
{
    public class HrAssetBundleUtility
    {
        public const string STR_ASSETBUNDLES_OUTPUT_PATH = "Assets/AssetBundles";

        public static readonly string mStrProjectplatform;

        public static readonly string mStrSteamAssetsPath = Application.streamingAssetsPath;

        public static readonly string mStrStreamingAssetBundlePath;
        //数据持久化目录
        public static readonly string mStrPersistentPath = Application.persistentDataPath;

        public static readonly string mStrPersistentAssetBundlePath;

        static HrAssetBundleUtility()
        {
            string strPlatformName = HrAssetBundleUtility.GetPlatformName();

            mStrStreamingAssetBundlePath = mStrSteamAssetsPath + "/" + strPlatformName + "/";
            mStrPersistentAssetBundlePath = mStrPersistentPath + "/" + "AssetBundles/";

        }

        public static string GetPlatformName()
        {
#if UNITY_EDITOR
            return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
            return GetPlatformForAssetBundles(Application.platform);
#endif
        }

#if UNITY_EDITOR
        private static string GetPlatformForAssetBundles(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return "Android";
#if UNITY_TVOS
                case BuildTarget.tvOS:
                    return "tvOS";
#endif
                case BuildTarget.iOS:
                    return "iOS";
                case BuildTarget.WebGL:
                    return "WebGL";

#if (UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
                case BuildTarget.WebPlayer:
                    return "WebPlayer";
#elif (UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4)
                case BuildTarget.WebPlayer:
                    return "WebPlayer";
#endif
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "Windows";
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSXUniversal:
                    return "OSX";
                // Add more build targets for your own.
                // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
                default:
                    return null;
            }
        }
#endif

        private static string GetPlatformForAssetBundles(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
#if UNITY_TVOS
                case RuntimePlatform.tvOS:
                    return "tvOS";
#endif
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
#if (UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
                case RuntimePlatform.OSXWebPlayer:
                case RuntimePlatform.WindowsWebPlayer:
                    return "WebPlayer";
#elif (UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4)
                case RuntimePlatform.OSXWebPlayer:
                case RuntimePlatform.WindowsWebPlayer:
                    return "WebPlayer";
#endif
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                // Add more build targets for your own.
                // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
                default:
                    return null;
            }
        }

        public static string GetAssetBundleManifestPath()
        {
            return HrAssetBundleUtility.mStrStreamingAssetBundlePath + GetPlatformName();
        }
    }
}
