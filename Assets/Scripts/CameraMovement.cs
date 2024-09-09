using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    public Vector3 CenterPos = Vector3.zero;

    public float RotSpeed = 5f;
    public float HeightTravel = .5f;
    public float StepSpeed = .3f;

    float initHeight;
    float temp = 0;
    bool up = true;

    Vector3 curpos;

    Transform currentLvl;
    Transform[] childs;
    Vector3 pos = Vector3.zero;

    //For CameraRotation from Finger
    [Header("Input")]
    public float Sensitivity = 5f;
    public float RotMultiplier = 10f;
    float tempMuliplier = 1;
    float StartPos, CurPos;
    bool haveInput = false;
    Ray ray;
    LayerMask lm;

    //For Party Papers
    Transform PartyPapersPrefab;

    void Start()
    {
        initHeight = transform.position.y;

        currentLvl = GameManager.instance.currentLvlHodler;

        childs = new Transform[currentLvl.childCount];

        for(int i = 0; i < currentLvl.childCount; i++)
        {
            childs[i] = currentLvl.GetChild(i);
            pos.x += childs[i].position.x;
            pos.y += childs[i].position.y;
            pos.z += childs[i].position.z;
        }
        pos = pos / currentLvl.childCount;
        pos.y = 0;

        CenterPos = pos;

        haveInput = false;
        tempMuliplier = 1;
        lm = LayerMask.GetMask("Default");

        PartyPapersPrefab = Resources.Load<Transform>("PartyPaperPS_Holder");
    }

    void Update()
    {
        if(Input.touchSupported)
        {
            GetTouchInput();
        }
        else
        {
            GetMouseInput();

        }

        if (up)
        {
            if (temp < HeightTravel)
            {
                temp += Time.deltaTime * StepSpeed;
            }
            else
            {
                up = false;
            }
        }
        else
        {
            if(temp > -HeightTravel)
            {
                temp -= Time.deltaTime * StepSpeed;
            }
            else
            {
                up = true;
            }
        }
        curpos = new Vector3(transform.position.x, initHeight + temp, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, curpos, .3f);

        transform.RotateAround(pos, Vector3.up, RotSpeed * tempMuliplier * Time.deltaTime);
        transform.LookAt(CenterPos + Vector3.up * temp);
        //transform.LookAt(pos + Vector3.up * temp);
    }

    void GetMouseInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, Mathf.Infinity, lm))
            {
                StartPos = Input.mousePosition.x;
                haveInput = true;
            }
        }
        else if(Input.GetMouseButton(0) && haveInput == true)
        {
            CurPos = Input.mousePosition.x;
            float dis = CurPos - StartPos;
            dis = dis / Screen.width;
            dis = dis *  Sensitivity;
            tempMuliplier = dis;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            haveInput = false;
        }
    }

    void GetTouchInput()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, Mathf.Infinity, lm))
                {
                    StartPos = touch.position.x;
                    haveInput = true;
                }
            }
            else if(touch.phase == TouchPhase.Moved && haveInput == true || touch.phase == TouchPhase.Stationary && haveInput == true)
            {
                CurPos = touch.position.x;
                float dis = CurPos - StartPos;
                dis = dis / Screen.width;
                dis = dis * Sensitivity;
                Debug.Log("DIS = " + dis);
                tempMuliplier = dis;
            }
            else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                haveInput = false;
            }
        }
    }

    public void SpawnPartyPapers()
    {
        if (PartyPapersPrefab != null)
        {
            Instantiate(PartyPapersPrefab, CenterPos, PartyPapersPrefab.rotation);
        }
        else
        {
            Debug.LogWarning("No Party Papers Prefab");
        }
    }
}
