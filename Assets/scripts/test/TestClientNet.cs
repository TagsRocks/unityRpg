using UnityEngine;
using System.Collections;

public class TestClientNet : MonoBehaviour {

    [ButtonCallFunc()]public bool CloseNet;
    public void CloseNetMethod(){
        KBEngine.KBEngineApp.app.networkInterface().close();
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
