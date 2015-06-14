
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/

/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;

namespace ChuMeng
{
	public class LoginUI : IUserInterface
	{
		public GameObject loginBut;
		public GameObject logLabel;
		public GameObject zhanghaoBut;
		public UILabel fastLogin;
		public GameObject selectAccount;
		public GameObject returnBut;

		public GameObject content;

		public List<GameObject> accountList;
		public GameObject createBut;

		//public GameObject createAccountPanel;

		public UILabel nameInput;
		public UILabel passwordInput;
		public UILabel passwordInput2;
		public GameObject loginGame;


		public GameObject addAccount;

		//public GameObject lastPanel;
		public GameObject createReturn;
		public GameObject startRegister;


		public UILabel serverLabel;

		public GameObject loginback;
		public GameObject loginUI;
		/*
		 * TODO: Make Sure all button press check if pressYet 
		 * such as Register Button
		 * 
		 */ 
		//LoginInit loginInit;

		int currentSelect = 0;

		public GameObject serverBut;

		public GameObject serverTemplate;
		public GameObject serverListPanel;
		public UIPopupList serverList;

		void Awake ()
		{
			//loginInit = GameObject.FindObjectOfType<LoginInit> ();

			accountList = new List<GameObject> ();
			for (int i=0; i < 3; i++) {
				var ac = Util.FindChildRecursive (transform, string.Format("account{0}", i)).gameObject;
				accountList.Add (ac);
				//var cp = i;
				UIEventListener.Get(ac).onClick = onSelect;
			}

			loginBut = Util.FindChildRecursive(transform, "loginBut").gameObject;
			//UIEventListener.Get (loginBut).onClick = OnLogin;
			logLabel = Util.FindChildRecursive (transform, "logLabel").gameObject;
			logLabel.SetActive (false);
			fastLogin = Util.FindChildRecursive (transform, "fastLogin").GetComponent<UILabel> ();

			zhanghaoBut = Util.FindChildRecursive (transform, "zhanghaoBut").gameObject;
			UIEventListener.Get (zhanghaoBut).onClick = onAccount;

			content = Util.FindChildRecursive (transform, "content").gameObject;
			content.SetActive (true);
			//lastPanel = content;


			selectAccount = Util.FindChildRecursive (transform, "selectAccount").gameObject;
			selectAccount.SetActive (false);

			returnBut = Util.FindChildRecursive (transform, "returnBut").gameObject;
			UIEventListener.Get (returnBut).onClick = OnReturn;

			createBut = Util.FindChildRecursive (transform, "createButton").gameObject;
			UIEventListener.Get (createBut).onClick = OnCreate;

			//createAccountPanel = Util.FindChildRecursive (transform, "createAccountPanel").gameObject;
			//createAccountPanel.SetActive (false);

			/*
			nameInput = Util.FindChildRecursive (transform, "nameInput").GetComponent<UILabel> ();
			nameInput.text = "test" + UnityEngine.Random.Range (1, 10000);
			passwordInput = Util.FindChildRecursive (transform, "passwordInput").GetComponent<UILabel> ();
			passwordInput2 = Util.FindChildRecursive (transform, "passwordInput2").GetComponent<UILabel> ();
			*/

			addAccount = Util.FindChildRecursive (transform, "addAccount").gameObject;
			UIEventListener.Get (addAccount).onClick = OnAdd;

			//addAccountPanel = Util.FindChildRecursive (transform, "addAccountPanel").gameObject;
			//addAccountPanel.SetActive (false);

			//backbut = Util.FindChildRecursive (transform, "backbut").gameObject;
			//UIEventListener.Get (backbut).onClick = OnAddBack;


			//createReturn = Util.FindChildRecursive (transform, "createReturn").gameObject;
			//UIEventListener.Get (createReturn).onClick = OnCreateReturn;

			//startRegister = Util.FindChildRecursive (transform, "startRegister").gameObject;
			//UIEventListener.Get (startRegister).onClick = OnStartRegister;


			loginGame = Util.FindChildRecursive (transform, "loginGame").gameObject;
			UIEventListener.Get (loginGame).onClick = OnLoginGame;

			serverLabel = Util.FindChildRecursive (transform, "serverLabel").GetComponent<UILabel> ();
			loginback = Util.FindChildRecursive (transform, "loginback").gameObject;
			//loginUI = Util.FindChildRecursive (transform, "loginUI").gameObject;
			loginUI = gameObject;

			//loginUI.SetActive (true);

			serverBut = Util.FindChildRecursive (transform, "serverBut").gameObject;
			serverList = serverBut.GetComponent<UIPopupList> ();

			regEvt = new List<MyEvent.EventType> (){
				MyEvent.EventType.UpdateLogin,
			};
			RegEvent ();
		}
		protected override void OnEvent (MyEvent evt)
		{
			UpdateFrame ();
		}

