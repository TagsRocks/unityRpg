using UnityEngine;
using System.Collections;

public class LogicModel : LogicNode {
    public string ani;

	// Use this for initialization
	void Start () {
        animation[ani].speed = 1;
        animation[ani].wrapMode = WrapMode.Loop;
        animation.CrossFade(ani);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void HIT(){
        logicGroup.GetComponent<CommandHandler>().AddCommand(string.Format("input_event actived {0}", myId));
    }
}
