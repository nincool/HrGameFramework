using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hr.Resource;

public class HrTestResourceLoad : MonoBehaviour {


    void Awake()
    {
        //HrResourceManager.Instance.LoadAssetBundleSync(HrResourcePath.mStrStreamingAssetBundlePath + "hrassets/hrtest01.normal");
        HrResourceManager.Instance.LoadAssetBundleManifest();
        HrResourceManager.Instance.LoadAssetBundleSync(HrResourcePath.mStrStreamingAssetBundlePath + "hrassets/hrtest01.normal");
    }
    // Use this for initialization
    void Start ()
    {
        var goAsset = HrResourceManager.Instance.GetAsset<GameObject>("assets/hrresource/hrtest01.prefab");
        if (goAsset != null)
           GameObject.Instantiate(goAsset);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
