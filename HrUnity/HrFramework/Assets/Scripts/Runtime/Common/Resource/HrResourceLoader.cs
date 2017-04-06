using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Resource
{
    public class HrResourceLoader
    {

        public IEnumerator LoadAssetBundleSync(string strAssetBundleName)
        {
            yield break;
            //Debug.Log("LoadAssetBundleSyn");
            //if (HrResourceManager.Instance.AssetBundlePool.ContainsKey(strAssetBundleName))
            //{
            //    //这里应该校验下，如果当前正在异步加载，那么等待异步加载完成
            //    HrAssetBundle tempAssetBundle = HrResourceManager.Instance.AssetBundlePool[strAssetBundleName];
            //    while (tempAssetBundle.IsLoading())
            //    {
            //        yield return null;
            //    }

            //    Debug.LogError("HrResourceManager:LoadAssetBundleSync Error! AssetBundleName:" + strAssetBundleName);
            //    yield return null;
            //}
            //if (!Directory.Exists(strAssetBundleName))
            //{
            //    Debug.LogError("HrResourceManager:LoadAssetBundleSync Error! AssetBunde is not exist!:" + strAssetBundleName);
            //}

            //HrAssetBundle assetBundle = new HrAssetBundle(ref strAssetBundleName, new Action<HrAssetBundle>(ActionAssetBundleLoadFinshed));
            //assetBundle.LoadSync();
        }
    }
}

