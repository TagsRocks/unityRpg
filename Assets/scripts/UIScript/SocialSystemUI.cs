using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ChuMeng;

/*--------------------------------*/
//  作者 ： QiuChell
//  时间 ： 2015年01月27日
//  说明 ： 好友系统
/*--------------------------------*/

public class SocialSystemUI : IUserInterface {
	
	/*---------------------------------------*/
	//社交标签页
	public UIToggle Friend;                         //好友                     
	public UIToggle Enemy;                          //仇人
	public UIToggle BlackList;                      //黑名单
	public UIToggle RecommendFriend;      			//推荐好友
	public UIToggle Message;                     	//验证信息
	public UIToggle AddFriend;                     	//添加好友
	/*--------------------------------------*/
	
	public GameObject FriendScrollView;              //好友视图
	public GameObject EnemyScrollView;               //仇人视图
	public GameObject BlackListScrollView;           //黑名单视图
	public GameObject RecommendScrollView;      	 //推荐好友视图
	public GameObject MessageView;                   //(申请列表)消息视图
	public GameObject AddFriendView;                   //添加好友视图
	
	public GameObject FriendCheckTips;
	public GameObject EnemyCheckTips;
	public GameObject RecommendCheckTips;
	public GameObject BG;

	public GameObject friendItem;		
	public GameObject enemyItem;
	public GameObject messageItem;
	public GameObject searchResultItem;


	public UILabel SearchLabel;
	public UIButton SearchButton;

	private int clickType;   	 // 1 好友  2 仇人  3 黑名单    4  推荐好友
	private int dealApplyType;	 //
	private int clickItemID;
	private List<GameObject> itemList = new List<GameObject>();
	void Awake()
	{
		itemList.Clear ();
		UIEventListener.Get (BG).onClick = BgOnClcik;
		UIEventListener.Get (SearchButton.gameObject).onClick = searchClcik;
		clickType = 1;
		EventDelegate.Add (Friend.onChange, FriendOnChange);                		  	//好友标签响应
		EventDelegate.Add (Enemy.onChange, EnemyOnChange);                				//仇人标签响应
		EventDelegate.Add (BlackList.onChange, BlackListOnChange);         				//黑名单标签响应
		EventDelegate.Add (RecommendFriend.onChange, RecommendFriendOnChange);          //推荐好友标签响应
		EventDelegate.Add (Message.onChange, MessageOnChange);        					//消息标签响应
		EventDelegate.Add (AddFriend.onChange, AddFriendOnChange);        				//搜索标签响应

		SetCallback ("CloseButton", Hide);
		tipbtnclick ();
	}

