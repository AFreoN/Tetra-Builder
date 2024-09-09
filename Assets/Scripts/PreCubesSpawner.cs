using UnityEngine;

public class PreCubesSpawner : MonoBehaviour
{
    Transform MainCubeHolder;
    public Transform PreCubePrefab;

    public Transform StartCube;

    PlayerController pc;

    public int EndPos = 0;

    Transform[] allPreCubes;

    public Vector3 StartcubePos = Vector3.zero;

    void Start()
    {
        MainCubeHolder = GameManager.instance.MainCubeHolder;

        if (PreCubePrefab != null)
        {
            Transform t = Instantiate(PreCubePrefab, MainCubeHolder.position, PreCubePrefab.rotation);
            allPreCubes = new Transform[t.childCount];
            for (int i = 0; i < t.childCount; i++)
            {
                allPreCubes[i] = t.GetChild(i);
                //allPreCubes[i].SetParent(MainCubeHolder);
            }

            for (int j = 0; j < allPreCubes.Length; j++)
            {
                allPreCubes[j].SetParent(MainCubeHolder);
            }
            Destroy(t.gameObject);
        }

        pc = PlayerController.instance;
        pc.StageEndPosition = new Vector3(0, EndPos, 0);

        MainCubeHolder.GetChild(0).GetComponent<MainCube>().StartSetter();

        PlayerController.StartCube = Instantiate(StartCube, StartcubePos, Quaternion.identity);
    }
}
