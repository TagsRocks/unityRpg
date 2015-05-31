using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ChuMeng;

/*--------------------------------*/
//  作者 ： QiuChell
//  时间 ： 2015年02月02日
//  说明 ： 骑士团系统
/*--------------------------------*/

public class KnightageUI : IUserInterface {
	
	public UIToggle Message;
	public UIToggle Member;
	public UIToggle SocietyList;
	public UIToggle Verification;

	public UIToggle Apply;
	public UIToggle GvG;
	
	public GameObject ApplyView;
	public GameObject GvGView;
	
	public GameObject MessageView;   		//公告
	public GameObject MemberView;			//成员
	public GameObject SocietyListView;		//公会列表
	public GameObject VerificationView;		//验证信息

	public GameObject ButtomBtn;

	public GameObject NoticeTips;			//更改公告的提示面板
	public UILabel ChangeNoticeText;
	public UILabel NoticeText;
	public UIButton ManorButton;			//进入领地
	public UIButton ExitButton;				//退出
	/*
	 * 解散以及更改  只有职位为团长才显示
	 * */
	public UIButton DissolveButton;			//解散
	public UIButton AlterButton;			//更改


	public UIToggle info;
	public UIToggle build;
	public UIToggle salary;

	public GameObject rootview;
	public GameObject infoview;
	public GameObject buildview;
	public GameObject salaryview;


	private GCLoadGuildInfo guildInfo;
	private bool isMaster = false;//是否有团长权限   只有团长的权限 显示内容以及操作 会不同
	private List<GameObject> itemList = new List<GameObject>();
	private GuildConfigInfo config;
	private GameInterface_Knightage interFace;
	void Awake()
	{
		itemList.Clear ();
		interFace = GameInterface_Knightage.GetInstance();
		guildInfo = GuildController.guildController.guildInfo;
		if (guildInfo.Duty == 4) 
		{
			isMaster = true;
		}
		
		GameData d = new GameData ();

		Verification.gameObject.SetActive(isMaster);
		DissolveButton.gameObject.SetActive (isMaster);
		AlterButton.gameObject.SetActive (isMaster);
		initClick ();
	}

	private void initClick()
	{
		SetCallback ("CloseButton", Hide);
		EventDelegate.Add (Message.onChange, MessageOnClick);
		EventDelegate.Add (Member.onChange, MemberOnClick);
		EventDelegate.Add (SocietyList.onChange, SocietyListOnClick);
		EventDelegate.Add (Verification.onChange, VerificationOnClick);
		EventDelegate.Add (Apply.onChange, ApplyOnClick);
		EventDelegate.Add (GvG.onChange, GvGOnClick);
		EventDelegate.Add (info.onChange, infoOnClick);
		EventDelegate.Add (build.onChange, buildOnClick);
		EventDelegate.Add (salary.onChange, salaryOnClick);
		UIEventListener.Get (ManorButton.gameObject).onClick = buttombtnClick;
		UIEventListener.Get (DissolveButton.gameObject).onClick = buttombtnClick;
		UIEventListener.Get (AlterButton.gameObject).onClick = buttombtnClick;
		UIEventListener.Get (ExitButton.gameObject).onClick = buttombtnClick;
		bottombtnEvent ();
	}



	private void MessageOnClick()
	{
		if (Message.value == true) {
			ViewOnClick();
			messageInitSet();
			//infoViewInit();
			StartCoroutine(salaryViewSet());
			MessageView.SetActive(true);
		}
	}

	//信息版面
	private void messageInitSet()
	{
		//公会总经验、维护消耗的木材  读取配置表  
		
		SetText (rootview, "noticeContents", guildInfo.GuildManifesto);
		config = new GuildConfigInfo (Convert.ToInt32(guildInfo.GuildLevel));
		SetText (rootview, "coinnumber", guildInfo.SilverTicket.ToString());
		SetText (rootview, "treenumber", guildInfo.Wood.ToString());
		SetText (rootview, "UpgradeResourseText", config.MaintainExpend.ToString());

		UISlider slider = Util.FindChildRecursive (rootview.transform, "ProgressBar").GetComponent<UISlider> ();
		slider.value = (float)guildInfo.GuildExperience / (float)config.MaintainExpend;
	}

	private void infoViewSet()
	{
		SetText (infoview, "KnightName", guildInfo.GuildName.ToString());
		SetText (infoview, "KnightLevel", guildInfo.GuildLevel.ToString()+"级");
		SetText (infoview, "KnightMember", guildInfo.MemberCount.ToString());
		SetText (infoview, "KnightContribution", guildInfo.CurrnetDonate.ToString());
		SetText (infoview, "KnightPosition", GetDutyName(guildInfo.Duty));
	}

