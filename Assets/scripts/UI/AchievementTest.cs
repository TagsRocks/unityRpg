using UnityEngine;
using System.Collections;
using ChuMeng;

public class AchievementTest : MonoBehaviour {

	public GameObject achiPanel;

	void Start()
	{
		StartCoroutine (initPage());
	}

	IEnumerator initPage()
	{
		//if (SaveGame.saveGame == null) {
		Log.Net ("Init savegame ");
			var g = new GameObject();
			var s = g.AddComponent<SaveGame>();
			s.InitData();
		//}

		yield return new WaitForSeconds (1);
		Log.Net ("等待数据初始化结束 才显示 UI");
		yield return StartCoroutine(AchievementController.achievementController.LoadAchievenment());

		/*
		GameObject page = Instantiate (achiPanel) as GameObject;
		page.transform.parent = this.transform;
		page.transform.localScale = Vector3.one;
		*/
		Log.Net ("Init window now");
		yield return new WaitForSeconds (1);
		WindowMng.windowMng.PushView ("UI/achievement");
		MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.OpenAchievement);
		Log.Net ("Init over");

	}

}
