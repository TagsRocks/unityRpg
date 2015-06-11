using UnityEngine;
using System.Collections;

public class LightCamera : MonoBehaviour {
    public Vector3 CamPos;
    void Awake() {
        DontDestroyOnLoad(gameObject);
        
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Camera.main.transform.position+CamPos;
	}
}
