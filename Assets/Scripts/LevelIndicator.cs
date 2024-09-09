using UnityEngine;
using UnityEngine.UI;

public class LevelIndicator : MonoBehaviour
{
    public static LevelIndicator instance;

    public Transform MainCubeHolder;
    Transform[] targetPoints;
    Transform[] allMainCubes;

    int noofTargets;

    float ProgressionValue = 0;
    float temp = 0;

    //FOR UI ELEMENTS
    public Image LevelFillerImg;
    [Range(0.01f, .5f)]
    public float FillSpeed = .2f;
    public Text currentLvlText;
    public Text NextLvlText;
    public Text LvlProgressionText;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Transform t = GameManager.instance.currentTargetsHolder;
        targetPoints = new Transform[t.childCount - 1];
        for(int i = 0; i < t.childCount - 1; i++)
        {
            targetPoints[i] = t.GetChild(i+1);
        }

        noofTargets = targetPoints.Length;
        LevelFillerImg.fillAmount = ProgressionValue;

        int s = PlayerPrefs.GetInt("lvl");
        currentLvlText.text = s.ToString();
        NextLvlText.text = (s + 1).ToString();
        LvlProgressionText.text = s + " >> " + (s + 1);
    }

    public void calculateCurrentProgress()
    {
        allMainCubes = new Transform[MainCubeHolder.childCount -1];
        for (int i = 0; i < MainCubeHolder.childCount - 1; i++)
        {
            allMainCubes[i] = MainCubeHolder.GetChild(i+1);
        }

        int r = 0;
        for (int i = 0; i < allMainCubes.Length; i++)
        {
            for (int j = 0; j < targetPoints.Length; j++)
            {
                float dif = Vector3.Distance(targetPoints[j].position, allMainCubes[i].position);
                if (Mathf.Abs(dif) == 3)
                {
                    if (targetPoints[j].position.x == allMainCubes[i].position.x && targetPoints[j].position.z == allMainCubes[i].position.z)
                    {
                        r++;
                    }
                }
            }
        }

        ProgressionValue = (float)r / noofTargets;
        //LevelFillerImg.fillAmount = ProgressionValue;
    }

    private void Update()
    {
        if(temp != ProgressionValue)
        {
            temp = Mathf.Lerp(temp, ProgressionValue, FillSpeed);
            LevelFillerImg.fillAmount = temp;
        }
    }
}
