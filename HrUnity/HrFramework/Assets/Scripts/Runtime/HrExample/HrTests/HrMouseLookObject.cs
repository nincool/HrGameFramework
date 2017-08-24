using Hr.CameraControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HrMouseLookObject : MonoBehaviour
{
    public HrMouseLook m_mouseLook;

    public GameObject m_aimObject;
	// Use this for initialization
	void Start ()
    {
        m_mouseLook = new HrMouseLook();
        m_mouseLook.Init(Camera.main.transform, m_aimObject.transform);
        this.gameObject.transform.position = Camera.main.transform.position;
        this.gameObject.transform.rotation = Camera.main.transform.rotation;
	}
	
    private void FixedUpdate()
    {
        m_mouseLook.UpdateLookRotation();
        m_mouseLook.UpdateMove();
    }
}
