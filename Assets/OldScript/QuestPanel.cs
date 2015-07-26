
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
using System;
using StringInject;

namespace ChuMeng {
	public class QuestPanel : MonoBehaviour {
		public GameObject dialog;
		GameObject player;

		GameObject panel;
		UILabel goldNum;
		UILabel expNum;
		UILabel questText;

		public VoidDelegate OnOk;

		GameObject okBut;
		void Awake() {
			dialog = Util.FindChildRecursive (transform, "DialogBut").gameObject;
			UIEventListener.Get (dialog).onClick = OnDialog;
			panel = Util.FindChildRecursive (transform, "QuestPanel").gameObject;
			panel.SetActive (false);

			goldNum = Util.FindChildRecursive (panel.transform, "goldNum").GetComponent<UILabel> ();
			expNum = Util.FindChildRecursive (panel.transform, "expNum").GetComponent<UILabel> ();
			questText = Util.FindChildRecursive (panel.transform, "questText").GetComponent<UILabel> ();
			okBut = Util.FindChildRecursive (panel.transform, "questOk").gameObject;
			UIEventListener.Get (okBut).onClick = ok;
		}
		void ok(GameObject g) {
			if (OnOk != null) {
				OnOk(g);
			}
			panel.SetActive (false);
		}
	
		public void ShowQuest(QuestData qd, QuestAI.QuestState state) {
			if (qd == null) {
				return;
			}

			panel.SetActive (true);
			
			var ht = new Hashtable ();
			ht.Add ("num", qd.RewardGold);
			goldNum.text = Util.GetString ("goldNum").Inject (ht);

			ht.Clear ();
			ht.Add ("num", qd.RewardXP);
			expNum.text = Util.GetString ("expNum").Inject (ht);

			switch(state) {
			case QuestAI.QuestState.INTRO:
				questText.text = Convert.ToString (qd.Intro);
				break;
			case QuestAI.QuestState.RETURN:
				questText.text = Convert.ToString (qd.Return);
				break;
			case QuestAI.QuestState.COMPLETE:
				questText.text = Convert.ToString (qd.Complete);
				break;
			}
		}

		void OnDialog(GameObject g) {
			if (player != null) {
				var forward = player.transform.forward;
				forward.y = 0;
				forward.Normalize();

				float minDir = Mathf.Cos((float)(Mathf.PI/4));
				var col = Physics.OverlapSphere(player.transform.position, 2);

				GameObject questNpc = null;
				foreach(Collider c in col) {
					if(c.gameObject.GetComponent<QuestAI>() != null) {
						var dir = c.gameObject.transform.position-player.transform.position;
						dir.y = 0;
						dir.Normalize();
						if(Vector3.Dot(forward, dir) > minDir) {
							questNpc = c.gameObject;
							break;
						}

					}
				}
				if(questNpc != null) {
					questNpc.GetComponent<QuestAI>().Speech();
				}
			}
		}
		// Use this for initialization
		IEnumerator Start () {
			player = null;
			while (player == null) {
				player = GameObject.FindGameObjectWithTag("Player");
				yield return null;
			}
		}
		//Check Player Forward has Npc
		// Update is called once per frame
		void Update () {

		}
	}

}