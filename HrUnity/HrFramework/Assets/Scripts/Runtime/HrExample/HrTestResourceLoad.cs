using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hr;
using UnityEngine.EventSystems;

public class HrTestResourceLoad : MonoBehaviour {

    public GameObject target;

    void Awake()
    {
        //HrResourceManager.Instance.LoadAssetBundleSync(HrResourcePath.ms_strStreamingAssetBundlePath + "hrassets/hrtest01.normal");
    }
    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnBtnClick()
    {
        ExecuteEvents.Execute<ICustomMessageTarget>(this.gameObject, null, (x, y) => x.Message1());
    }
}
