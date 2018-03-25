using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceControl : MonoBehaviour {
    bool isFree = false;
    Vector3 screenPosition;
    Vector3 position;
    Vector3 tempPosition;
    public Camera main;
	// Use this for initialization
	void Start () {
		
	}
    private void OnMouseDown()
    {
        if (!isFree)
        {
            //相对于摄像机的坐标
            screenPosition = main.WorldToScreenPoint(transform.position);

            position = transform.position - main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
            StartCoroutine(ChangePosition());
        }
    }
    IEnumerator ChangePosition()
    {
        while (Input.GetMouseButton(0)) 
        {
            tempPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
            transform.position = tempPosition + position;
            yield return null;
        }
        isFree = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isFree)
        {
            RotateDice();
        }
    }
    void RotateDice()
    {
        Vector3 neweuler = new Vector3(Random.value * 180, Random.value * 180, Random.value * 180);
        transform.rotation = Quaternion.Euler(neweuler);
    }
}
