/*
 * Author : Wangjunbo
 * Email : 1305201219@qq.com
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StringInject;
using System.Text.RegularExpressions;
using System;

namespace ChuMeng
{
	//任务数据池
	public class TaskController : MonoBehaviour
	{
		public static TaskController taskController;

		public List<TaskItem> taskItem = new List<TaskItem>();
		//初始化函数
		void Awake(){
			taskController = this;
			DontDestroyOnLoad (gameObject);
		}
	
		public IEnumerator LoadTask(){
			var packet = new KBEngine.PacketHolder ();
			var list = CGLoadTaskList.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, list, packet));
			var data = packet.packet.protoBody as GCLoadTaskList;
			foreach (PlayerTask task in data.PlayerTaskList) {
				var n = new TaskItem(task.TaskId);	
				taskItem.Add(n);
				Log.Net("type" + n.Type);
			}
			Log.Net ("taskItem data Number" + taskItem.Count);
		}

		//获取任务链表
		public List<TaskItem> GetTask(int Type){
			var task = new List<TaskItem> ();
			foreach (TaskItem ts in taskItem) {
				if( ts.Type == Type ){
					task.Add(ts);
				}
			}
			return task;
		}

		//获取任务目标
		public string GetTaskTarget(string target){
			//任务类型
			string[] Cond = target.Split (new[]{"_"}, StringSplitOptions.None);
				int[] tar = new int[Cond.Length];
				int n = 0;
				foreach (string s in Cond) {
					tar[n] = System.Convert.ToInt32(s);
					n++;
				}
				
				//根据任务类型获取不同的文本
				string str = "";
				string text = "";
				var ht = new Hashtable ();
				switch (tar [0]) {
				case 1:
					str = "";
					break;
				case 2:
					str = "";
					break;
				case 3:
					str = "";
					break;
				case 4:
					str = "";
					break;
				case 5:
					str = "等级达到{num}级。";
					ht.Clear();
					ht.Add("num", tar[1]);
					text = str.Inject(ht);
					break;
				case 6:
					str = "通关{fubenid}。";
					ht.Clear();
					var test = GMDataBaseSystem.SearchIdStatic<DungeonConfigData>(GameData.DungeonConfig, tar[1]).name;
					ht.Add("fubenid", test);
					text = str.Inject(ht);
					break;
				}
				return text;			
		}

		//获取任务奖励
		public List<AwardItem> GetTaskAward(string award){
			List<AwardItem> alistItem = new List<AwardItem> ();
			//aunm数组保存的为奖励数量
			string[] anum = award.Split (new[] {"|"}, StringSplitOptions.None);
				
			for (int i = 0; i < anum.Length; i++) {
				Log.Net(anum[i]);
				AwardItem aItem = new AwardItem();
				//str数组保存的为每个奖励物品信息
				string[] str = anum[i].Split(new[] {"_"}, StringSplitOptions.None);
				//字符串转换为int类型
				int[] istr = new int[str.Length];
				int j = 0;
				foreach(string s in str){
					istr[j] = System.Convert.ToInt32(s);
					j++;
				}

				//根据第一个字符判断奖励类型
				switch(istr[0]){
				case 1:  //道具
					aItem.Icon = Util.GetItemData(0, istr[3]).IconName;
					aItem.Sheet = Util.GetItemData(0, istr[3]).IconSheet;
					aItem.Number = istr[1];
					break;
				case 2:  //装备
					aItem.Icon = Util.GetItemData(1, istr[3]).IconName;
					aItem.Sheet = Util.GetItemData(1, istr[3]).IconSheet;
					aItem.Number = istr[1];
					break;
				case 3:   //经验
				case 4:   //非绑银
				case 5:   //绑银
				case 6:   //绑金
				case 7:   //绑定金票
				case 8:   //声望币
					aItem.Icon = Util.GetItemData(0, istr[0]-2).IconName;
					aItem.Sheet = Util.GetItemData(0, istr[0]-2).IconSheet;
					aItem.Number = istr[1];
					break;
				}
				alistItem.Add(aItem);
			}
			return alistItem;	
		}
	}

	//任务类
	public class TaskItem{
		TaskConfigData taskData;
		List<AwardItem> awardData;
		string condition;
		//任务类型
		public int Type{
			get{
				return taskData.type;
			}
		}

		//任务名称
		public string Name{
			get{
				return taskData.name;
			}
		}

		//任务目标
		public string Condition{
			get{
				return condition;
			}
		}

		//任务详情
		public string Describe{
			get{
				return taskData.description;
			}
		}

		public string Award{
			get{
				return taskData.rewards;
			}
		}

		//奖励物品
		public List<AwardItem> AwardList{
			get{
				return awardData;
			}
		}

		public TaskItem(int Id){
			taskData = GMDataBaseSystem.SearchIdStatic<TaskConfigData> (GameData.TaskConfig, Id);
			condition = TaskController.taskController.GetTaskTarget (taskData.condition);
			awardData = TaskController.taskController.GetTaskAward (taskData.rewards);
		}
	}

	//物品奖励
	public class AwardItem{
		string icon;
		int sheet;
		int number;
		
		public int Number{
			set{
				number = value;
			}
			get{
				return number;
			}
		}

		public int Sheet{
			set{
				sheet = value;
			}
			get{
				return sheet;
			}
		}
		
		public string Icon{
			set{
				icon = value;
			}
			get{
				return icon;
			}
		}
	}
}