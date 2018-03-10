using UnityEngine;

public class TiltWindow : MonoBehaviour
{
	public Vector2 range = new Vector2(5f, 3f);

	Transform mTrans;
	Quaternion mStart;
	Vector2 mRot = Vector2.zero;
    float x = -1f;
    float y = 1f;
    int flag = 1;
	void Start ()
	{
		mTrans = transform;
		mStart = mTrans.localRotation;
	}

	void Update ()
	{
		Vector3 pos = Input.mousePosition;

		float halfWidth = Screen.width * 0.5f;
		float halfHeight = Screen.height * 0.5f;
        x += flag * 0.5f * Time.deltaTime;
        y -= flag * 0.5f * Time.deltaTime;
        if (x >= 1 || x <= -1)
        {
            flag *= -1;
        }
		mRot = Vector2.Lerp(mRot, new Vector2(x, y), Time.deltaTime * 5f);
		mTrans.localRotation = mStart * Quaternion.Euler(-mRot.y * range.y, mRot.x * range.x, 0f);
	}
}
