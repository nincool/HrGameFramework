using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
