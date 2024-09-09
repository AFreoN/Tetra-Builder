using UnityEditor;
using UnityEngine;

public class CubeReplacer : EditorWindow
{
    Transform GroundCubes;
    Transform gc2;

    Transform[] old;
    Transform[] New;

    [MenuItem("My Tools/Cube Replacer")]
    public static void ShowCubeReplacerWindow()
    {
        GetWindow<CubeReplacer>("Cube Replacer");
    }

    private void OnEnable()
    {
         GroundCubes = Resources.Load<Transform>("GroundCube");
        gc2 = Resources.Load<Transform>("GC2");
    }

    private void OnGUI()
    {
        GUILayout.Label("Cube Replacer Window");

        if(Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<GroundCubesAssembler>() != null)
        {
            if(GUILayout.Button("Replace"))
            {
                Replace();
            }
        }
        else
        {
            GUILayout.Label("Select Level to Replace");
        }

        if(Selection.gameObjects.Length > 0)
        {
            if(GUILayout.Button("Replace GC2"))
            {
                GameObject[] allt = new GameObject[Selection.objects.Length];
                Debug.Log("objects Generated = " + allt.Length);
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    allt[i] = Selection.objects[i] as GameObject;
                }
                foreach (GameObject t in allt)
                {
                    GameObject c = PrefabUtility.InstantiatePrefab(gc2.gameObject) as GameObject;
                    if (t != null)
                    {
                        c.transform.position = t.transform.position;
                        c.transform.rotation = t.transform.rotation;
                        DestroyImmediate(t.gameObject);
                    }
                }
            }
        }
    }

    void Replace()
    {
        Transform t = Selection.activeGameObject.transform;
        GameObject n = new GameObject("Ground Cubes");
        old = new Transform[t.childCount];
        for(int i = 0; i < t.childCount; i++)
        {
            old[i] = t.GetChild(i);
        }

        New = new Transform[old.Length];
        for(int j = 0; j < old.Length; j++)
        {
            //New[j] = Instantiate(GroundCubes, old[j].position, old[j].rotation);
            GameObject c = PrefabUtility.InstantiatePrefab(GroundCubes.gameObject) as GameObject;
            New[j] = c.transform;
            New[j].position = old[j].position;
            New[j].rotation = old[j].rotation;
            New[j].SetParent(t);
            c.name = "GC1";
            DestroyImmediate(old[j].gameObject);
        }
    }
}
