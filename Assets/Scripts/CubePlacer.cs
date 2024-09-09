using UnityEngine;

public class CubePlacer : MonoBehaviour
{
    public Transform Parent;
    public float rotSpeed = 200f;

    bool destinationReached = false;

    Vector3 curRotPoint;

    public float finalAngle;

    Transform[] RotPoints;
    int curIndex = 0;

    void Start()
    {
        RotPoints = new Transform[Parent.childCount];
        for(int i = 0; i < Parent.childCount; i++)
        {
            RotPoints[i] = Parent.GetChild(i);
        }
        finalAngle = transform.localEulerAngles.y + 180;
        finalAngle = Mathf.Round(finalAngle);
        getRotPoint2();
    }

    private void Update()
    {

    }

    void getRotPoint2()
    {
        if(curIndex < RotPoints.Length)
        {
            curRotPoint = RotPoints[curIndex].position;
            curIndex++;
        }
        else
        {
            curIndex = 0;
            curRotPoint = RotPoints[curIndex].position;

            Debug.Log(RotPoints[curIndex].name);
        }
    }

    void FixedUpdate()
    {
        if (destinationReached == false)
        {
            transform.RotateAround(curRotPoint, Vector3.up, rotSpeed * Time.fixedDeltaTime);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Parent.rotation, .05f);
            //transform.position = Vector3.Lerp(transform.position, Parent.position, .05f);
        }
        if (destinationReached == false)
        {
            if (Mathf.Round(transform.localEulerAngles.y) == finalAngle)
            {
                destinationReached = true;
                getRotPoint2();
                finalAngle = transform.localEulerAngles.y + 180;
                finalAngle = Mathf.Round(finalAngle);
                if(finalAngle == 360)
                {
                    finalAngle = 0;
                }
                destinationReached = false;
            }
        }
    }
}
