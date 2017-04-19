using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hr;


public class HrGameApp : HrUnitySingleton<HrGameApp>
{

    private void Awake()
    {
        InitGame();
    }

	public void Start ()
	{
        HrLogger.Log("HrGameApp Start!");

        DontDestroyOnLoad(this);

        Application.runInBackground = true;
	}

    private void InitGame()
    {
        HrGameWorld.Instance.Init();
        
        HrGameWorld.Instance.ChangeScene(EnumSceneType.SCENE_LAUNCH);
    }


    
}

