using UnityEngine;

public class GroundCubes : MonoBehaviour
{
    [HideInInspector]
    public Vector3 StartPos;
    [HideInInspector]
    public Quaternion startAngle;

    public float LerpSpeed = .08f;

    MainCube mc;

    private void Awake()
    {
        StartPos = transform.position;
        startAngle = transform.rotation;
        mc = GetComponent<MainCube>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position != StartPos && transform.rotation != startAngle)
        {
            transform.position = Vector3.Lerp(transform.position, StartPos, LerpSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, startAngle, LerpSpeed);
        }
        else if(gameObject.GetComponent<GroundCubes>() != null)
        {
            transform.position = StartPos;
            transform.rotation = startAngle;
            Destroy(gameObject.GetComponent<GroundCubes>());
        }
    }
}
