using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetsGenerator : MonoBehaviour
{
    [Range(1,5)]
    public float X = 2, Y = 2, Z = 2;

    public Transform TargetCubesPrefab;

    public int NoofCubes = 5;

    public List<Vector3> PossiblePositions = new List<Vector3>();

    Transform[] SpawnedCubes;
    Transform CurrentCube;
    Transform MainCube;

    void Start()
    {
        for(int x = 0; x < X; x++)
        {
            for(int y = 0; y < Y; y++)
            {
                for(int z = 0; z < Z; z++)
                {
                    PossiblePositions.Add(new Vector3(x, y, z));
                }
            }
        }

        CurrentCube = Instantiate(TargetCubesPrefab, PossiblePositions[0], Quaternion.identity);
        MainCube = CurrentCube;
        PossiblePositions.Remove(PossiblePositions[0]);

        SpawnedCubes = new Transform[NoofCubes - 1];
        for (int i = 0; i < NoofCubes - 1; i++)
        {
            SpawnedCubes[i] = SpawnCubes();
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SpawnedCubes = new Transform[NoofCubes - 1];
            for(int i = 0; i < NoofCubes - 1; i++)
            {
                 SpawnedCubes[i] = SpawnCubes();
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            if(SpawnedCubes[0] != null)
            {
                foreach(Transform t in SpawnedCubes)
                {
                    Destroy(t.gameObject);
                }
                CurrentCube = MainCube;

                PossiblePositions.Clear();
                for (int x = 0; x < X; x++)
                {
                    for (int y = 0; y < Y; y++)
                    {
                        for (int z = 0; z < Z; z++)
                        {
                            PossiblePositions.Add(new Vector3(x, y, z));
                        }
                    }
                }
            }

            else
            {
                Debug.Log("No Cubes to Destroy");
            }
        }
    }

    Transform SpawnCubes()
    {
        Vector3[] pos = new Vector3[6];
        pos[0] = CurrentCube.position + Vector3.up;
        pos[1] = CurrentCube.position + Vector3.down;
        pos[2] = CurrentCube.position + Vector3.forward;
        pos[3] = CurrentCube.position + Vector3.back;
        pos[4] = CurrentCube.position + Vector3.right;
        pos[5] = CurrentCube.position + Vector3.left;

        List<Vector3> ta = new List<Vector3>();

        foreach(Vector3 v in pos)
        {
            for(int i = 0; i < PossiblePositions.Count; i++)
            {
                if(v == PossiblePositions[i])
                {
                    ta.Add(v);
                }
            }
        }

        Vector3[] curPossiblePos = new Vector3[ta.Count];
        curPossiblePos = ta.ToArray();

        int c = Random.Range(0, curPossiblePos.Length);

        CurrentCube = Instantiate(TargetCubesPrefab, curPossiblePos[c], Quaternion.identity);

        PossiblePositions.Remove(curPossiblePos[c]);

        return CurrentCube;
    }
}
