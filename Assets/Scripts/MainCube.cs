using UnityEngine;
using System.Collections.Generic;

public class MainCube : MonoBehaviour
{
    Transform[] allChilds;

    public Transform HollowCubePrefab;
    List<Transform> possiblePosList;

    public bool Main = false;

    void Awake()
    {
        if (Main == false)
        {
            allChilds = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                allChilds[i] = transform.GetChild(i);
            }

            possiblePosList = new List<Transform>();
            foreach (Transform t in allChilds)
            {
                if (Physics.Raycast(t.position - t.forward * transform.localScale.z, t.forward, transform.localScale.z))
                {
                    Destroy(t.gameObject);
                }
                else
                {
                    possiblePosList.Add(t);
                }
            }

            allChilds = new Transform[possiblePosList.Count];
            allChilds = possiblePosList.ToArray();
        }

    }

    private void Start()
    {
        if (GameManager.GameStarted)
        {
            SpawnHollow();
        }
        else if(transform.GetSiblingIndex() == 0)
        {
            GameManager.instance.unspawnedMainCube = transform;
        }
    }

    public void SpawnHollow()
    {
        Transform t1 = Instantiate(HollowCubePrefab, allChilds[0].position, allChilds[0].rotation);
        t1.localScale = transform.localScale;
        t1.GetComponent<CubePlacement>().transPoints = allChilds;
        t1.GetComponent<CubePlacement>().Parent = transform;    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Finish" && GameManager.GameStarted)
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GameManager.instance.GameOver();
        }
    }

    public void StartSetter()
    {
        allChilds = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            allChilds[i] = transform.GetChild(i);
        }

        possiblePosList = new List<Transform>();
        foreach (Transform t in allChilds)
        {
            if (Physics.Raycast(t.position - t.forward * transform.localScale.z, t.forward, transform.localScale.z))
            {
                Destroy(t.gameObject);
            }
            else
            {
                possiblePosList.Add(t);
            }
        }

        allChilds = new Transform[possiblePosList.Count];
        allChilds = possiblePosList.ToArray();
    }
}
