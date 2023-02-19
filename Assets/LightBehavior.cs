using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBehavior : MonoBehaviour {

    private Vector3 m_curRot;
    public float rotSpeed;

	// Use this for initialization
	void Start () {
        m_curRot = this.transform.rotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
        m_curRot.y += rotSpeed * Time.deltaTime;
        this.transform.rotation = Quaternion.Euler(m_curRot);
	}
}
