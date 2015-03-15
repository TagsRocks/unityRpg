
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
using System.IO;

namespace ChuMeng {
	public class QuestDataController : MonoBehaviour {
		public static QuestDataController questDataController;

		[System.Serializable]
		public class Quest {
			public string Name; //Who's Quest vasman
			public int Progress = 0; //Current Quest Id
			public int State = 0; //Quest State
			public QuestData quest; //Quest detail information for cache


		}

		//Collect Quest From Npc
		List<Quest> currentQuests = new List<Quest>();

		List<QuestData> allQuests = new List<QuestData>();

		BackPack backpack;

		public class TaskList {
			public GCLoadTaskList allTask;
			public List<PlayerTask> playerTask;
			public List<PlayerTaskComplete> playerTaskCompletes;
			public TaskList(GCLoadTaskList t) {
				allTask = t;
			}
			public void AddTask(GCAcceptTask acc) {
			}

			public void CompleteTask(GCCompleteTask task) {
			}

			public void Abandon(GCCancelTask task) {
			}

			public void UpdateTask(GCPushTask task) {
			}
		}

		public TaskList taskList;


		[ButtonCallFunc()]
		public bool CollectQuest;

		public void CollectQuestMethod() {

#if !UNITY_WEBPLAYER
			allQuests.Clear ();

			Debug.Log ("DataPath is " + Application.dataPath);
			var parPath = Path.Combine (Application.dataPath, "Resources/quests");
			var resPath = Path.Combine(Application.dataPath, "Resources")+Path.DirectorySeparatorChar;
			var resDir = new DirectoryInfo (parPath);
			FileInfo[] fileInfo = resDir.GetFiles ("*.*", SearchOption.AllDirectories);
			foreach (FileInfo file in fileInfo) {
				Debug.Log("file is "+file+" "+file.FullName);
				//Debug.Log(file.Extension);
				if(file.Extension == ".prefab") {
					//Debug.Log("name "+file.Name);
					var fname = file.FullName;
					//var fname = Path.GetFileNameWithoutExtension(file.FullName);
					var npath = fname.Replace(resPath, "");
					npath = npath.Replace(".prefab", "");
					Debug.Log(npath);
					//var who = Path.GetFileName(Path.GetDirectoryName(npath));
					var que = Resources.Load<GameObject>(npath);
					allQuests.Add(que.GetComponent<QuestData>());
				}
			}
			Debug.Log ("fileNum " + fileInfo.Length);
#endif
		}

		void Awake() {
			questDataController = this;
			DontDestroyOnLoad (gameObject);
		}

		// Use this for initialization
		//Find Backpack object
		void Start () {
			backpack = GameObject.FindObjectOfType<BackPack> ();

		}

		/*
		 * New Get Item == Quest Require Item 
		 * 任务控制器 注册背包更新 事件
		 */
		void OnUpdateBackpack(ItemData id) {
			foreach (Quest q in currentQuests) {
				if(q.State == 1) {
					if(q.quest.AcquireItem == id.ItemName) {
						q.State = 2;//Quest Complete
						var questComplete = GameObject.FindObjectOfType<QuestComplete> ();
						questComplete.ShowQuestComplete(q.quest);
						break;
					}
				}
			}
		}

		// Update is called once per frame
		void Update () {
		
		}

		public JSONClass Serialize() {
			return null;
		}
		public void Deserialize(JSONClass js) {
		
		}

		public Quest GetMyCurrentQuest(string name) {
			foreach (Quest q in currentQuests) {
				if(q.Name == name)
					return q;
			}
			return null;
		}

		public void UpdateQuestState(QuestData qd, int Progress, QuestAI.QuestState state) {
			if (qd == null) {
				return;
			}
			bool find = false;
			foreach (Quest q in currentQuests) {
				if(q.quest == qd) {
					q.State = (int)state;
					find = true;
					break;
				}
			}

			if (!find) {
				var q = new Quest();
				q.Name = qd.UnitName;
				q.quest = qd;
				q.Progress = Progress;
				q.State = (int)state;
				currentQuests.Add(q);
			}
		}

		//关卡初始化的时候，初始化任务相关的物品
		public void InitQuestItem() {
			foreach (Quest q in currentQuests) {
				//Return State  
				if(q.State == 1) {
					/*
					var item = GameObject.Find("QuestItem");
					//
					var itemData = Resources.Load<GameObject>("units/items/quest_items/"+q.quest.AcquireItem).GetComponent<ItemData>();
					ItemDataRef.MakeDropItem(itemData, item.transform.position);
*/
				}
			}
		}

		public void CompleteQuest(QuestData qd) {
			int ind = 0;
			foreach (Quest q in currentQuests) {
				if(q.quest == qd) {
					currentQuests.RemoveAt(ind);
					break;
				}
				ind++;
			}
		}


		void UpdateMissionList() {
			
		}

		/*
		 * 获取任务列表
		 */ 
		public IEnumerator AskMissionList() {
			var packet = new KBEngine.PacketHolder ();
			var load = CGLoadTaskList.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, load, packet));
			if (packet.packet.responseFlag == 0) {
				var net = packet.packet.protoBody as GCLoadTaskList;
				taskList = new TaskList(net);
				UpdateMissionList();
			}
		}


		/*
		 * 接受任务  
		 * Update MissionList Message
		 */ 
		public IEnumerator MissionAccept(int taskId) {
			var packet = new KBEngine.PacketHolder ();
			var acc = CGAcceptTask.CreateBuilder ();
			acc.TaskId = taskId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, acc, packet));
			var ret = packet.packet.protoBody as GCAcceptTask;
			taskList.AddTask (ret);
		}

		/*
		 * 完成任务
		 */ 
		public IEnumerator MissionSubmit(int taskId) {
			var packet = new KBEngine.PacketHolder ();
			var com = CGCompleteTask.CreateBuilder ();
			com.PlayerTaskId = taskId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, com, packet));
			var ret = packet.packet.protoBody as GCCompleteTask;
			taskList.CompleteTask (ret);
		}

		/*
		 * 采集物品  CertainNpc On Map
		 */
		public IEnumerator CollectItems(int npcId) {
			var packet = new KBEngine.PacketHolder ();
			var col = CGCollectItems.CreateBuilder ();
			col.NpcId = npcId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, col, packet));
			var ret = packet.packet.protoBody as GCCollectItems;
			BackPack.backpack.PutInBackpack (ret);
		}

		/*
		 * 放弃任务
		 */ 
		public IEnumerator MissionAbandon(int taskId) {
			var packet = new KBEngine.PacketHolder ();
			var ab = CGCancelTask.CreateBuilder ();
			ab.UserTaskId = taskId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, ab, packet));
			taskList.Abandon (packet.packet.protoBody as GCCancelTask);
		}
		 
	}

}