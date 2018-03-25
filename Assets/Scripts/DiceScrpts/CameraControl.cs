using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public GameObject aim;
    public float smooth;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPositon = aim.transform.position;
        newPositon.z -= 10;
        newPositon.y += 10;
        transform.position = Vector3.Lerp(transform.position, newPositon, Time.deltaTime * smooth);
	}
}
