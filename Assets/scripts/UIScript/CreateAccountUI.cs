using UnityEngine;
using System.Collections;
namespace ChuMeng {
	public class CreateAccountUI : IUserInterface {
		void Awake() {
			SetCallback ("startRegister", OnReg);
			SetCallback ("createReturn", Hide);
		}
		void OnReg(GameObject g) {
			var nameInput = GetInput ("nameInput");
			var passwordInput = GetInput("password");
			var pass2 = GetInput("pass2");

			if (nameInput.text == "" || passwordInput.text == "" || pass2.text == "" || passwordInput.text != pass2.text) {
				//showLog(Util.GetString("inputError"));
			} else {
				GameInterface_Login.loginInterface.RegisterAccount(nameInput.text, passwordInput.text);
				//loginInit.startRegister();
			}
		}
	}
}
