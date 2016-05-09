using UnityEngine;
using System.Collections;

public class SpawnDefense : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        foreach (Transform t in this.transform)
        {
            t.gameObject.SetActive(false);
        }
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }
}
