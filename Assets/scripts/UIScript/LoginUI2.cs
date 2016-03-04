using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class LoginUI2 : IUserInterface
    {
        UIInput ip;
        UIInput port;
        void Awake(){
            SetCallback("StartButton", OnStart);
            ip = GetInput("IPInput");
            port = GetInput("PortInput");
            SetCallback("StartServer", OnServer);
        }
        private bool serverYet = false;
        void OnServer() {
            var ca = ClientApp.Instance;
            ca.remoteServerIP = ip.value;
            ca.testPort = System.Convert.ToInt32(port.value);
            ca.StartServer();
            serverYet = true;
        }

        void OnStart(GameObject g){
            if(!serverYet) {
                OnServer();
            }
            GameInterface_Login.loginInterface.LoginGame();
        }

      
    }

}