		void RegisterAndLogin(GameObject g) {
			GameInterface_Login.loginInterface.FastRegister ();
		}

		void UpdateFrame() {
			serverList.items.Clear();
			foreach(SimpleJSON.JSONNode n in GameInterface_Login.loginInterface.GetServerList()) {
				serverList.items.Add(n.AsObject["serverName"]);
			}
			serverList.value = serverList.items[0];
		
			if (GameInterface_Login.loginInterface.GetSaveAccountNum() == 0) {
				fastLogin.text = Util.GetString ("fastLogin");
				serverLabel.text = serverList.value;
				UIEventListener.Get (loginBut).onClick = RegisterAndLogin;
			} else {
				fastLogin.text = Util.GetString ("login");
				serverLabel.text = serverList.value+"("+SaveGame.saveGame.GetDefaultUserName ()+")";
				UIEventListener.Get (loginBut).onClick = OnLogin;
			}
		}
		
		void onSelect(GameObject g) {
			var id = Convert.ToInt32(g.name.Replace("account", ""));
			if (SaveGame.saveGame.otherAccounts.Count > id) {
				currentSelect = id;
				UpdateSelect ();
			}
		}
		/*
		 * Current Select To Login
		 */ 
		void OnLoginGame(GameObject g) {
			GameInterface_Login.loginInterface.SelectAccounAndLogin (currentSelect);
			/*
			SaveGame.saveGame.SwapFirstAccount (currentSelect);
			currentSelect = 0;
			loginInit.OnLogin (null);
			*/
		}

		/*
		void OnStartRegister(GameObject g) {
			if (nameInput.text == "" || passwordInput.text == "" || passwordInput2.text == "" || passwordInput.text != passwordInput2.text) {
				showLog(Util.GetString("inputError"));
			} else {
				GameInterface_Login.loginInterface.RegisterAccount();
				//loginInit.startRegister();
			}
		}
		*/

		void OnCreateReturn(GameObject g) {
			//uiManager.PopView ();
		}

		void OnAdd(GameObject g) {
			//uiManager.PushView (addAccountPanel);
		}
		void OnAddBack(GameObject g) {
			//uiManager.PopView ();
		}

		void OnCreate(GameObject g) {
			//uiManager.PushView (createAccountPanel);
		}

		void OnReturn(GameObject g) {
			//uiManager.PopView ();
		}

		void onAccount(GameObject g) {
			//uiManager.PushView (selectAccount);
			//UpdateSelect ();
			WindowMng.windowMng.PushView ("UI/addAccountPanel");
		}

		void UpdateSelect() {

			int c = 0;
			foreach (JSONClass js in SaveGame.saveGame.otherAccounts) {
				var lab = Util.FindChildRecursive(accountList[c].transform, "Label").GetComponent<UILabel>();
				var uname = js["username"];
				lab.text = uname;

				var mark = Util.FindChildRecursive(accountList[c].transform, "selectMark").gameObject;
				if(currentSelect == c) {
					mark.SetActive(true);
				}else {
					mark.SetActive(false);
				}
				c++;
				if(c >= 3) {
					break;
				}
			}

			for (int i=c; i < 3; i++) {
				var lab = Util.FindChildRecursive(accountList[i].transform, "Label").GetComponent<UILabel>();
				lab.text = "None";
				var mark = Util.FindChildRecursive(accountList[i].transform, "selectMark").gameObject;
				mark.SetActive(false);
			}
		}

		void OnLogin(GameObject g) {
			GameInterface_Login.loginInterface.LoginGame ();
		}
		// Use this for initialization
		void Start ()
		{
			//uiManager.PushView (content);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void showLog(string msg) {
			logLabel.SetActive (true);
			logLabel.GetComponent<UILabel> ().text = msg;
		}

		public void Close() {
			//uiManager.PopAll ();
			loginUI.SetActive (false);
		}
	}

}