	private void OnClose(GameObject g) {
		WindowMng.windowMng.PushView ("UI/shopping");
		MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.OpenMall);
	}

	//好友列表
	void initFriendListPannel()
	{
		List<FriendInfo> friendlistsinfo = FriendsController.friendsController.GetFriendsList ();
		
		int Length = friendlistsinfo.Count;
		//int Length = 8;
		Debug.Log(Length);
		for (int i = 0; i < Length; i++) {
			var obj = NGUITools.AddChild(Util.FindChildRecursive(FriendScrollView.transform, "Friend_Grid").gameObject,friendItem);
			obj.SetActive(true);
			obj.newName = i.ToString();
			Itemcontentset(friendlistsinfo[i],obj,false,false);
			itemList.Add(obj);
			UIEventListener.Get(obj.gameObject).onClick = clickListItem;
		}

		GridSet(Util.FindChildRecursive(FriendScrollView.transform, "Friend_Grid"));
	}

	//仇人列表
	void initEnemyListPannel()
	{
		List<FriendInfo> enemylistsinfo = FriendsController.friendsController.GetFriendsList ();
		
		int Length = enemylistsinfo.Count;
//		int Length = 15;
		Debug.Log("enemyList:"+Length);
		for (int i = 0; i < Length; i++) {
			var obj = NGUITools.AddChild(Util.FindChildRecursive(EnemyScrollView.transform, "Enemy_Grid").gameObject,enemyItem);
			obj.SetActive(true);
			UIDragScrollView sc = obj.GetComponent<UIDragScrollView>();
			sc.scrollView = Util.FindChildRecursive(EnemyScrollView.transform, "EnemyScrollView").GetComponent<UIScrollView>();
			obj.newName = i.ToString();
			//Itemcontentset(enemylistsinfo[i],obj,false,false);
			itemList.Add(obj);
			UIEventListener.Get(obj.gameObject).onClick = clickListItem;
		}
		GridSet(Util.FindChildRecursive(EnemyScrollView.transform, "Enemy_Grid"));
	}

	//黑名单列表
	void initBlackListPannel()
	{
		List<FriendInfo> blacklistsinfo = FriendsController.friendsController.GetFriendsList ();
		
		int Length = blacklistsinfo.Count;
//		int Length = 8;
		Debug.Log("Black:"+Length);
		for (int i = 0; i < Length; i++) {
			var obj = NGUITools.AddChild(Util.FindChildRecursive(BlackListScrollView.transform, "BlackList_Grid").gameObject,enemyItem);
			obj.SetActive(true);
			UIDragScrollView sc = obj.GetComponent<UIDragScrollView>();
			sc.scrollView = Util.FindChildRecursive(BlackListScrollView.transform, "BlackListScrollView").GetComponent<UIScrollView>();
			obj.newName = blacklistsinfo[i].PlayerId.ToString();
			Itemcontentset(blacklistsinfo[i],obj,false,false);
			itemList.Add(obj);
			UIEventListener.Get(obj.gameObject).onClick = clickListItem;
		}
		GridSet(Util.FindChildRecursive(BlackListScrollView.transform, "BlackList_Grid"));
	}

	//推荐好友列表
	void initRecommendListPannel()
	{
		List<FriendInfo> recommendlistsinfo = FriendsController.friendsController.GetFriendsList ();
		
		int Length = recommendlistsinfo.Count;
		//int Length = 12;
		Debug.Log("Recommend:"+Length);
		for (int i = 0; i < Length; i++) {
			var obj = NGUITools.AddChild(Util.FindChildRecursive(RecommendScrollView.transform, "RecommendFriend_Grid").gameObject,friendItem);
			obj.SetActive(true);
			UIDragScrollView sc = obj.GetComponent<UIDragScrollView>();
			sc.scrollView = Util.FindChildRecursive(RecommendScrollView.transform, "RecommendScrollView").GetComponent<UIScrollView>();
			obj.newName = i.ToString();
			//Itemcontentset(recommendlistsinfo[i],obj,false,false);
			itemList.Add(obj);
			UIEventListener.Get(obj.gameObject).onClick = clickListItem;
		}
		GridSet(Util.FindChildRecursive(RecommendScrollView.transform, "RecommendFriend_Grid"));
	}

	//申请列表
	void initMessageListPannel()
	{
		List<VerifyPlayer> messagelistsinfo = FriendsController.friendsController.GetApplyList ();
		
		int Length = messagelistsinfo.Count;
		//int Length = 10;
		Debug.Log("Message:"+Length);
		for (int i = 0; i < Length; i++) {
			var obj = NGUITools.AddChild(Util.FindChildRecursive(MessageView.transform, "Message_Grid").gameObject,messageItem);
			UIDragScrollView sc = obj.GetComponent<UIDragScrollView>();
			sc.scrollView = MessageView.GetComponent<UIScrollView>();
			obj.SetActive(true);
			obj.newName = messagelistsinfo[i].VerifyId.ToString();
			applyItemSet(messagelistsinfo[i],obj);
			itemList.Add(obj);
			UIEventListener.Get(obj.gameObject).onClick = clickListItem;
		}
		GridSet(Util.FindChildRecursive(MessageView.transform, "Message_Grid"));
	}

	//好友|推荐  isEnemy 仇人|黑名单   列表 信息设置 
	void Itemcontentset(FriendInfo iteminfo,GameObject obj,bool isEnemy,bool isSearch)
	{
		//是否在线
		if (iteminfo.Online)
		{

		}
		if (isEnemy) 
		{
			//
			//VIP 等级 对应的图标  iteminfo.VipType 
		}
		if (isSearch) 
		{
			
			//SetText (obj, "ItemMap", iteminfo.MapId)
			//所在地图 需要转换  iteminfo.MapId
		}
		SetText (obj, "ItemName", iteminfo.PlayerName);
		//职业 需要转换		iteminfo.Job
		//SetText (obj, "ItemOccupation", iteminfo.Job)
		SetText (obj, "ItemLevel", iteminfo.Level.ToString());
	}

	void applyItemSet(VerifyPlayer info,GameObject obj)
	{
		SetText (obj, "msgDate", Util.GetTimer (info.VerifyTime.ToString()));
		string contents = "";
		string leftbtntxt = "";
		string rightbtntxt = "";
		switch (info.VerifyType) 
		{
			case VerifyType.VERIFY_APPLY:
				dealApplyType = 0;
				contents = "申请添加您为好友";
				leftbtntxt = "同意";
				rightbtntxt = "拒绝";
				break;
			case VerifyType.VERIFY_AGREE:
				dealApplyType = 1;
				contents = "同意您的好友申请";
				leftbtntxt = "聊天";
				rightbtntxt = "删除";
			break;
			case VerifyType.VERIFY_REFUSE:
				dealApplyType = 2;
				contents = "拒绝您的好友申请";
				leftbtntxt = "再次申请";
				rightbtntxt = "删除";
				break;
		}
		SetText (obj, "leftLabel", leftbtntxt);
		SetText (obj, "rightLabel", rightbtntxt);
		UILabel[] labels = obj.GetComponentsInChildren<UILabel> (true);
		foreach (UILabel la in labels) {
			if(la.newName == "msgName")
			{
				la.text = info.RelatedPlayerName;
			}else if (la.newName == "msgLabel")
			{
				la.text = contents;
			}
		}
		UIButton[] buttons = obj.GetComponentsInChildren<UIButton> (true);
		foreach (UIButton button in buttons) {
			UIEventListener.Get (button.gameObject).onClick = applaybtnClick;
		}
	}

	//申请列表的按钮处理事件
	void applaybtnClick(GameObject btn)
	{
		Debug.Log (dealApplyType+"：btnname:"+btn.newName);
		if (btn.newName == "ChatButton") {
			applayLeftDeal();
		} 
		else if (btn.newName == "DeleteButton") 
		{
			applayRightDeal();
		}
	}

	private void applayLeftDeal()
	{
		switch (dealApplyType) 
		{
			case 0:
				agreeAddFriend();
				break;
			case 1:
				talkFriend();
				break;
			case 2:
				addFriend();
				break;
		}
	}

	private void applayRightDeal()
	{
		switch (dealApplyType) 
		{
		case 0:
			refuseAddFriend();
			break;
		case 1:
		case 2:
			delFriend();
			break;
		}
	}
	
	
	//列表  点击操作事件
	void clickListItem(GameObject btn)
	{
		clickItemID = int.Parse(btn.newName);
		commonTipPotionSet ();
	}


	public void commonTipPotionSet()
	{
		BG.SetActive (true);
		switch (clickType) 
		{
			case 1:
				FriendCheckTips.SetActive (true);
				if (Input.mousePosition.y > 320) {
					FriendCheckTips.transform.localPosition = new Vector3 (0, Input.mousePosition.y - 480, 0);
				} else {
					FriendCheckTips.transform.localPosition = new Vector3 (0, Input.mousePosition.y - 320, 0);
				}
				break;
			case 2:
			case 3:
				EnemyCheckTips.SetActive (true);
				EnemyCheckTips.transform.localPosition = new Vector3 (0, Input.mousePosition.y - 378, 0);
				break;
			case 4:
				RecommendCheckTips.SetActive (true);
				RecommendCheckTips.transform.localPosition = new Vector3 (0, Input.mousePosition.y - 328, 0);
				break;
			default:
				BG.SetActive (false);
				break;
		}
	}
	
	
	void BgOnClcik( GameObject bg )
	{
		FriendCheckTips.SetActive (false);
		EnemyCheckTips.SetActive (false);
		RecommendCheckTips.SetActive (false);
		
		BG.SetActive (false);
	}

	
	void searchClcik(GameObject bt)
	{
		StartCoroutine(setSearchShow ());
		searchResultItem.SetActive (true);
	}

	private IEnumerator setSearchShow()
	{
		yield return StartCoroutine (FriendsController.friendsController.SearchPlayer (SearchLabel.text));
		FriendInfo searchInfo = FriendsController.friendsController.searchInfo;
		SetText (searchResultItem, "ItemName", searchInfo.PlayerName);
		//SetText (searchResultItem, "ItemOccupation", searchInfo.Job);
		SetText (searchResultItem, "ItemLevel", searchInfo.Level.ToString());
		//SetText (searchResultItem, "VipLevel", searchInfo.VipType);
	}
	
	//好友标签响应
	private void FriendOnChange()
	{
		if ( Friend.value == true)
		{
			SetPanel ();
			clickType = 1;
			resetScrollViewPosition(Util.FindChildRecursive(FriendScrollView.transform, "FriendScrollView"),-199,-117);
			FriendScrollView.SetActive (true);
			tagFriend = FriendType.FRIENDLY_RELATION;
			updateFrame();
		}
	}
	
	//仇人标签响应
	private void EnemyOnChange()
	{
		if (Enemy.value == true) {
			SetPanel ();
			clickType = 2;
			resetScrollViewPosition(Util.FindChildRecursive(EnemyScrollView.transform, "EnemyScrollView"),-199,-117);
			EnemyScrollView.SetActive (true);
			tagFriend = FriendType.ENEMY_RELATION;
			updateFrame();
		}
	}
	
	//黑名单标签响应
	private void BlackListOnChange()
	{
		if (BlackList.value == true) {
			SetPanel ();
			clickType = 3;
			resetScrollViewPosition(Util.FindChildRecursive(BlackListScrollView.transform, "BlackListScrollView"),-199,-117);
			BlackListScrollView.SetActive (true);
			tagFriend = FriendType.BLACK_RELATION;
			updateFrame();
		}
		
	}
	
	//推荐好友标签响应
	private void RecommendFriendOnChange()
	{
		if (RecommendFriend.value == true) {
			SetPanel ();
			clickType = 4;
			resetScrollViewPosition(Util.FindChildRecursive(RecommendScrollView.transform, "RecommendScrollView"),-199,-117);
			RecommendScrollView.SetActive (true);
			// To do 
			//initRecommendListPannel();
		}
	}
	//验证信息 标签相应
	private void MessageOnChange()
	{
		if (Message.value == true) {
			SetPanel();
			clickType = 0;
			resetScrollViewPosition(MessageView.transform,-1,87);
			MessageView.SetActive(true);
			StartCoroutine(requestApplyLists());

		}
	}

	private IEnumerator requestApplyLists()
	{
		yield return StartCoroutine(FriendsController.friendsController.LoadVerifyPlayer());
		initMessageListPannel();
	}

	//添加好友 标签相应
	private void AddFriendOnChange()
	{
		if (AddFriend.value == true) {
			SetPanel();
			clickType = 5;
			AddFriendView.SetActive(true);
		}
	}

	private void updateFrame()
	{
		StartCoroutine(requestFriendLists());
	}

	private FriendType tagFriend;
	private IEnumerator requestFriendLists()
	{
		yield return StartCoroutine(FriendsController.friendsController.LoadList(tagFriend));

		switch (tagFriend) {
			case FriendType.FRIENDLY_RELATION:
				initFriendListPannel();
				break;
			case FriendType.ENEMY_RELATION:
				initEnemyListPannel();
				break;
			case FriendType.BLACK_RELATION:
				initBlackListPannel();
				break;
			case FriendType.NONE_RELATION:
				break;
		}
	}

	private void clearList()
	{
		foreach (GameObject g in itemList) 
		{
			Destroy(g);
		}
		itemList.Clear ();
	}
	
	private void SetPanel()
	{
		clearList ();
		FriendScrollView.SetActive (false);
		EnemyScrollView.SetActive (false);
		BlackListScrollView.SetActive (false);
		RecommendScrollView.SetActive (false);
		MessageView.SetActive (false);
		AddFriendView.SetActive (false);
		searchResultItem.SetActive (false);
	}
	/*
	private void resetScrollViewPosition(Transform tra,int _x,int _y)
	{
		tra.transform.localPosition = new Vector3 (_x, _y, 0);
		UIPanel p = tra.GetComponent<UIPanel> ();
		p.clipOffset = new Vector2 (0, 0);
	}*/

	private void tipbtnclick()
	{
		common(FriendCheckTips);
		common(EnemyCheckTips);
		common(RecommendCheckTips);
	}

	private void common(GameObject obj)
	{
		UIButton[] buttons = obj.GetComponentsInChildren<UIButton> (true);
		foreach (UIButton button in buttons) {
			UIEventListener.Get (button.gameObject).onClick = tipbtnClick;
		}
	}

	void tipbtnClick(GameObject btn)
	{
		Debug.Log ("tipbtnClick:"+btn.newName);
		switch (btn.newName) 
		{
			case "Look":
				lookFriend();
				break;
			case "lookFriend":
				talkFriend();
				break;
			case "Home":
				gotoHome();
				break;
			case "Give":
				givePhysical();
				break;
			case "Compare ":
				fightFriend();
				break;
			case "BlackList":
				blackFriend();
				break;
			case "AddFriend":
				addFriend();
				break;
			case "Delete":
				//删除分为  好友列表删除、仇人离列表删除、黑名单列表删除、申请列表删除
				Debug.Log("clicktype:"+clickType);
				delFriend();
				break;
		}
	}
	//查看玩家信息 接口
	private void lookFriend()
	{

	}
	//私聊玩家 接口
	private void talkFriend()
	{
		
	}
	//访问家园  接口
	private void gotoHome()
	{

	}
	//赠送体力  接口
	private void givePhysical()
	{
		FriendsController.friendsController.StartCoroutine(FriendsController.friendsController.GivePhysical(clickItemID));
	}
	//切磋  接口
	private void fightFriend()
	{
		
	}
	//拉黑  接口
	private void blackFriend()
	{
		FriendsController.friendsController.StartCoroutine(FriendsController.friendsController.AddBlack(clickItemID));
	}
	//添加好友  接口
	private void addFriend()
	{
		FriendsController.friendsController.StartCoroutine(FriendsController.friendsController.InviteFriend(clickItemID));
	}
	//删除 接口
	private void delFriend()
	{
		//普通删除（好友列表中删除）
		FriendsController.friendsController.StartCoroutine(FriendsController.friendsController.DelFriend(clickItemID));

		//黑名单删除（黑名单列表中删除）
		//FriendsController.friendsController.StartCoroutine(FriendsController.friendsController.DelBlack(clickItemID));

		//仇人删除（仇人列表中删除）
		//FriendsController.friendsController.StartCoroutine(FriendsController.friendsController.DelEnemy(clickItemID));

	}

	//同意添加好友  接口
	private void agreeAddFriend()
	{

	}
	//拒绝添加好友  接口
	private void refuseAddFriend()
	{

	}
}