	//职位 类型显示中文
	public static string GetDutyName(int type)
	{
		string dutyName = "";
		switch (type) 
		{
			case 1:
				dutyName = "成员";
				break;
			case 2:
				dutyName = "队长";
				break;
			case 3:
				dutyName = "祭祀";
				break;
			case 4:
				dutyName = "副团长";
				break;
			case 5:
				dutyName = "团长";
				break;
			default:
				dutyName = "成员";
				break;
		}
		return dutyName;
	}

	//建筑信息
	private void buildViewSet()
	{
	
	}

	//成员工资列表
	private IEnumerator salaryViewSet()
	{
		clearList ();
		GameObject saveBtn = Util.FindChildRecursive (salaryview.transform, "SaveButton").gameObject;
		saveBtn.SetActive(isMaster);
		resetScrollViewPosition(Util.FindChildRecursive(salaryview.transform, "ScrollView"),0,0);
		yield return StartCoroutine(GuildController.guildController.LoadWageList());
		List<WageItem> wageList = GuildController.guildController.GetWageList ();
		int Length = wageList.Count;
		Debug.Log("salaryViewSet:"+Length);
		for (int i = 0; i < Length; i++) {
			var obj = NGUITools.AddChild(Util.FindChildRecursive(salaryview.transform, "Grid").gameObject,Util.FindChildRecursive(salaryview.transform, "salaryMesh").gameObject);
			obj.SetActive(true);
			UIDragScrollView sc = obj.GetComponent<UIDragScrollView>();
			sc.scrollView = Util.FindChildRecursive(salaryview.transform, "ScrollView").GetComponent<UIScrollView>();
			BoxCollider inputBtn = Util.FindChildRecursive (obj.transform, "Input").GetComponent<BoxCollider> ();
			inputBtn.enabled = isMaster;
			obj.newName = wageList[i].PlayerId.ToString();
			salaryItemSet(wageList[i],obj);
			itemList.Add(obj);
		}

		GridSet (Util.FindChildRecursive(salaryview.transform, "Grid"));

	}


	//工资列表数据设置
	private void salaryItemSet(WageItem itemInfo,GameObject obj)
	{
		SetText (obj, "name", itemInfo.PlayerName);
		SetText (obj, "occupation", Util.GetJobName((int)(itemInfo.Job)));//职业
		string jobname = Util.GetJobName (1);
		SetText (obj, "level", itemInfo.PlayerLevel.ToString());
		SetText (obj, "post", GetDutyName(itemInfo.Duty));
		SetText (obj, "contribution", itemInfo.Donate.ToString());
		SetText (obj, "inputText", itemInfo.Wage.ToString());
	}
	
	private void MemberOnClick()
	{
		if (Member.value == true) {
			ViewOnClick();
			StartCoroutine(menberInitSet());
			MemberView.SetActive(true);
		}
	}
	
	bool online;
	//成员列表
	private IEnumerator menberInitSet()
	{
		clearList ();
		resetScrollViewPosition(Util.FindChildRecursive(MemberView.transform, "ScrollView"),0,0);
		yield return StartCoroutine(GuildController.guildController.LoadMembersList());
		List<GuildMemberItem> memberList = GuildController.guildController.GetMembersList ();
		int Length = memberList.Count;
		int onLineNum = 0;
		Debug.Log("menberInitSet:"+Length);
		for (int i = 0; i < Length; i++) {
			var obj = NGUITools.AddChild(Util.FindChildRecursive(MemberView.transform, "Grid").gameObject,Util.FindChildRecursive(MemberView.transform, "MemberMeshMember").gameObject);
			obj.SetActive(true);
			UIDragScrollView sc = obj.GetComponent<UIDragScrollView>();
			sc.scrollView = Util.FindChildRecursive(MemberView.transform, "ScrollView").GetComponent<UIScrollView>();
			
			obj.newName = memberList[i].PlayerId.ToString();
			memberItemSet(memberList[i],obj);
			itemList.Add(obj);
			if(memberList[i].IsOnline)
			{
				onLineNum++;
			}
		}

		GridSet (Util.FindChildRecursive(MemberView.transform, "Grid"));

		SetText (MemberView, "peopleOnlineNum", onLineNum+ "/" + config.MemberLimit);
	}

