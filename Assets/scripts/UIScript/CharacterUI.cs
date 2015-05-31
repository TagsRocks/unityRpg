
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
using ChuMeng;
using System.Collections.Generic;
using System;

namespace ChuMeng
{
	public class CharacterUI : IUserInterface
	{
		public List<Transform> selectChar;
		GameObject curSelect = null;
		RolesInfo selectRoleInfo;

		public GameObject destroyer;
		public GameObject vanquisher;
		public GameObject alchemist;
		public GameObject stalker;
		public List<Transform> charJob;

		public GameObject createBut;
		UIInput nameLabel;
		public GameObject backBut;

		void Awake ()
		{

			selectChar = new List<Transform> ();
			/*
		 * Job  
		 * 0 Novice
		 * 1 Warrior
		 * 2 ARMOURER
		 * 3 ALCHEMIST
		 * 4 STALKER
		 * UI 
		 * 0 Warrior
		 */ 
			//selectChar.Add (null);
			for (int i = 0; i < 3; i++) {
				var c = GetName(string.Format ("char{0}", i));
				selectChar.Add (c.transform);
				UIEventListener.Get (c.gameObject).onClick = OnSelect;
			}

				
			SetCallback ("delete", OnDelete);
			SetCallback ("startGame", OnStart);

			regEvt = new List<MyEvent.EventType> (){
				MyEvent.EventType.UpdateSelectChar,
			};
			RegEvent ();
		}

		protected override void OnEvent (MyEvent evt)
		{
			UpdateFrame ();
		}

		//UpdateNameLabel
		//Update Model And Button
		public void InitUI ()
		{
			/*
		 * Show CharacterData
		 */ 
			var charInfo = GameInterface_Login.loginInterface.GetCharInfo ();

			Log.GUI ("InitUI: charInfo is " + charInfo);
			if (charInfo != null) {
				Debug.Log ("CharacterUI::InitUI  ");
				int i = 0;
				foreach (RolesInfo role in charInfo.RolesInfosList) {
					
					var c = selectChar [i];
					var name = Util.FindChildRecursive (c, "Label").GetComponent<UILabel> ();
					name.text = role.Name+" Lv "+role.Level.ToString();
					i++;
				}
				
				for (int j = i; j < 3; j++) {
					var c = selectChar [j];
					var name = Util.FindChildRecursive (c, "Label").GetComponent<UILabel> ();
					name.text = Util.GetString ("createChar");
				}
			}

		}

		void UpdateFrame() {
			InitUI ();
			SelectCharBut (null);//Choose First Character
		}

		void OnStart (GameObject g)
		{
			//loginInit.StartGame (selectCharObject.GetComponent<SelectChar> ().roleInfo);
			GameInterface_Login.loginInterface.StartGame (selectRoleInfo);
		}

		void OnDelete (GameObject g)
		{
			//loginInit.DeleteChar (selectCharObject.GetComponent<SelectChar> ().roleInfo.PlayerId);
		}


		void OnDisable() {
			var evt = new MyEvent (MyEvent.EventType.MeshHide);
			MyEventSystem.myEventSystem.PushEvent (evt);
		}

		void SelectCharBut (GameObject g)
		{
			int cid = 0;
			if (g != null) {
				cid = Convert.ToInt32 (g.newName.Replace ("char", ""));
			} else {
				
			}
			var charInfo = GameInterface_Login.loginInterface.GetCharInfo ();

			Log.GUI ("CharacterUI::SelectCharBut:" + g + " " + cid + " charInfo "+charInfo);
			bool toCreate = false;

			//Create New Character
			if (charInfo == null) {
				Log.GUI ("charInfo is null ");
				toCreate = true;
			} else if (charInfo.RolesInfosCount > cid) {
				RolesInfo role = charInfo.RolesInfosList [cid];
				Log.GUI ("CharacterUI::SelectCharBut:  ConstructChar " + role);
				Log.GUI ("RolesInfoCount " + charInfo.RolesInfosList);
			
				selectRoleInfo = role;
				var evt = new MyEvent(MyEvent.EventType.MeshShown);
				evt.intArg = -1;
				evt.rolesInfo = role;
				MyEventSystem.myEventSystem.PushEvent(evt);

				Log.GUI("MeshShown SelectRoleInfo ");
			} else {
				Log.GUI ("select empty slot ");
				toCreate = true;
			}

			GameObject newSelect = null;
			if (g == null) {
				newSelect = selectChar [cid].gameObject;
			} else {
				newSelect = g;
			}

			if (curSelect != null && curSelect != newSelect) {
				Log.GUI("curSelect shrink ");
				var ctp = curSelect.GetComponent<TweenPosition> ();	
				StartCoroutine (Util.tweenReverse (ctp));
		
			}
			curSelect = newSelect;


			Log.GUI ("CharacterUI::SelectCharBut " + curSelect + " " + toCreate);
			var tp = curSelect.GetComponent<TweenPosition> ();
			StartCoroutine (Util.tweenRun(tp));
			Log.GUI ("Set curSelect Button Move ");

			if (toCreate) {
				Log.GUI ("SelectCharBut:: toCreate");

				StartCoroutine (ShowCreate ());
			}
		}
		IEnumerator ShowCreate() {
			//yield return new WaitForSeconds(1);
			yield return null;
			Log.GUI ("Push View Create");
			//WindowMng.windowMng.PushView("UI/CharacterCreate", false, true);
			WindowMng.windowMng.ReplaceView ("UI/CharacterCreate", false);
			MyEventSystem.myEventSystem.PushEvent(MyEvent.EventType.UpdateCharacterCreateUI);
		}



		void OnSelect (GameObject g)
		{
			if (curSelect == g) {
				return;
			}
			SelectCharBut (g);
		}




	
		public void deleteChar (GCLoginAccount charInfo)
		{
			Debug.Log ("CharacterUI::deleteChar " + charInfo);
			if (charInfo != null) {
				Debug.Log ("CharacterUI::deleteChar  init Name  ");
				int i = 0;
				foreach (RolesInfo role in charInfo.RolesInfosList) {
				
					var c = selectChar [i];
					var name = Util.FindChildRecursive (c, "Label").GetComponent<UILabel> ();
					name.text = role.Name;
					i++;
				}
			
				for (int j = i; j < 3; j++) {
					var c = selectChar [j];
					var name = Util.FindChildRecursive (c, "Label").GetComponent<UILabel> ();
					name.text = Util.GetString ("createChar");
				}
			}
			SelectCharBut (null);
		}





	}

}
