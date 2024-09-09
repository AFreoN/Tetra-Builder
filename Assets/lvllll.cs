using UnityEngine;
using System.Collections.Generic;

public class lvllll : MonoBehaviour
{
    public Transform SpherePrefab;
    public Transform LinePrefab;
    public int Min = 5, Max = 11;

    public Vector3 XYZ = new Vector3(4, 3, 4);

    Vector3[] allPos = { new Vector3(0, 0, 1), new Vector3(0 ,0, -1), new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 1 , 0), new Vector3(0, -1, 0)};
    Transform[] allSphere;
    Transform[] allLines;

    List<Vector3> allPossiblePosition = new List<Vector3>();
    bool generated = false;

    void Start()
    {
        for(int i = 0; i < XYZ.x; i++)
        {
            for(int j = 0; j < XYZ.y; j ++)
            {
                for (int k = 0; k < XYZ.z; k++)
                {
                    allPossiblePosition.Add(new Vector3(i, j, k));
                }
            }
        }

        Generate();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (generated == false)
            {
                Generate();
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            if(generated)
            {
                DeleteALL();
            }
            else
            {
                Debug.LogWarning("Nothing to Delete");
            }
        }
    }

    void DeleteALL()
    {
        for(int i = 0; i < allSphere.Length; i++)
        {
            Destroy(allSphere[i].gameObject);
        }
        for(int i = 0; i < allLines.Length; i++)
        {
            if (allLines[i] != null)
            {
                Destroy(allLines[i].gameObject);
            }
        }
        allPossiblePosition.Clear();

        for (int i = 0; i < XYZ.x; i++)
        {
            for (int j = 0; j < XYZ.y; j++)
            {
                for (int k = 0; k < XYZ.z; k++)
                {
                    allPossiblePosition.Add(new Vector3(i, j, k));
                }
            }
        }
        generated = false;
        //Generate();
    }

    void Generate()
    {
        int r = Random.Range(Min, Max + 1);
        allSphere = new Transform[r];
        allLines = new Transform[r];
        for (int i = 0; i < r; i++)
        {
            Vector3 v;
            if (i != 0)
            {
                v = getPos(allSphere[i - 1].position);
                allSphere[i] = Instantiate(SpherePrefab, v, Quaternion.identity);
                allLines[i] = Instantiate(LinePrefab, Vector3.zero, Quaternion.identity);
                LineRenderer lr = allLines[i].GetComponent<LineRenderer>();
                lr.positionCount = 2;
                lr.SetPosition(0, allSphere[i - 1].position);
                lr.SetPosition(1, allSphere[i].position);
                
                //Debug.DrawRay(allSphere[i - 1].position, allSphere[i].position - allSphere[i - 1].position, Color.green, 20f);
            }
            else
            {
                v = getPos(Vector3.zero);
                allSphere[i] = Instantiate(SpherePrefab, v, Quaternion.identity);
            }
        }
        generated = true;
    }

    Vector3 getPos(Vector3 prev)
    {
        int c = Random.Range(0, allPos.Length);
        Vector3 result = prev + allPos[c];
        while(!allPossiblePosition.Contains(result))
        {
            c = Random.Range(0, allPos.Length);
            result = prev + allPos[c];
        }
        allPossiblePosition.Remove(result);
        return result;
    }
}
