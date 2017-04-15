﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hr.CommonUtility;
using Hr.Resource;

using System.IO;
using UnityEngine.EventSystems;

namespace Hr.Sample
{

    public class HrLaunch : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Debug.Log("Application.dataPath:" + Application.dataPath);
            Debug.Log("Application.persistentDataPath:" + Application.persistentDataPath);
            Debug.Log("Application.streamingAssetsPath" + Application.streamingAssetsPath);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClickStart()
        {
            //StartCoroutine(CopyZipAssetToPersistentPath());
            ExecuteEvents.Execute<ICustomMessageTarget>(this.gameObject, null, (x, y) => x.Message1());
        }

        private IEnumerator CopyZipAssetToPersistentPath()
        {
            string strAssetSrcPath = HrResourcePath.ms_strStreamingAssetPathForWWW + HrResourcePath.STR_ZIP_ASSETFILE;
            string strAssetOutPath = HrResourcePath.ms_strZipAssetBundleUnPackPath;

            using (WWW www = new WWW(strAssetSrcPath))
            {
                yield return www;

                if (!string.IsNullOrEmpty(www.error))
                {
                    HrLoger.LogError("HrLaunch error! CopyZipAssetToPersistentPath:" + www.error);

                    Application.Quit(); 
                }
                else
                {
                    HrUnpackZipFileThread unpackZip = new HrUnpackZipFileThread(www.bytes, strAssetOutPath, (a, b) => { Debug.Log("UnPack:" + a + " " + b); });
                    unpackZip.Start();
                    while (!unpackZip.IsDone)
                    {
                        yield return 0;
                    }
                }
            }
            HrResourceManager.Instance.LoadAssetBundleManifest();
            HrResourceManager.Instance.LoadAssetBundleSync("hrasset/test01.normal");
            var goAsset = HrResourceManager.Instance.GetAsset<GameObject>("assets/hrresource/uncompressedasset/prefab/hrtest01.prefab");
            if (goAsset != null)
                GameObject.Instantiate(goAsset);
        }
    }
}
