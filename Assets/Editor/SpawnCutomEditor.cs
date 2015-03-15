using UnityEngine;
using System.Collections;
using UnityEditor;
using ChuMeng;

[CustomEditor(typeof(SpawnTrigger))]
public class SpawnCutomEditor : Editor {
	void OnEnable() {
		if(Application.isEditor && !EditorApplication.isPlaying) {
			SpawnTrigger st = target as SpawnTrigger;
			EditorApplication.update = st.UpdateEditor;
		}
	}
	void OnDisable() {
		EditorApplication.update = null;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("update Editor");
		
	}
}