	private void memberItemSet(GuildMemberItem itemInfo,GameObject obj)
	{
		BoxCollider inputBtn = Util.FindChildRecursive (obj.transform, "selectPost").GetComponent<BoxCollider> ();
		inputBtn.enabled = isMaster;
		UIPopupList popup = Util.FindChildRecursive (obj.transform, "selectPost").GetComponent<UIPopupList> ();
		/*if(!isMaster)
		{
			popup.items.Insert(0,"团长");
		}*/


	
		popup.value = GetDutyName(itemInfo.Duty);
		SetText (obj, "Name", itemInfo.PlayerName);
		SetText (obj, "Occupation", Util.GetJobName(itemInfo.Job));//职业
		SetText (obj, "Level", itemInfo.PlayerLevel.ToString());
		SetText (obj, "Contribution", itemInfo.Donate.ToString());
		
	}

	private void SocietyListOnClick()
	{
		if (SocietyList.value == true) {
			ViewOnClick();
			StartCoroutine(societyInitSet());
			SocietyListView.SetActive(true);
		}
	}

	//公会列表
	private IEnumerator societyInitSet()
	{
		clearList ();
		resetScrollViewPosition(Util.FindChildRecursive(SocietyListView.transform, "ScrollView"),0,0);
		yield return StartCoroutine(GuildController.guildController.LoadGuildListInfo());
		List<GuildItem> guildList = GuildController.guildController.GetGuildList ();
		int Length = guildList.Count;
		Debug.Log("societyInitSet:"+Length);
		for (int i = 0; i < Length; i++) {
			
			var obj = NGUITools.AddChild(Util.FindChildRecursive(SocietyListView.transform, "Grid").gameObject,Util.FindChildRecursive(SocietyListView.transform, "SocietyListMesh").gameObject);
			obj.SetActive(true);
			UIDragScrollView sc = obj.GetComponent<UIDragScrollView>();
			sc.scrollView = Util.FindChildRecursive(SocietyListView.transform, "ScrollView").GetComponent<UIScrollView>();
			UIButton[] buttons = obj.GetComponentsInChildren<UIButton> (true);
			foreach (UIButton button in buttons) {
				UIEventListener.Get (button.gameObject).onClick = societyItemClick;
			}
			obj.newName = guildList[i].GuildMasterId.ToString();
			societyItemSet(guildList[i],obj);
			itemList.Add(obj);

		}
		Log.GUI ("grid is  start");

		GridSet (Util.FindChildRecursive(SocietyListView.transform, "Grid"));
	}

	private void societyItemSet(GuildItem itemInfo,GameObject obj)
	{
		SetText (obj, "GuildName", itemInfo.GuildName);
		SetText (obj, "GuildMaster", itemInfo.MasterName);
		SetText (obj, "GuildLevel", itemInfo.GuildLevel.ToString());
		SetText (obj, "GuildNumber", itemInfo.MemberCount.ToString());
		SetText (obj, "GuildRanking", itemInfo.Top.ToString());

	}

	private void societyItemClick(GameObject btn)
	{
		if (btn.newName == "CallGuildMaster") 
		{
			Debug.Log("跳转 打开邮件写邮件面板");
		} else if (btn.newName == "ApplyFight") 
		{
			Debug.Log("功能未开启");
			var evt = new MyEvent(MyEvent.EventType.DebugMessage);
			evt.strArg = "功能未开启";
			MyEventSystem.myEventSystem.PushEvent(evt);
		}
	}
	
	private void VerificationOnClick()
	{
		if (Verification.value == true) {
			ViewOnClick();
			verificationInitSet();
			VerificationView.SetActive(true);
		}
	}

	//验证信息版面
	private void verificationInitSet()
	{
		clearList ();
		resetScrollViewPosition(Util.FindChildRecursive(SocietyListView.transform, "ScrollView"),0,0);
		int Length = 4;
		Debug.Log("verificationInitSet:"+Length);
		for (int i = 0; i < Length; i++) {
			
			var obj = NGUITools.AddChild(Util.FindChildRecursive(ApplyView.transform, "Grid").gameObject,Util.FindChildRecursive(ApplyView.transform, "MessageMesh").gameObject);
			obj.SetActive(true);
			UIDragScrollView sc = obj.GetComponent<UIDragScrollView>();
			sc.scrollView = ApplyView.GetComponent<UIScrollView>();
			UIButton[] buttons = obj.GetComponentsInChildren<UIButton> (true);
			foreach (UIButton button in buttons) {
				UIEventListener.Get (button.gameObject).onClick = applyItemClick;
			}
			obj.newName = i.ToString();
			//societyItemSet(enemylistsinfo[i],obj);
			itemList.Add(obj);
			
		}
		Log.GUI ("grid is  start");
		
		GridSet (Util.FindChildRecursive(ApplyView.transform, "Grid"));
	}

