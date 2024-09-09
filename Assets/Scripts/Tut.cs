using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tut : MonoBehaviour
{
    public GameObject circleImg;
    public GameObject FingerImg;
    public Transform MainCubeHolder;

    public GameObject tapToPlaceText, tapBlockText;
    public GameObject WrongPlacementTextPrefab;
    Vector3 wpos, wscale;

    Transform[] allCubes;

    Vector3 screenPos;
    int startCount;

    bool CubePlacedProperly = false;

    void Start()
    {
        getCubes();
        startCount = MainCubeHolder.childCount;

        wpos = WrongPlacementTextPrefab.transform.localPosition;
        wscale = WrongPlacementTextPrefab.transform.localScale;

        circleImg.SetActive(false);
        FingerImg.SetActive(false);
        tapToPlaceText.SetActive(true);
        tapBlockText.SetActive(false);
    }

    void Update()
    {
        if (CubePlacedProperly)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        Destroy(gameObject);
                    }
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Destroy(gameObject);
            }
        }

        if (CubePlacedProperly)
        {
            screenPos = Camera.main.WorldToScreenPoint(allCubes[0].position + allCubes[0].forward * .5f);
            circleImg.GetComponent<RectTransform>().position = screenPos;
            FingerImg.GetComponent<RectTransform>().position = screenPos;
        }

        if (GameManager.GameStarted)
        {
            if (MainCubeHolder.childCount > 2)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else if (MainCubeHolder.childCount != startCount)
            {
                getCubes();
                if (allCubes[1].localPosition != new Vector3(0, 0, 1))
                {
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    Destroy(allCubes[1].gameObject);
                    allCubes[0].GetComponent<MainCube>().SpawnHollow();
                    GameObject g = Instantiate(WrongPlacementTextPrefab, WrongPlacementTextPrefab.transform.position, WrongPlacementTextPrefab.transform.rotation);
                    AudioManager.instance.Play("wrongtap");
                    g.transform.SetParent(transform);
                    g.transform.localPosition = wpos;
                    g.transform.localScale = wscale;
                    Destroy(g, 2f);
                }
                else
                {
                    CubePlacedProperly = true;
                    tapToPlaceText.SetActive(false);
                    tapBlockText.SetActive(true);

                    circleImg.SetActive(true);
                    FingerImg.SetActive(true);
                    startCount = MainCubeHolder.childCount;
                }
            }
        }
        Debug.Log("Start Count = " + startCount + " MainCubeHolder Child Count = " + MainCubeHolder.childCount);
    }

    void getCubes()
    {
        allCubes = new Transform[MainCubeHolder.childCount];
        for (int i = 0; i < MainCubeHolder.childCount; i++)
        {
            allCubes[i] = MainCubeHolder.GetChild(i);
        }
    }
}
