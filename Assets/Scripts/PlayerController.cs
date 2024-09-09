using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public static Transform PlacementTransform;

    public static Transform currentHollowCube;
    [Range(0.01f,.5f)]
    public float LerpSpeed = .15f;
    public Transform MainCubePrefab;

    public Transform MainCubeHolder;
    public Transform CubeJoiningPSPrefab;

    LayerMask lm;

    bool StartLerping = false;
    bool GameFinished = false;

    [HideInInspector]
    public Vector3 StageEndPosition = Vector3.zero;

    public static Transform StartCube;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        lm = LayerMask.GetMask("TransparentFX");

        StartLerping = false;
        GameFinished = false;
    }

    void Update()
    {
        if (GameManager.GameStarted)
        {
            if (Input.touchSupported)
            {
                if (Input.touchCount > 0)
                {
                    getTouchInput();
                }
            }
            else
            {
                GetMouseInput();
            }
        }
    }

    void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && GameFinished == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, Mathf.Infinity, lm))
            {
                StartLerping = true;
                Destroy(currentHollowCube.gameObject);
                AudioManager.instance.Play("validate");
            }
            else if(Physics.Raycast(ray, Mathf.Infinity, LayerMask.GetMask("Default")))
            {
                return;
            }
            else if (PlacementTransform != null)
            {
                Vector3 pos = PlacementTransform.position;
                pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
                Transform t = Instantiate(MainCubePrefab, pos, Quaternion.identity);
                t.SetParent(MainCubeHolder);
                AudioManager.instance.Play("join" + Random.Range(1, 4));

                Transform p = Instantiate(CubeJoiningPSPrefab, t.position - t.forward * .5f, t.rotation);
                Destroy(currentHollowCube.gameObject);

                LevelIndicator.instance.calculateCurrentProgress();
                if(StartCube != null)
                {
                    Destroy(StartCube.gameObject);
                }
                kyVibrator();
            }
            else
            {
                Debug.LogWarning("No Placement Transform Player");
            }
        }
    }

    void getTouchInput()
    {
        Touch touch = Input.GetTouch(0);
        if(touch.phase == TouchPhase.Began && GameFinished == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, Mathf.Infinity, lm))
            {
                StartLerping = true;
                Destroy(currentHollowCube.gameObject);
                AudioManager.instance.Play("validate");
            }
            else if (Physics.Raycast(ray, Mathf.Infinity, LayerMask.GetMask("Default")))
            {
                return;
            }
            else if (PlacementTransform != null)
            {
                Vector3 pos = PlacementTransform.position;
                pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
                Transform t = Instantiate(MainCubePrefab, pos, PlacementTransform.rotation);
                t.SetParent(MainCubeHolder);
                AudioManager.instance.Play("join" + Random.Range(1, 4));

                Transform p = Instantiate(CubeJoiningPSPrefab, t.position - t.forward * .5f, t.rotation);
                Destroy(currentHollowCube.gameObject);

                LevelIndicator.instance.calculateCurrentProgress();
                if (StartCube != null)
                {
                    Destroy(StartCube.gameObject);
                }

                if (GameManager.instance.Vibration == 1)
                {
                    AndroidJavaObject Vibrator = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getSystemService", "vibrator");
                    Vibrator.Call("vibrate", 1);
                }
            }
            else
            {
                Debug.LogWarning("No Placement Transform Player");
            }
        }
    }

    private void FixedUpdate()
    {
        if(StartLerping && GameFinished == false)
        {
            MainCubeHolder.position = Vector3.Lerp(MainCubeHolder.position, StageEndPosition, LerpSpeed);
            if(Vector3.Distance(MainCubeHolder.position, StageEndPosition) <= .1f)
            {
                StartLerping = false; 
                GameFinished = true;
                MainCubeHolder.position = StageEndPosition;
                ResultCalculator.instance.GetResults();
            }
        }
    }

    static void kyVibrator()
    {
        if(Application.isEditor)
        {
            Handheld.Vibrate();
        }
    }
}
