using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceControl : MonoBehaviour {
    bool isFree = false;
    Vector3 screenPosition;
    Vector3 position;
    Vector3 tempPosition;
    Vector3 beginPosition;
    Rigidbody m_rigidbody;
    public Camera main;
	// Use this for initialization
	void Start () {
        m_rigidbody = GetComponent<Rigidbody>();
	}
    private void OnMouseDown()
    {
        if (!isFree)
        {
            beginPosition = transform.position;
            //相对于摄像机的坐标
            screenPosition = main.WorldToScreenPoint(transform.position);
            //实际物体位置于手第一次点上摄像机位置之差
            position = transform.position - main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
            StartCoroutine(ChangePosition());
        }
    }
    IEnumerator ChangePosition()
    {
        while (Input.GetMouseButton(0)) 
        {
            tempPosition = main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
            transform.position = tempPosition + position;
            yield return null;
        }
        m_rigidbody.constraints = RigidbodyConstraints.None;
        isFree = true;
        Vector3 force = new Vector3(transform.position.x - beginPosition.x, 0, 0);
        float dy = transform.position.y - beginPosition.y;
        force.y = 1.414f * dy;
        force.z = 1.414f * dy;
        m_rigidbody.AddForce(250 * force);
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
