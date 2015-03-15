using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ChuMeng;

public class AuctionUI : IUserInterface {


	public UIToggle Auctionner;
	public UIToggle MyAuction;

	public GameObject AuctionnerView;
	public GameObject MyAuctionView;

	public GameObject AuctionnerMeshView;
	public GameObject MyAuctionMeshView;

	public GameObject Table;
	public GameObject GridParent;
	public UILabel MoneyCount;
	public UILabel PageCount;
	public UILabel MinLabel;
	public UILabel MaxLabel;
	public UILabel SeachLabel;

	public UIButton[] Btns;

	public GameObject ItemTipsView;
	private List<GameObject> itemList = new List<GameObject>();
	private GameInterface_Auction auctionFace;
	private int currentPage = 1;
	private int pageShowNum = 4;
	private int pages = 1;
	private int tipBuyNumber = 1;
	private int tipBuyPrice = 0;
	private int tipBuyId = 0;
	private int itemId;
	private AuctionController controller;
	void Awake()
	{
		controller = AuctionController.auctionController;
		itemList.Clear ();
		SetCallback ("CloseButton", Hide);
		auctionFace = GameInterface_Auction.GetInstance();
		EventDelegate.Add (Auctionner.onChange, AucitonnerOnClick);
		EventDelegate.Add (MyAuction.onChange, MyAuctionOnClick);
		leftbtnInit ();
		auctionInit (controller.GetItemList(),controller.auctionInfo.MaxSize,controller.auctionInfo.PageSize%pageShowNum+1);
		btnInit ();
		tipsBtnInit ();
	}

	private void btnInit()
	{
		foreach (UIButton button in Btns) {
			UIEventListener.Get (button.gameObject).onClick = btnClick;
		}
	}

	private void tipsBtnInit()
	{
		UIButton[] buttons = ItemTipsView.GetComponentsInChildren<UIButton> (true);
		foreach (UIButton button in buttons) {
			UIEventListener.Get (button.gameObject).onClick = btnClick;
		}
	}

	private void btnClick(GameObject btn)
	{
		Debug.Log ("btnClick:"+btn.name);
		switch (btn.name) 
		{
			case "CloseBtn":
				break;
			case "LeftPage":
				if(currentPage>1)
					currentPage--;
				break;
			case "RightPage":
				if(currentPage<pages)
					currentPage++;
				break;
			case "SeachBtn":
				seachRequest();
				break;
			case "add":
				if(tipBuyNumber<10)
				{
					tipBuyNumber++;
					tippannelBuySet();
				}
				break;
			case "delete":
				if(tipBuyNumber>1)
				{
					tipBuyNumber--;
					tippannelBuySet();
				}
				break;
			case "buy":
				controller.AuctionBuyGoods(tipBuyId,tipBuyNumber);
				break;
			case "Tipsbg":
				ItemTipsView.SetActive(false);
				break;
		}
	}

	//物品搜索请求
	private int lv1;
	private int lv2;
	private void seachRequest()
	{
		if(SeachLabel.text == "")
		{
			tipshow("请输入需要搜索的拍卖品名称");
			return;
		}
		if (MinLabel.text == "") 
		{
			tipshow("请输入需要搜索的拍卖品等级范围");
			return;
		}
		lv1 = System.Int32.Parse(MinLabel.text);
		if (MaxLabel.text == "") 
		{
			tipshow("请输入需要搜索的拍卖品等级范围");
			return;
		}
		lv2 = System.Int32.Parse(MaxLabel.text);
		if(lv1 >= lv2||lv1<0||lv2<0)
		{
			tipshow("输入等级范围有误！");
			return;
		}
		StartCoroutine(requestForSeach());
	}

	private IEnumerator requestForPages()
	{
		yield return StartCoroutine(controller.SearchAuctionInfo (currentPage, 0, "", 0, 0, 0, 0));
		auctionInit (controller.GetSeachItemList(),1,1);
	}

	private IEnumerator requestForSeach()
	{
		yield return StartCoroutine(controller.SearchAuctionInfo (0, 0, SeachLabel.text, lv1, lv2, 0, 0));
		auctionInit (controller.GetSeachItemList(),1,1);
	}


	private void tipshow(string str)
	{
		var evt = new MyEvent(MyEvent.EventType.DebugMessage);
		evt.strArg = str;
		MyEventSystem.myEventSystem.PushEvent(evt);
	}