	private void applyItemSet(VerifyItem itemInfo,GameObject obj)
	{
		SetText (obj, "Time", itemInfo.VerifyTime.ToString());
		string str = "";
		//if(itemInfo.
	}

	private void bottombtnEvent()
	{
		UIButton[] buttons = ButtomBtn.GetComponentsInChildren<UIButton> (true);
		foreach (UIButton button in buttons) {
			UIEventListener.Get (button.gameObject).onClick = applyItemClick;
		}
	}

	private void applyItemClick(GameObject btn)
	{
		if (btn.newName == "Consent") 
		{
			Debug.Log("同意 入团");
		} else if (btn.newName == "Refuse") 
		{
			Debug.Log("拒绝 入团");
		}else if (btn.newName == "ConsentButton") 
		{
			Debug.Log("全部同意");
		}else if (btn.newName == "RefuseButton") 
		{
			Debug.Log("全部拒绝 入团");
		}else if (btn.newName == "DeleteButton") 
		{
			Debug.Log("全部删除");
		}
	}

	//申请信息
	private void ApplyOnClick()
	{
		if (Apply.value == true) {
			
			GvGView.SetActive( false );
			ButtomBtn.SetActive(true);
			ApplyView.SetActive( true );
		}
	}

	//公会战信息
	private void GvGOnClick()
	{
		if (GvG.value == true) {
			
			ApplyView.SetActive( false );
			ButtomBtn.SetActive(false);
			GvGView.SetActive( true );
		}
	}
	
	void ViewOnClick()
	{
		MessageView.SetActive (false);
		MemberView.SetActive (false);
		SocietyListView.SetActive (false);
		VerificationView.SetActive (false);
	}


	private void infoOnClick()
	{
		if (info.value == true) {
			infoViewInit();
		}
	}

	private void infoViewInit()
	{
		buildview.SetActive(false);
		salaryview.SetActive(false);
		infoViewSet();
		rootview.SetActive(true);
		infoview.SetActive(true);
	}
	
	private void buildOnClick()
	{
		if (build.value == true) {
			infoview.SetActive(false);
			salaryview.SetActive(false);
			rootview.SetActive(true);
			buildViewSet();
			buildview.SetActive(true);
		}
	}
	
	private void salaryOnClick()
	{
		if (salary.value == true) {
			rootview.SetActive(false);
			StartCoroutine(salaryViewSet());
			salaryview.SetActive(true);
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


	//面板 下面列 按钮的事件处理
	private void buttombtnClick(GameObject btn)
	{
		switch (btn.newName) 
		{
			case "ManorButton":
				enterGuild ();
				break;
			case "DissolveButton":
				disbandGuild ();
				break;
			case "AlterButton":
				noticeShow ();
				break;
			case "ExitButton":
				exitGuild ();
				break;
		}
	}

	private void noticeShow()
	{
		Debug.Log ("修改 公告");
		UIButton[] buttons = NoticeTips.GetComponentsInChildren<UIButton> (true);
		foreach (UIButton button in buttons) {
			UIEventListener.Get (button.gameObject).onClick = noticeClick;
		}
		ChangeNoticeText.text = NoticeText.text;
		NoticeTips.SetActive (true);
	}

	private void noticeClick(GameObject btn)
	{
		if (btn.newName == "cancelButton")
		{
			NoticeTips.SetActive (false);
		}
		else if (btn.newName == "okButton")
		{
			Debug.Log("NoticeText.text:"+NoticeText.text);
			if(ChangeNoticeText.text != "")
			{
				modifyManifesto(ChangeNoticeText.text);
			}
		}
	}

	//进入领地
	private void enterGuild()
	{
		GuildController.guildController.StartCoroutine (GuildController.guildController.EnterGuildScene ());
	}
	//解散公会
	private void disbandGuild()
	{
		GuildController.guildController.StartCoroutine (GuildController.guildController.DisbandGuild ());
	}
	//更改宣言
	private void modifyManifesto(string str)
	{
		GuildController.guildController.StartCoroutine (GuildController.guildController.ModifyManifesto (str));
		NoticeText.text = ChangeNoticeText.text;
	}
	//退出公会
	private void exitGuild()
	{
		GuildController.guildController.StartCoroutine (GuildController.guildController.ExitGuild ());
	}

}
