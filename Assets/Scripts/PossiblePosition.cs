using UnityEngine;

public class PossiblePosition : MonoBehaviour
{
    RaycastHit hit;

    void Awake()
    {
        if(Physics.Raycast(transform.position - transform.forward, transform.forward,out hit, 2))
        {
            Debug.Log("Object name = " + hit.transform.name);
            Destroy(gameObject);
        }
        Debug.Log("Awake");
    }
}
