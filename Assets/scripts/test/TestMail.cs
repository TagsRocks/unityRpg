using UnityEngine;
using System.Collections;
using ChuMeng;

public class TestMail : MonoBehaviour {

	public GameObject mailpannel;
	void Start()
	{

		StartCoroutine(initPage ());
	}
	private IEnumerator initPage()
	{
		if (SaveGame.saveGame == null) {
			var g = new GameObject();
			var s = g.AddComponent<SaveGame>();
			s.InitData();
		}
		Log.Net ("等待数据初始化结束 才显示 UI");
		yield return StartCoroutine(MailController.mailController.LoadMail ());

		//yield return new WaitForSeconds (5);


		GameObject page = Instantiate (mailpannel) as GameObject;
		page.transform.parent = this.transform;
		page.transform.localScale = Vector3.one;
	}
}
