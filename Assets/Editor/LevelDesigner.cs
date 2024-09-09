using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class LevelDesigner : EditorWindow
{
    [Range(1,6)]
    public int X = 3, Y = 2, Z = 3;

    public int NoofCubes = 5;

    Transform TargetCubesPrefab;
    Transform GroundCubesPrefab;

    List<Vector3> PossiblePositions = new List<Vector3>();

    Transform[] SpawnedCubes;
    Transform CurrentCube;
    Transform MainCube;

    int xt, yt, zt, nooft;

    [MenuItem("Window/Level Generator")]
    public static void ShowLevelGeneratorWindow()
    {
        GetWindow<LevelDesigner>("Level Generator");
    }

    private void OnEnable()
    {
        GameObject t = new GameObject("Target Cubes");
        Selection.activeGameObject = t;

        TargetCubesPrefab = Resources.Load<Transform>("HollowCube");
        GroundCubesPrefab = Resources.Load<Transform>("GroundCube");
        GetPossiblePosList();

        CurrentCube = Instantiate(TargetCubesPrefab, PossiblePositions[0], Quaternion.identity);
        CurrentCube.SetParent(t.transform);
        MainCube = CurrentCube;
        PossiblePositions.Remove(PossiblePositions[0]);

        Spawn();
    }

    private void OnGUI()
    {
        X = (int)EditorGUILayout.Slider("X Value" ,X, 1, 6);
        Y = (int)EditorGUILayout.Slider("Y Value", Y, 1, 6);
        Z = (int)EditorGUILayout.Slider("Z Value", Z, 1, 6);
        NoofCubes = (int)EditorGUILayout.Slider("Number of Cubes", NoofCubes, 2, 10);


        if (X != xt || Y != yt || Z != zt || NoofCubes != nooft)
        {
            GetPossiblePosList();
        }

        if (SpawnedCubes[0] == null)
        {
            if (GUILayout.Button("Generate"))
            {
                Spawn();
            }
        }
        else if (GUILayout.Button("Respawn"))
        {
            foreach (Transform t in SpawnedCubes)
            {
                DestroyImmediate(t.gameObject);
            }
            CurrentCube = MainCube;

            PossiblePositions.Clear();
            GetPossiblePosList();

            SpawnedCubes = new Transform[NoofCubes - 1];
            for (int i = 0; i < NoofCubes - 1; i++)
            {
                SpawnedCubes[i] = SpawnCubes();
            }
        }
    }

    void Spawn()
    {
        SpawnedCubes = new Transform[NoofCubes - 1];
        for (int i = 0; i < NoofCubes - 1; i++)
        {
            SpawnedCubes[i] = SpawnCubes();
        }

        for(int j = 0; j < PossiblePositions.Count; j++)
        {
            Instantiate(GroundCubesPrefab, PossiblePositions[j], Quaternion.identity);
        }
    }

    void GetPossiblePosList()
    {
        int x1 = Random.Range(0, 2) == 0 ? 1 : -1;
        int z1 = Random.Range(0, 2) == 0 ? 1 : -1;

        for (int x = 0; x < X; x++)
        {
            for (int y = 0; y > -Y; y--)
            {
                for (int z = 0; z < Z; z++)
                {
                    PossiblePositions.Add(new Vector3((x - Mathf.Round(X/2)) * x1, y , (z - Mathf.Round(Z/2)) * z1));
                }
            }
        }

        xt = X;
        yt = Y;
        zt = Z;
        nooft = NoofCubes;
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

        foreach (Vector3 v in pos)
        {
            for (int i = 0; i < PossiblePositions.Count; i++)
            {
                if (v == PossiblePositions[i])
                {
                    ta.Add(v);
                }
            }
        }

        Vector3[] curPossiblePos = new Vector3[ta.Count];
        curPossiblePos = ta.ToArray();

        int c = Random.Range(0, curPossiblePos.Length);

        CurrentCube = Instantiate(TargetCubesPrefab, curPossiblePos[c], Quaternion.identity);
        CurrentCube.name = "TargetCubes";
        if(Selection.activeGameObject!= null)
        {
            CurrentCube.SetParent(Selection.activeGameObject.transform);
        }

        PossiblePositions.Remove(curPossiblePos[c]);

        return CurrentCube;
    }
}
