
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
	public class CharacterCreateUI : IUserInterface
	{
		List<GameObject> charJob;
		int currentJob = 0;
		GameObject jobSelect = null;
		//0 1 2 3 4
		void Awake ()
		{
			regEvt = new List<MyEvent.EventType> () {
				MyEvent.EventType.UpdateCharacterCreateUI,
				MyEvent.EventType.CreateSuccess,
			};
			RegEvent ();

			charJob = new List<GameObject> ();
			for (int i = 0; i < 3; i++) {
				var name = "char"+i.ToString();
				var but = GetName(name);
				charJob.Add(but);
				int temp = i;
				SetCallback(name, delegate(GameObject g) {
					OnJob(temp);
				});
			}
			randomName ();
			SetCallback ("createChar", OnCreate);
			SetCallback ("sezi", OnRandomName);
		}
		void OnCreate(GameObject g) {
			var name = GetInput ("nameBoard").value;
			Log.GUI ("Create Char is "+ name +" "+currentJob);
			CharSelectLogic.charSelectLogic.CreateChar (name, currentJob+1);

		}

		void OnRandomName(GameObject g) {
			randomName ();
		}

		private void randomName()
		{
			Debug.Log ("click.......................................................................");
			System.Random r = new System.Random ();
			int n= r.Next(1,GameData.RollNamesConfig.Count+1);
			int m= r.Next(1,GameData.RollNamesConfig.Count+1);
			GetInput ("nameBoard").value = GameData.RollNamesConfig[n].surname + GameData.RollNamesConfig[m].forename;
		}

		void OnJob(int id) {
			currentJob = id;
			UpdateFrame ();
		}

		protected override void OnEvent (MyEvent evt)
		{
			if (evt.type == MyEvent.EventType.UpdateCharacterCreateUI) {
				UpdateFrame ();
			} else if(evt.type == MyEvent.EventType.CreateSuccess) {
				//Hide(null);
				WindowMng.windowMng.ReplaceView ("UI/CharSelect", false);
				MyEventSystem.myEventSystem.PushEvent(MyEvent.EventType.UpdateSelectChar);
			}
		}

		void UpdateJob() {
			var g = charJob [currentJob].gameObject;
			if (jobSelect == g) {
				return;
			}
			if (jobSelect != null) {
				var ctp = jobSelect.GetComponent<TweenPosition> ();
				StartCoroutine (Util.tweenReverse (ctp));
			}
			jobSelect = g;
			var tp = g.GetComponent<TweenPosition> ();
			tp.ResetToBeginning ();
			tp.enabled = true;
		}
		void UpdateModel() {
			FakeObjSystem.fakeObjSystem.OnUIShown (-1, null, currentJob+1);
		}
		void UpdateFrame() {
			UpdateJob ();	
			UpdateModel ();
		}

	}

}