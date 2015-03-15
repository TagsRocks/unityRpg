
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
using KBEngine;
using Google.ProtocolBuffers;

namespace ChuMeng
{

	public class LoginInit : UnityEngine.MonoBehaviour
	{
		//GameObject uiRoot;
		//LoginUI loginUI;
		//CharacterUI charUI;
	
		//GCLoginAccount charInfo;
		/*
		public GCLoginAccount GetCharInfo() {
			return charInfo;
		}
		*/
		static LoginInit loginInit;
		public static LoginInit GetLogin() {
			return loginInit;
		}
		SaveGame sg;
		void Awake ()
		{
			loginInit = this;

			//uiRoot = GameObject.FindGameObjectWithTag ("UIRoot");
			//loginUI = uiRoot.GetComponent<LoginUI> ();
			//charUI = uiRoot.GetComponent<CharacterUI> ();


			if (SaveGame.saveGame == null) {
				var saveGame = new GameObject("SaveGame");
				saveGame.AddComponent<SaveGame>();
				saveGame.GetComponent<SaveGame>().InitData();
				saveGame.GetComponent<SaveGame>().InitServerList();
			}
			sg = SaveGame.saveGame;
		}



		void Test(GameObject g) {
			/*
			Debug.Log("login CGLoginAccount");
			CGLoginAccount.Builder account = CGLoginAccount.CreateBuilder ();
			
			//account.Uuid = username;
			var data = account.Build ();
			byte[] bytes;
			using (System.IO.MemoryStream stream = new System.IO.MemoryStream()) {
				data.WriteTo (stream);
				bytes = stream.ToArray ();
			}
			Bundle bundle = new Bundle ();
			bundle.newMessage ("MPlayerProtoc", "CGLoginAccount");
			uint fid = bundle.writePB (bytes);
			bundle.send (KBEngineApp.app.networkInterface(), TestGCLoginAccount, fid);
			*/

		}

		public IEnumerator loginCoroutine() {
			Debug.Log("LoginInit::loginCoroutine start login CGLoginAccount");
			CGLoginAccount.Builder account = CGLoginAccount.CreateBuilder ();
			Debug.Log ("save Game Data ");

			account.Username = SaveGame.saveGame.otherAccounts[0]["username"];
			account.Password = SaveGame.saveGame.otherAccounts[0]["password"];

			var data = account.Build ();
			Bundle bundle = new Bundle ();
			bundle.newMessage (data.GetType());
			uint fid = bundle.writePB (data);
			PacketHolder packet = new PacketHolder();
			yield return StartCoroutine (bundle.sendCoroutine(KBEngineApp.app.networkInterface(), fid, packet));

			if (packet.packet.responseFlag == 0) {
				//charInfo = packet.packet.protoBody as GCLoginAccount;
				SaveGame.saveGame.charInfo = packet.packet.protoBody as GCLoginAccount;

				//ShowCharacterPanel();
				Application.LoadLevel("XuanZeRenWu");
				//var g = new GameObject("CharSelectLogic");
				//g.AddComponent<CharSelectLogic>();

			} else {
				//loginUI.showLog(Util.GetString("loginError"));
				WindowMng.windowMng.ShowNotifyLog(Util.GetString("loginError"), 3);
			}

			Debug.Log ("LoginInit::loginCoroutine finish login");

		}

		
		public IEnumerator regAndLog() {
			Debug.Log ("LoginInit::regAndlog: start Register");
			PacketHolder packet = new PacketHolder ();
			{
				CGAutoRegisterAccount.Builder auto = CGAutoRegisterAccount.CreateBuilder ();
				var data = auto.Build ();
				Bundle bundle = new Bundle ();
				bundle.newMessage (typeof(CGAutoRegisterAccount));
				uint fid = bundle.writePB (data);
				yield return StartCoroutine (bundle.sendCoroutine (KBEngineApp.app.networkInterface(), fid, packet));
			}

			{
				var newAccount = packet.packet.protoBody as GCAutoRegisterAccount;
				CGRegisterAccount.Builder reg = CGRegisterAccount.CreateBuilder ();
				reg.Username = newAccount.Username;
				//reg.Password = newAccount.Password;
				//reg.Password = newAccount.Username;
				reg.Password = "123456";
				var data = reg.BuildPartial ();
				Debug.Log("LoginInit::regAndLog: "+newAccount+" : "+data);
				Debug.Log("LoginInit::regAndLog:  username pass "+newAccount.Username+" "+data.Password);
				Bundle bundle = new Bundle ();
				bundle.newMessage (data.GetType ());
				var fid = bundle.writePB (data);
				yield return StartCoroutine (bundle.sendCoroutine (KBEngineApp.app.networkInterface(), fid, packet));
				if (packet.packet.responseFlag == 0) {

					SaveGame.saveGame.AddNewAccount(newAccount.Username, data.Password);
					SaveGame.saveGame.SaveFile ();
				} else {

					//loginUI.showLog (Util.GetString ("autoRegError"));
				}
			}

			yield return StartCoroutine (loginCoroutine());
			Debug.Log("LoginInit::regAndlog: finish register");
		}

		IEnumerator startRegisterCoroutine(string name, string pass) {

			Debug.Log ("LoginInit::startRegisterCoroutine: ");
			//var name = loginUI.nameInput.text;
			//var pass = loginUI.passwordInput.text;

			PacketHolder packet = new PacketHolder ();
			CGRegisterAccount.Builder reg = CGRegisterAccount.CreateBuilder ();
			reg.Username = name;
			reg.Password = pass;

			yield return StartCoroutine (Bundle.sendSimple(this, reg, packet));

			if (packet.packet.responseFlag == 0) {
				SaveGame.saveGame.AddNewAccount(name, pass);

				SaveGame.saveGame.SaveFile();
				//Login Game Auto

				yield return StartCoroutine(loginCoroutine());
			} else {
				//loginUI.showLog(Util.GetString("autoRegError"));
			}

		}

		public void startRegister(string name, string pass) {
			StartCoroutine (startRegisterCoroutine (name, pass));
		}


		void Start ()
		{
			WindowMng.windowMng.PushView ("UI/loginUI");
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.UpdateLogin);

		}



		IEnumerator DeleteCharCoroutine(long playerId) {
			yield return null;
			/*
			var packet = new PacketHolder ();
			CGDelCharacter.Builder delChar = CGDelCharacter.CreateBuilder ();
			delChar.Username = SaveGame.saveGame.GetDefaultUserName ();
			delChar.Password = SaveGame.saveGame.GetDefaultPassword ();


			delChar.PlayerId = playerId;
			var data = delChar.BuildPartial ();
			Bundle bundle = new Bundle ();
			bundle.newMessage (data.GetType());
			var fid = bundle.writePB (data);
			yield return StartCoroutine (bundle.sendCoroutine(KBEngineApp.app.networkInterface(), fid, packet));
			if (packet.packet.responseFlag == 0) {
				GCDelCharacter delRes = packet.packet.protoBody as GCDelCharacter;

				GCLoginAccount.Builder cinfo = GCLoginAccount.CreateBuilder();
				foreach(RolesInfo ri in charInfo.RolesInfoList) {
					if(ri.PlayerId != playerId) {
						cinfo.RolesInfoList.Add(ri);
					}
				}

				charInfo = cinfo.Build();
				charUI.deleteChar(charInfo);
			} else {
			}
			*/
		}

		public void DeleteChar(long playerId) {
			StartCoroutine (DeleteCharCoroutine(playerId));
		}








	}

}
