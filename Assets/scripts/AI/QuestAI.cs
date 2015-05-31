
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

namespace ChuMeng {
	public class QuestAI : MonoBehaviour {
		/*
		 * Load Quest from Resource
		 * All MyQuest 
		 * MyQuest Progress
		 */ 
		public List<QuestData> myQuests;
		public int Progress = 1;
		public enum QuestState
		{
			INTRO = 0,
			RETURN = 1,
			COMPLETE = 2,
		}

		public QuestState State = 0;


		QuestDataController quest;
		GameObject headMark;

		QuestDataController controller;
		QuestPanel panel;

		void Awake() {
			/*
			myQuests = new List<QuestData> ();
			quest = GameObject.FindObjectOfType<QuestDataController> ();
			//quest belong to this npc
			foreach (QuestData qd in quest.allQuests) {
				if(qd.UnitName == this.name) {
					myQuests.Add(qd);
				}
			}

			var current = quest.GetMyCurrentQuest (this.name);
			if (current != null) {
				Progress = current.Progress;
				State = (QuestState)(current.State);
			} else {
				Progress = 0;
				State = 0;
			}

			animation.Play("idle");
			animation ["idle"].speed = 2;
			animation ["idle"].wrapMode = WrapMode.Loop;
			*/
		}

		// Use this for initialization
		void Start () {
			controller = GameObject.FindObjectOfType<QuestDataController> ();
			panel = GameObject.FindObjectOfType<QuestPanel> ();
			UpdateMark();
		}
		
		// Update is called once per frame
		void Update () {
		
		}
		QuestData GetCurrent() {
			if (Progress < myQuests.Count) {
				return myQuests [Progress];
			}
			return null;
		}
		void UpdateMark() {
			if (Progress < myQuests.Count) {
				if (headMark != null) {
					GameObject.Destroy (headMark);
					headMark = null;
				}
				
				if (State == QuestState.INTRO) {
					headMark = Instantiate (Resources.Load<GameObject> ("particles/quest/goldquestion")) as GameObject;
				} else if (State == QuestState.RETURN) {
					headMark = Instantiate (Resources.Load<GameObject> ("particles/quest/greyquestion")) as GameObject;
				} else if (State == QuestState.COMPLETE) {
					headMark = Instantiate (Resources.Load<GameObject> ("particles/quest/goldExclamation")) as GameObject;
				}
				headMark.transform.parent = transform;
				headMark.transform.localPosition = Vector3.zero;
			} else {
				if(headMark != null) {
					GameObject.Destroy(headMark);
					headMark = null;
				}
			}
		}
		public void UpdateState(QuestAI.QuestState s) {
			if (State == QuestState.COMPLETE) {
				//Add Next New Quest And State
				controller.CompleteQuest(GetCurrent());
				Progress++;
				State = QuestState.INTRO;
				controller.UpdateQuestState(GetCurrent(), Progress, State);
				UpdateMark();
			} else {
				State = s;
				UpdateMark();
			}
		}


		void OnOk(GameObject g) {
			switch (State) {
			case QuestState.INTRO:
				State++;
				controller.UpdateQuestState(GetCurrent(), Progress, State);
				UpdateState (State);
				break;
			case QuestState.RETURN:

				break;
			case QuestState.COMPLETE:
				var q = GetCurrent();
				var backpack = GameObject.FindObjectOfType<BackPack>();
				backpack.Collect(q.newName);

				backpack.PutGold(q.RewardGold);
				var player = GameObject.FindGameObjectWithTag("Player");
				player.GetComponent<NpcAttribute>().ChangeExp(q.RewardXP);

				UpdateState(State);
				break;
			}
		}

		public void Speech() {
			panel.ShowQuest (GetCurrent(), State);
			panel.OnOk = OnOk;
		}
	}

}