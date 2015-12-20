using UnityEngine;
using System.Collections;
using ChuMeng;
public class TestTask : MonoBehaviour {

    public int v = 4;

    [ButtonCallFunc()]public bool SetBool;
    public void SetBoolMethod() {
        GameInterface_Player.SetIntState(GameBool.cunZhangState, v);
    }

}
