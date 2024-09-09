using UnityEngine;

public class ResultCalculator : MonoBehaviour
{
    public static ResultCalculator instance;

    public Transform MainCubeHolder;

    Transform TargetCubesHolder;

    Transform[] allMainCubes;
    Transform[] allTargtetCubes;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        TargetCubesHolder = GameManager.instance.currentTargetsHolder;
        allTargtetCubes = new Transform[TargetCubesHolder.childCount];
        for(int j =0; j < TargetCubesHolder.childCount; j++)
        {
            allTargtetCubes[j] = TargetCubesHolder.GetChild(j);
        }
    }

    public void GetResults()
    {
        allMainCubes = new Transform[MainCubeHolder.childCount];
        for (int i = 0; i < MainCubeHolder.childCount; i++)
        {
            allMainCubes[i] = MainCubeHolder.GetChild(i);
        }

        if(allMainCubes.Length == allTargtetCubes.Length)
        {
            int r = 0;
            for (int i = 0; i < allTargtetCubes.Length; i++)
            {
                for (int j = 0; j < allMainCubes.Length; j++)
                {
                    float dif = Vector3.Distance(allTargtetCubes[i].position, allMainCubes[j].position);
                    if (dif <= 0.1f)
                    {
                        r++;
                    }
                }
            }
            if (r == allTargtetCubes.Length)
            {
                Debug.Log("Success");
                GameManager.instance.StageFinished();
            }
            else if(GameManager.GameStarted)
            {
                GameManager.instance.GameOver();
            }
        }
        else if(GameManager.GameStarted )
        {
            GameManager.instance.GameOver();
        }
    }
}
