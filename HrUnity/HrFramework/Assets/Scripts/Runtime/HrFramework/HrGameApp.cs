using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hr.CommonUtility;

public class HrGameApp : HrUnitySingleton<HrGameApp>
{

	public void Start ()
	{
		DontDestroyOnLoad(this);
	}
}

