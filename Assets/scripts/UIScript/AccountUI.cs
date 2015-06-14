using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	public class AccountUI : IUserInterface
	{
		void Awake() {
			SetCallback ("close", Hide);
			SetCallback ("backbut", OnRegister);
			SetCallback ("loginAccountbut", OnLogin);
		}
		void OnLogin(GameObject g) {
			GameInterface_Login.loginInterface.LoginWithUserNamePass (GetInput("username").value, GetInput("password").value);

		}
		void OnRegister(GameObject g) {
			WindowMng.windowMng.PushView ("UI/createAccountPanel");
		}
	}
}
