using UnityEngine;
using System.Collections;

public class TestClientNet : MonoBehaviour {

    [ButtonCallFunc()]public bool CloseNet;
    public void CloseNetMethod(){
        KBEngine.KBEngineApp.app.networkInterface().close();
    }
    [ButtonCallFunc()]public bool TestServer;
    public void TestServerMethod(){
        ChuMeng.DemoServer.demoServer.GetThread().CloseServerSocket();
    }
    [ButtonCallFunc()] public bool Save;
    public void SaveMethod(){
        ChuMeng.ServerData.Instance.SaveUserData();
    }

    public string file;
    [ButtonCallFunc()] public bool ReadFile;
    public void ReadFileMethod(){
        var f = Resources.LoadAssetAtPath<TextAsset>("Assets/Protobuffer/"+file);
        var data = f.bytes;
        try {
            var playerInfo = ChuMeng.PlayerInfo.CreateBuilder().MergeFrom(data);
            Debug.Log(playerInfo);
        }catch(System.Exception ex){
            Debug.LogError("PlayerInfo Load Error :"+ex);
        }

    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
