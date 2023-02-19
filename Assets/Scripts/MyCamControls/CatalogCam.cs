using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalogCam : MonoBehaviour {

    float birthTime;
    Camera _camera;
    float rotSpeed = .5f;

	// Use this for initialization
	void Awake () {
        birthTime = Time.time;

        transform.position = new Vector3(0, 1.5f, -10);
        transform.LookAt(Vector3.zero);

        _camera = GetComponent<Camera>();
        _camera.cullingMask = 1 << 8;
    }

    private void OnEnable()
    {
        // Reset camera position when re-enabled
        birthTime = Time.time;
    }

    // Update is called once per frame
    void Update () {
        //this.transform.localPosition = Vector3.zero;
        float passedTime = Time.time - birthTime;

        transform.localPosition = new Vector3(-10 * Mathf.Sin(passedTime * rotSpeed), 1.5f, -10 * Mathf.Cos(passedTime * rotSpeed));
        transform.LookAt(this.transform.parent);
    }
}
