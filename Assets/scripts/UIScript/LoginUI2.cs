using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class LoginUI2 : IUserInterface
    {
        void Awake(){
            SetCallback("StartButton", OnStart);
        }
        void OnStart(GameObject g){
            GameInterface_Login.loginInterface.LoginGame();
        }
        // Use this for initialization
        void Start()
        {
    
        }
    
        // Update is called once per frame
        void Update()
        {
    
        }
    }

}