using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class TestRemoteServer : MonoBehaviour
    {
        void Awake() {
            gameObject.AddComponent<SaveGame>();
        }
        // Use this for initialization
        void Start()
        {
    
        }
    
        [ButtonCallFunc()]
        public bool
            Connect;

        RemoteClient rc;
        public void ConnectMethod()
        {
            rc = new RemoteClient();
            rc.Connect("127.0.0.1", 10001);

        }

        [ButtonCallFunc()]public bool Send;
        public void SendMethod() {
            var pk = CGLoginAccount.CreateBuilder();
            pk.Username = "liyong";
            pk.Password = "123456";
            var data = KBEngine.Bundle.GetPacket(pk);
            rc.Send(data);

        }

    }

}