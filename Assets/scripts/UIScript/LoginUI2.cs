using UnityEngine;
using System.Collections;

namespace MyLib
{
    public class LoginUI2 : IUserInterface
    {
        UIInput ip;
        UIInput port;
        UIInput sync;

        void Awake()
        {
            SetCallback("StartButton", OnStart);
            ip = GetInput("IPInput");
            port = GetInput("PortInput");
            SetCallback("StartServer", OnServer);
            sync = GetInput("SyncInput");
        }

        IEnumerator InitJson()
        {
            while (true)
            {
                var w = new WWW("http://www.52unity.info:8080/server.json");
                yield return w;
                if (!string.IsNullOrEmpty(w.error))
                {
                    Log.Net(w.error);
                    var f = Localization.Get("NetFail");
                    Util.ShowMsg(f);
                } else
                {
                    var server = SimpleJSON.JSONNode.Parse(w.text).AsArray;
                    var jobj = server [0].AsObject;
                    var iv = jobj ["ip"].Value;
                    Log.Net("ServerIP:" + iv);
                    ip.value = iv;
                    loadIp = true;
                    break;
                }
                yield return new WaitForSeconds(5);
            }
        }

        private bool loadIp = false;

        IEnumerator Start()
        {
            yield return StartCoroutine(InitJson());
            if(NetDebug.netDebug.IsTest) {
                ip.value = "127.0.0.1";
            }

            if (NetDebug.netDebug.JumpLogin)
            {
                OnStart(null);
            }
        }

        private bool serverYet = false;

        void OnServer()
        {
            var ca = ClientApp.Instance;
            ca.remoteServerIP = ip.value;
            ca.testPort = System.Convert.ToInt32(port.value);
            ca.syncFreq = System.Convert.ToSingle(sync.value);
            ca.StartServer();
            serverYet = true;
        }

        void OnStart(GameObject g)
        {
            if (!loadIp)
            {
                Util.ShowMsg(Localization.Get("InitConfig"));
                return;
            }
            if (!serverYet)
            {
                OnServer();
            }
            GameInterface_Login.loginInterface.LoginGame();
        }

      
    }

}