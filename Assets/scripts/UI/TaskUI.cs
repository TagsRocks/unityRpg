/*
 * Author :  Wangjunbo
 * Email :  1305201219@qq.com
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace ChuMeng
{
	
	public class TaskUI : IUserInterface
	{

		int type = 0;                              //任务类型
		string typeName = "【主线】";   //标题名称类型

		public GameObject taskMesh;              //任务标题框
		public GameObject awardMesh;          //任务奖励

		public List<GameObject> item = null;    //任务标题链表
		public List<GameObject> aItem = null;  //任务奖励链表

		public List<TaskItem> taskItem = null;  //任务数据链表
		public List<AwardItem> awardItem = null;//任务奖励数据

		public UILabel target;           //任务目标
		public UILabel particulars;    //任务详情
		public UIScrollView view;      //任务列表视图

		public delegate void BoolIdDelegate(bool b, int id) ;

		//给每个任务设置一个响应事件
		public void CheckBox(string name, int id, BoolIdDelegate cb) {
			var tog = GetName(name).GetComponent<UIToggle>();
			
			EventDelegate.Set(tog.onChange, delegate{
				cb(tog.value, id);
			});
		}


		void Awake(){
			regEvt = new System.Collections.Generic.List<MyEvent.EventType> (){
				MyEvent.EventType.UpdateTask,
			};
			RegEvent ();

			//任务标题
			item = new List<GameObject> ();
			taskMesh = GetName ("TaskMesh");
			taskMesh.newName = "0";
			item.Add (taskMesh);

			//任务奖励
			aItem = new List<GameObject> ();
			awardMesh = GetName ("AwardMesh");
			awardMesh.newName = "0";
			aItem.Add (awardMesh);

			SetCallback ("close", Hide);
			SetCheckBox("mainToggle", OnMain);
			SetCheckBox("branchToggle", OnBranch);
			SetCheckBox("everydayToggle", OnDay);
			SetCheckBox ("activityToggle", OnActivity);

			target = GetLabel ("targetText");
			Debug.Log (target);

			particulars = GetLabel("particularsText");
			Debug.Log (particulars);

			view = Util.FindChildRecursive (transform, "Scroll View").GetComponent<UIScrollView> ();
			Debug.Log (view);
		}

		void OnMain(bool b){
			if (b) {
				type = 1;
				typeName = "【主线】";
				UpdateFrame();	
			}
		}

		void OnBranch(bool b){
			if (b) {
				type = 2;
				typeName = "【支线】";
				UpdateFrame();
			}
		}

		void OnDay(bool b){
			if (b) {
				type = 3;
				typeName = "【日常】";
				UpdateFrame();
			}
		} 

		void OnActivity(bool b){
			if (b) {
				type = 4;
				typeName = "【活动】";
				UpdateFrame();
			}
		}

		protected override void OnEvent(MyEvent evt){
			UpdateFrame ();		
		}

		//任务标题响应事件
		void OnTask(bool b,int id){
			if (b) {
				target.text = taskItem[id].Condition;
				particulars.text= taskItem[id].Describe;

				//添加任务奖励
				awardItem = taskItem[id].AwardList;
				Debug.Log("奖励数量"+awardItem.Count);
				int cou = aItem.Count;
				while (aItem.Count < awardItem.Count) {
					var aitems = NGUITools.AddChild(awardMesh.transform.parent.gameObject, awardMesh);
					aitems.newName = cou.ToString();
					cou++;
					aItem.Add(aitems);
				}	

				for (int i = 0; i < awardItem.Count; i++) {
					aItem[i].SetActive(true);
					var icon = Util.FindChildRecursive(aItem[i].transform, "icon").GetComponent<UISprite>();
					Util.SetIcon(icon, awardItem[i].Sheet, awardItem[i].Icon);
				}

				for(int i = awardItem.Count; i < aItem.Count; i++){
					aItem[i].SetActive(false);
				}
				awardMesh.transform.parent.gameObject.GetComponent<UIGrid>().Reposition();
			}
		}

		//更新函数
		void UpdateFrame(){
			taskItem = GameInterface_Task.TaskInterface.GetTaskList (type);
			if (taskItem.Count != 0) {
				//添加任务标题
				int c = item.Count;
				while (item.Count < taskItem.Count) {
					var items = NGUITools.AddChild(taskMesh.transform.parent.gameObject, taskMesh);
					items.newName = c.ToString();
					item.Add(items);
					c++;
				}

				for (int i =0; i < taskItem.Count; i++) {
					item[i].SetActive(true);
					item[i].GetComponent<UIToggle>().value = false;
					var id = System.Convert.ToInt32(item[i].newName);
					CheckBox(item[i].newName, id, OnTask);
					//添加任务标题内容
					var name = Util.FindChildRecursive(item[i].transform, "name").GetComponent<UILabel>();
					name.text = typeName + taskItem[i].Name;
				}
				item[0].GetComponent<UIToggle>().value = true;
				OnTask(true, 0);

				for (int i = taskItem.Count; i < item.Count; i++) {
					item[i].SetActive(false);
				}
				
				taskMesh.transform.parent.gameObject.GetComponent<UIGrid> ().Reposition ();
				view.ResetPosition();
			}
			else
			{
				target.text = "";
				particulars.text = "";

				for(int i = 0; i < item.Count; i++){
					item[i].SetActive(false);
				}
				for(int i = 0; i < aItem.Count; i++){
					aItem[i].SetActive (false);
				}
			}
		
		}




	}
}