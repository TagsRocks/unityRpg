using UnityEngine;
using System.Collections;
using ChuMeng;

public class RemoveMonsterInZone : MonoBehaviour {
    [ButtonCallFunc()]
    public bool Remove;
    public void RemoveMethod(){
        var pro = transform.Find("properties");
        foreach(Transform t in pro){
            var trigger = t.GetComponent<SpawnTrigger>();
            if(trigger != null){
                for(int i = 0; i < t.childCount; i++){
                    GameObject.DestroyImmediate(t.GetChild(i).gameObject);

                }
            }
        }
    }

    [ButtonCallFunc()]
    public bool Reset;
    public void ResetMethod(){
        var pro = transform.Find("properties");
        foreach(Transform t in pro){
            var trigger = t.GetComponent<SpawnTrigger>();
            if(trigger != null){
                trigger.reset = true;
                trigger.UpdateEditor();
                trigger.UpdateEditor();
            }
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