	//左边列表数据读取数据表显示
	private void leftbtnInit()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < auctionFace.tableList.Count; i++) {
			var obj = NGUITools.AddChild(Table,Util.FindChildRecursive(Table.transform, "Quest1").gameObject);
			SetText (obj, "Name", auctionFace.tableList[i].tableName);
			obj.name = auctionFace.tableList[i].tableNode.ToString();
			obj.SetActive(true);
			list.Add(obj);
		}

		for (int j = 0; j< list.Count; j++) 
		{
			tweenGrid(list[j],auctionFace.AllList[j].arr);
		}

		UIButton[] buttons = Table.GetComponentsInChildren<UIButton> (true);
		foreach (UIButton button in buttons) {
			UIEventListener.Get (button.gameObject).onClick = tableItemClick;
		}
		//统一读取 游戏币接口
		MoneyCount.text = "1000";
	}

	//分类列表请求
	private void tableItemClick(GameObject btn)
	{
		Debug.Log ("....click ...."+btn.name);
		itemId = int.Parse (btn.name);
		StartCoroutine (itemRequest ());
	}

	private IEnumerator itemRequest()
	{
		yield return StartCoroutine(controller.SearchAuctionInfo (0, 0, "", 0, 0, itemId, 0));
		auctionInit (controller.GetSeachItemList(),1,1);
	}


	private void tweenGrid(GameObject obj,List<AuctionTable> gritItem)
	{
		GameObject gridObj = Util.FindChildRecursive(obj.transform, "Grid").gameObject;
		for (int i = 0; i < gritItem.Count; i++) {
			var ob = NGUITools.AddChild(gridObj,Util.FindChildRecursive(gridObj.transform, "Toggle1").gameObject);
			ob.name = gritItem[i].tableNode.ToString();
			SetText (ob, "Label", gritItem[i].tableName);
			ob.SetActive(true);
		}
		
		GridSet (Util.FindChildRecursive(gridObj.transform, "Grid"));
	}

	//数组清空
	private void clearList()
	{
		foreach (GameObject g in itemList) 
		{
			Destroy(g);
		}
		itemList.Clear ();
	}

	//拍卖场
	private void AucitonnerOnClick()
	{
		if ( Auctionner.value == true ) {
			MyAuctionView.SetActive(false);
			//MyAuctionMeshView.SetActive(false);
			auctionInit (controller.GetItemList(),controller.auctionInfo.MaxSize,controller.auctionInfo.PageSize/pageShowNum);
			AuctionnerView.SetActive(true);
			//AuctionnerMeshView.SetActive(true);
		}
	}
	//点击相应分类 请求的物品显示
	private void requestAuctionInfo()
	{

	}

	//初始面板时 显示的拍卖物品
	private List<AuctionItem> setItemInfo;
	private void auctionInit(List<AuctionItem> itemInfo,int _currentPage,int _pages)
	{
		clearList ();
		setItemInfo = itemInfo;
		int length = itemInfo.Count;
		GameObject gridObj = Util.FindChildRecursive(AuctionnerMeshView.transform, "Grid").gameObject;
		for (int i = 0; i < length; i++) {
			var ob = NGUITools.AddChild(gridObj,Util.FindChildRecursive(AuctionnerMeshView.transform, "AucitonMesh").gameObject);
			ob.name = itemInfo[i].Id.ToString();
			//物品图片 等级根据id读取  配置表的icon  todo
			commondSetForIcon (ob,itemInfo[i].BaseId);
			//SetText (ob, "Level", ""+i);
			SetText (ob, "Time", itemInfo[i].RemainTime.ToString());
			SetText (ob, "Count", itemInfo[i].TotalCost.ToString());
			ob.SetActive(true);
			UIEventListener.Get (ob.gameObject).onClick = auctionItemClick;
			itemList.Add(ob);
		}

		GridSet (Util.FindChildRecursive(gridObj.transform, "Grid"));
		currentPage = _currentPage;
		pages = _pages;
		PageCount.text = currentPage + "/" + pages;

	}

	private void auctionItemClick(GameObject bt)
	{
		Debug.Log ("bt:"+bt.name);

		tipBuyId = int.Parse (bt.name);
		AuctionItem info = setItemInfo [auctionFace.GetItemIndex(setItemInfo,int.Parse(bt.name))];

		//ItemData data = new ItemData (1, info.BaseId);



		commondSetForIcon (ItemTipsView,info.BaseId);

		//var icon = Util.FindChildRecursive(ItemTipsView.transform, "iconsprite");
		//Util.SetIcon (icon.GetComponent<UISprite>(),data.IconSheet,data.IconName);



		var onePrice = Util.FindChildRecursive(ItemTipsView.transform, "onePriceLabel").GetComponent<UILabel>();
		onePrice.text = info.TotalCost.ToString();
		tipBuyPrice = info.TotalCost;
		tippannelBuySet();


		ItemTipsView.SetActive(true);
	}

	private void commondSetForIcon(GameObject _partent,int _baseId)
	{
		ItemData data = new ItemData (1, _baseId);
		var icon = Util.FindChildRecursive(_partent.transform, "iconsprite");
		Util.SetIcon (icon.GetComponent<UISprite>(),data.IconSheet,data.IconName);

		var level = Util.FindChildRecursive(_partent.transform, "levelLabel").GetComponent<UILabel>();
		level.text = data.Level.ToString();

		var name = Util.FindChildRecursive(_partent.transform, "name").GetComponent<UILabel>();
		name.text = data.ItemName;
	}


	private void tippannelBuySet()
	{
		var price = Util.FindChildRecursive(ItemTipsView.transform, "totalPricesLabel").GetComponent<UILabel>();
		price.text = tipBuyNumber*tipBuyPrice + "";
		
		var buyNum = Util.FindChildRecursive(ItemTipsView.transform, "numberLabel").GetComponent<UILabel>();
		buyNum.text = tipBuyNumber.ToString();
	}


	//我的拍卖
	private void MyAuctionOnClick()
	{
		if ( MyAuction.value == true ) {
			AuctionnerView.SetActive(false);
			//AuctionnerMeshView.SetActive(false);
			StartCoroutine(myAuctionInitSet());
			MyAuctionView.SetActive(true);
			//MyAuctionMeshView.SetActive(true);
		}
	}

	private IEnumerator myAuctionInitSet()
	{
		yield return StartCoroutine(controller.LoadSellGoodsInfo());
		auctionInit (controller.GetMyItemList(),1,1);
	}

}
