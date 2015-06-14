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

			if (nameInput.value == "" || passwordInput.value == "" 
                || pass2.value == "" 
                || passwordInput.value != pass2.value) {
				//showLog(Util.GetString("inputError"));
			} else {
				GameInterface_Login.loginInterface.RegisterAccount(nameInput.value, passwordInput.value);
				//loginInit.startRegister();
			}
		}
	}
}
