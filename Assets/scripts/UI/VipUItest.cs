using UnityEngine;
using System.Collections;
using ChuMeng;

public class VipUItest : MonoBehaviour {

	public GameObject vipPanel;

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
		yield return StartCoroutine(VipController.vipController.LoadVip());

		Log.Net ("初始化vip成功 显示 windows Init window now");
		yield return new WaitForSeconds (1);
		WindowMng.windowMng.PushView("UI/Vip");
		MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.openVip);
		Log.Net ("Init over");
	}
}
