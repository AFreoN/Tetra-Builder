using UnityEngine;

public class GroundCubesAssembler : MonoBehaviour
{
    Transform GroundCubeHolder;
    Transform[] allCubes;
    Vector3[] allPos;
    Vector3[] startPos;

    public float IntensityMin = 10f;
    public float IntensityMax = 20f;
    public Vector3 randomRot = new Vector3(180, 180, 180);

    private void Awake()
    {
        GroundCubeHolder = transform;
    }

    private void Start()
    {
        allCubes = new Transform[GroundCubeHolder.childCount];
        allPos = new Vector3[GroundCubeHolder.childCount];
        for(int i = 0; i < GroundCubeHolder.childCount; i ++)
        {
            allCubes[i] = GroundCubeHolder.GetChild(i);
            allPos[i] = GroundCubeHolder.GetChild(i).position;
        }

        startPos = new Vector3[GroundCubeHolder.childCount];
        for(int i = 0; i < GroundCubeHolder.childCount; i++)
        {
            float r = Random.Range(IntensityMin, IntensityMax + 1);
            startPos[i] = allPos[i] + Random.insideUnitSphere * r;
        }

        for(int i = 0; i < allCubes.Length; i++)
        {
            allCubes[i].position = startPos[i];     //Setting Start Position

            int x = (int)Random.Range(-randomRot.x, randomRot.x);
            int y = (int)Random.Range(-randomRot.y, randomRot.y);
            int z = (int)Random.Range(-randomRot.z, randomRot.z);
            allCubes[i].localEulerAngles = new Vector3(x, y, z);        //Setting Start Rotation
        }
    }

    public void ReturnCubes()
    {
        for (int i = 0; i < GroundCubeHolder.childCount; i++)
        {
            float r = Random.Range(IntensityMin * 1.5f, (IntensityMax * 1.5f) + 1);
            startPos[i] = allPos[i] + Random.insideUnitSphere * r;
        }

        for (int i = 0; i < allCubes.Length; i++)
        {
            GroundCubes gc = allCubes[i].gameObject.AddComponent<GroundCubes>();
            gc.StartPos = startPos[i];

            int x = (int)Random.Range(-randomRot.x, randomRot.x);
            int y = (int)Random.Range(-randomRot.y, randomRot.y);
            int z = (int)Random.Range(-randomRot.z, randomRot.z);
            gc.startAngle = Quaternion.Euler(new Vector3(x, y, z));
        }
    }

    public void ReturnMainCubes()
    {
        startPos = new Vector3[GroundCubeHolder.childCount];
        allCubes = new Transform[GroundCubeHolder.childCount];
        for(int x = 0; x < GroundCubeHolder.childCount; x++)
        {
            allCubes[x] = GroundCubeHolder.GetChild(x);
        }
        for (int i = 0; i < GroundCubeHolder.childCount; i++)
        {
            float r = Random.Range(IntensityMin * 1.5f, (IntensityMax * 1.5f) + 1);
            startPos[i] = allCubes[i].position + Random.insideUnitSphere * r;
        }

        for (int i = 0; i < allCubes.Length; i++)
        {
            GroundCubes gc = allCubes[i].gameObject.AddComponent<GroundCubes>();
            gc.StartPos = startPos[i];

            int x = (int)Random.Range(-randomRot.x, randomRot.x);
            int y = (int)Random.Range(-randomRot.y, randomRot.y);
            int z = (int)Random.Range(-randomRot.z, randomRot.z);
            gc.startAngle = Quaternion.Euler(new Vector3(x, y, z));
        }
    }
}
