using UnityEditor;
using UnityEngine;

public class StoneGenerator : EditorWindow
{
    public Vector2 Density;
    public Vector2 TotalDistance;

    public float ScaleMin = .2f;
    public float ScaleMax = .36f;

    public Vector2 RotMinMax = new Vector2(0, 360);

    public AllObjects ObjectName;
    Transform Prefab;

    Transform[] SpawnedObjects;

    bool HavePrefabs = false;

    [MenuItem("Window/Stone Generator")]
    public static void ShowStoneWindow()
    {
        GetWindow<StoneGenerator>("Stone Generator");
    }

    private void OnGUI()
    {
        ObjectName = (AllObjects)EditorGUILayout.EnumPopup("Type of Object", ObjectName);
        GetPrefab(ObjectName);

        TotalDistance = EditorGUILayout.Vector2Field("Total Distance to place", TotalDistance);
        Density = EditorGUILayout.Vector2Field("Object Density", Density);

        ScaleMin = EditorGUILayout.FloatField("ScaleMin", ScaleMin);
        ScaleMax = EditorGUILayout.FloatField("ScaleMax", ScaleMax);

        RotMinMax = EditorGUILayout.Vector2Field("RotMinMax", RotMinMax);

        if (Selection.activeGameObject != null)
        {
            if (GUILayout.Button("Generate"))
            {
                if (Prefab != null && Density.x > 0 && Density.y > 0 && TotalDistance.x > 0 && TotalDistance.y > 0)
                {
                    GeneratePrefab();
                }
            }
        }
        else
        {
            if(GUILayout.Button("Create Empty"))
            {
                GameObject g = new GameObject("Stones&Grass_Holder");
                g.transform.position = Vector3.one;
                g.transform.position = Vector3.zero;
                g.transform.rotation = Quaternion.identity;
                Selection.activeGameObject = g;
            }
            //GUILayout.Label("Select the parent Object", EditorStyles.boldLabel);
        }

        if(HavePrefabs && GUILayout.Button("Delete Generated Objects"))
        {
            HavePrefabs = false;
            for(int i=0; i < SpawnedObjects.Length; i++)
            {
                DestroyImmediate(SpawnedObjects[i].gameObject);
            }
        }
    }

    void GeneratePrefab()
    {
        for (float i = 0; i < TotalDistance.x; i += Density.x)
        {
            for (float j = 0; j < TotalDistance.y; j += Density.y)
            {
                Vector3 pos = Random.insideUnitSphere * Density;
                float rot = Random.Range(RotMinMax.x, RotMinMax.y);
                pos.y = 0;

                float sc = Random.Range(ScaleMin, ScaleMax);
                Transform t = Instantiate(Prefab, new Vector3(i, 0, j) + pos, Quaternion.Euler(0, rot , 0));
                t.localScale = Vector3.one * sc;
                t.name = Prefab.name;
                t.SetParent(Selection.activeGameObject.transform);
            }
        }
        for (float i = 1; i < TotalDistance.x; i += Density.x)
        {
            for (float j = 1; j < TotalDistance.y; j += Density.y)
            {
                Vector3 pos = Random.insideUnitSphere * Density;
                float rot = Random.Range(RotMinMax.x, RotMinMax.y);
                pos.y = 0;

                float sc = Random.Range(ScaleMin, ScaleMax);
                Transform t = Instantiate(Prefab, new Vector3(-i, 0, j) + pos, Quaternion.Euler(0, rot, 0)); 
                t.localScale = Vector3.one * sc;
                t.name = Prefab.name;
                t.SetParent(Selection.activeGameObject.transform);
            }
        }

        SpawnedObjects = new Transform[Selection.activeGameObject.transform.childCount];
        for(int c = 0; c < Selection.activeGameObject.transform.childCount; c++)
        {
            SpawnedObjects[c] = Selection.activeGameObject.transform.GetChild(c);
        }

        Debug.Log(SpawnedObjects.Length + " has been Spawned");

        HavePrefabs = true;
    }

    void GetPrefab(AllObjects ao)
    {
        switch(ao)
        {
            case AllObjects.Stone1:
                Prefab = Resources.Load<Transform>("StoneSmall1");
                break;
            case AllObjects.Stone2:
                Prefab = Resources.Load<Transform>("StoneSmall2");
                break;
            case AllObjects.Grass1:
                Prefab = Resources.Load<Transform>("Grass1");
                break;
            case AllObjects.Grass2:
                Prefab = Resources.Load<Transform>("Grass2");
                break;
        }
    }

    public enum AllObjects
    {
        Stone1,
        Stone2,
        Grass1,
        Grass2
    }
}
