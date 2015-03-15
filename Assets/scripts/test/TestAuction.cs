using UnityEngine;
using System.Collections;
using ChuMeng;

public class TestAuction : MonoBehaviour {

	public GameObject pannel;
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
		
		yield return new WaitForSeconds (1);
		Log.Net ("等待数据初始化结束 才显示 UI:"+AuctionController.auctionController);
		yield return StartCoroutine(AuctionController.auctionController.LoadAuctionInfo(1,5));



		Log.Net ("load data over");
		WindowMng.windowMng.PushView ("UI/AuctionUI");
	}
}
