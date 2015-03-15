using UnityEngine;
using System.Collections;
using ChuMeng;

public class TaskTest : MonoBehaviour {
	
	public GameObject taskPanel;
	
	void Start()
	{
		StartCoroutine (initPage());
	}
	
	IEnumerator initPage(){
		Log.Net ("Init savagame");
		var g = new GameObject ();
		var s = g.AddComponent<SaveGame> ();
		s.InitData ();
		
		yield return new WaitForSeconds(1);
		Log.Net ("等待数据初始化结束 才显示UI");
		yield return StartCoroutine(TaskController.taskController.LoadTask());
		
		Log.Net ("初始化vip成功 显示 windows Init window now");
		yield return new WaitForSeconds (1);
		WindowMng.windowMng.PushView("UI/Task");
		MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.openTask);
		Log.Net ("Init over");
	}
}
