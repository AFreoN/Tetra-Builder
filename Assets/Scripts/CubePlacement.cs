using UnityEngine;
using System.Collections;

public class CubePlacement : MonoBehaviour
{
    [Range(.01f,.5f)]
    public float LerpSpeed = .05f;

    public float StepTime = .2f;
    float temp = 0;

    public Transform Parent;
    public Transform[] transPoints;

    int curIndex = 0;

    Transform curTransform;
    bool Started = false;

    Coroutine lastCoroutine;

    void Start()
    {
        PlayerController.currentHollowCube = transform;
        GetNextTranform();

        Started = true;
    }

    void Update()
    {
        if (Started)
        {
            if (temp < StepTime)
            {
                temp += Time.deltaTime;
            }
            else
            {
                GetNextTranform();
            }
        }
    }

    void GetNextTranform()
    {
        if(lastCoroutine != null)
        {
            StopCoroutine(lastCoroutine);
        }

        if(curIndex < Parent.childCount)
        {
            curTransform = transPoints[curIndex];
            curIndex++;
        }
        else
        {
            curIndex = 0;
            curTransform = transPoints[curIndex];
            curIndex++;
        }

        //PlayerController.PlacementTransform = curTransform;
        lastCoroutine = StartCoroutine(assignPlayerPlacementTransform());
        temp = 0;
    }

    IEnumerator assignPlayerPlacementTransform()
    {
        yield return new WaitForSeconds(StepTime / 4);
        PlayerController.PlacementTransform = curTransform;
    }

    private void FixedUpdate()
    {
        if (Started)
        {
            if (curTransform != null)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, curTransform.rotation, LerpSpeed);
                transform.position = Vector3.Lerp(transform.position, curTransform.position, LerpSpeed);
            }
            else
            {
                GetNextTranform();
            }
        }
    }
}
