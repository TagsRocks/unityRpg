/*--------------------------------*/
//  作者 ： QiuChell
//  时间 ： 2015年01月26日
//  说明 ： 邮件系统
/*--------------------------------*/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ChuMeng;

public class EmailSystemUI : IUserInterface {
	
	//变量定义
	/*--------------------------------------------------------------*/
	public GameObject EmailPanel;               //邮件主面板
	public GameObject NewEmailPanel;            //新建邮件面板
	public GameObject OpenEmailPanel;           //打开邮件面板
	public GameObject OpendSystemTipPanel;      //打开系统提示框面板
	
	public UIButton ClosePannel;             //销毁邮件主面板
	public UIButton NewEmailButton;          //新建邮件按钮
	public UIButton CloseNewEmailButton;     //关闭写邮件按钮
	public UIButton CloseOpenEmailButton;    //关闭收邮件面板
	public UIButton CloseSysSureButton;      //系统提示框 确定
	public UIButton CloseSysCannelButton;    //系统提示框 取消
	public UILabel SystemTipLabel;           //系统提示框提示内容
	public UIButton DelSingleEMail;          //删除单个邮件
	public UIButton GetSingleAnnex;          //提前单个附件
	public UIButton DelAllEMail;             //删除所有邮件
	public UIButton GetAllAnnex;             //提前所有附件
	public UIButton SendEMail;               //发送邮件
	public UIButton InputButton;             //金币输入组件
	public UIButton ReplyButton;             //回复邮件按钮
	
	
	public TweenPosition ToPosition;             //点击输入金币界面朝上移动的动画
	public UILabel acceptName;
	public UILabel writeTitle;
	public UILabel writeContents;
	public UILabel giveMoneys;
	public UIPopupList popuplist;
	public UIButton[] annexList;
	
	public GameObject EmailGrid;
	private List<GameObject> ListInfo;       //邮件主面板邮件列表
	List<Mail> allmaillistinfo;
	private int openMailID;
	private int systemType = 0;             //0 删除所有  1删除单个
	private bool To = false;
	//执行函数
	/*--------------------------------------------------------------*/
	//初始化函数
	void Awake()
	{
		ListInfo = new List<GameObject>();
		//新建邮件面板朝上移动动画最开始是不播放的  只有点击了输入金币框才会移动
		ToPosition.enabled = false;
		//游戏开始时新建邮件面板隐藏
		NewEmailPanel.SetActive (false);
		//游戏开始时邮件打开面板隐藏
		OpenEmailPanel.SetActive (false);

		//添加Grid下面的按钮到链表中
		
		int Length = GameInterface_EMail.GetInstance().GetEMailNum();
		allmaillistinfo = GameInterface_EMail.GetInstance().GetMailListInfo();
		Debug.Log(Length);
		for (int i = 0; i < Length; i++) {
			var obj = NGUITools.AddChild(EmailGrid,Util.FindChildRecursive(EmailGrid.transform, "EmailMesh0").gameObject);
			obj.SetActive(true);
			obj.name = allmaillistinfo[i].MailId.ToString();
			contentset(allmaillistinfo[i],obj);
			ListInfo.Add(obj);
			UIEventListener.Get(obj.gameObject).onClick = OpenWriteEmail;
		}

		GridSet (EmailGrid.transform);
		buttonClickEvent();
	}
	//郵件列表信息顯示
	void contentset (Mail info, GameObject obj)
	{
		string senderName;
		string title;
		if (info.Mark == true) {
			senderName = "[7b7c9f]" + info.Sender;
			title = "[7b7c9f]" + info.Title;
		} else {
			senderName = info.Sender;
			title = info.Title;
		}
		SetText (obj, "NameLabel", senderName);
		SetText (obj, "HeadLineLabel", title);
		if (info.IsAttach == true) {
			
		}
	}
	
	private void buttonClickEvent()
	{
		//按钮响应
		SetCallback ("CloseButton", Hide);
		SetCallback ("NewButton", NewEmail);//打开新建邮件面板  
		SetCallback ("ExtractButton", GetAllAnnexDeal);
		SetCallback ("DeleteAllButton", DelAllEMailDeal);

		//EventDelegate.Add(NewEmailButton.onClick, NewEmail);       
		EventDelegate.Add(CloseNewEmailButton.onClick, CloseEmail);     //关闭新建邮件面板
		EventDelegate.Add(InputButton.onClick, InPutOnclick);       //点击金币输入框按钮响应
		EventDelegate.Add(CloseOpenEmailButton.onClick, CloseOpenEmail);
		EventDelegate.Add(ClosePannel.onClick, onClickDel);
		EventDelegate.Add(ReplyButton.onClick, ReplyEmail);
		EventDelegate.Add(CloseSysSureButton.onClick, SureDeal);
		EventDelegate.Add(CloseSysCannelButton.onClick, CannelDeal);
		//EventDelegate.Add(DelAllEMail.onClick, DelAllEMailDeal);
		//EventDelegate.Add(GetAllAnnex.onClick, GetAllAnnexDeal);
		EventDelegate.Add(DelSingleEMail.onClick, DelSingleEMailDeal);
		EventDelegate.Add(GetSingleAnnex.onClick, GetSingleAnnexDeal);
		EventDelegate.Add(SendEMail.onClick, SendEMailDeal);
		EventDelegate.Add(popuplist.onChange, ChangeAcceptName);
		foreach(UIButton bt in annexList)
		{
			UIEventListener.Get(bt.gameObject).onClick = writeSaveAnnexDeal;
		}
	}
	
	//点击金币输入按钮响应
	private void InPutOnclick()
	{
		if (To == false) 
		{
			//动画重制并播放
			ToPosition.ResetToBeginning ();
			ToPosition.enabled = true;
			ToPosition.PlayForward ();
			To = true;
		}
	}
	
	private void ReplyEmail()
	{
		OpenEmailPanel.SetActive (false);
		NewEmailPanel.SetActive (true);
	}
	
	//点击新建邮件按钮
	private void NewEmail(GameObject g)
	{
		Debug.Log ("新建？");
		//切换两个面板
		EmailPanel.SetActive (false);
		NewEmailPanel.SetActive (true);
	}
	//关闭新建邮件面板
	private void CloseEmail()
	{
		//关闭面板 并关闭停止响应背景图
		NewEmailPanel.SetActive (false);
		//OpenSystemManage.OpenAlphaImage ();
		//把新建邮件的面板位置归0
		NewEmailPanel.transform.position = Vector3.zero;
		EmailPanel.SetActive(true);
		To = false;
	}
	
	//收邮件面板响应
	
	private void OpenWriteEmail(GameObject button)
	{
		openMailID = int.Parse(button.name);
		UILabel[] label = button.GetComponentsInChildren<UILabel> (true);
		foreach (UILabel la in label)
		{
			la.text = "[7b7c9f]" + la.text;
		}
		StartCoroutine(setMailInfoShow ());
	}
	
	private IEnumerator setMailInfoShow()
	{
		yield return StartCoroutine(MailController.mailController.ReadMail (openMailID));
		EMailData reamMailInfo = GameInterface_EMail.GetInstance ().GetSingleEmaildata ();
		SetText (OpenEmailPanel, "Sender", reamMailInfo.sender);
		SetText (OpenEmailPanel, "Surplus_Time", "剩余时间："+reamMailInfo.leaveTimer+"天");
		SetText (OpenEmailPanel, "Title", reamMailInfo.title);
		SetText (OpenEmailPanel, "Text_Label", reamMailInfo.content);
		if (reamMailInfo.Attachment.Count > 0)
		{
			
		}
		//关闭邮件主面板
		EmailPanel.SetActive (false);
		OpenEmailPanel.SetActive (true);
	}
	
	private void CloseOpenEmail()
	{
		OpenEmailPanel.SetActive (false);
		EmailPanel.SetActive(true);
		//OpenSystemManage.OpenAlphaImage ();
	}
	
	//系统提示框 确定处理
	private void SureDeal()
	{
		OpendSystemTipPanel.SetActive(false);
		if (systemType == 0)
		{
			for (int i = 0; i < ListInfo.Count; i++)
			{
				GameInterface_EMail.GetInstance().DelEMailRequest(int.Parse(ListInfo[i].name));
				Destroy(ListInfo[i]);
			}
			ListInfo.Clear();
		}
		else
		{
			GameInterface_EMail.GetInstance().DelEMailRequest(openMailID);
			GameObject des = EmailGrid.transform.Find(openMailID.ToString()).gameObject;
			for (int i = 0; i < ListInfo.Count; i++)
			{
				if (des.name == ListInfo[i].name)
				{
					ListInfo.RemoveAt(i);
				}
			}
			UIGrid uigrid = EmailGrid.GetComponent<UIGrid>();
			uigrid.RemoveChild(des.transform);
			Destroy(des);
			uigrid.Reposition();
			uigrid.repositionNow = true;
		}
		OpenEmailPanel.SetActive(false);
		EmailPanel.SetActive(true);
	}
	
	//系统提示框 取消处理
	private void CannelDeal()
	{
		OpendSystemTipPanel.SetActive(false);
	}
	
	//隐藏邮件版面
	private void onClickDel()
	{
		this.gameObject.SetActive(false);
	}
	//删除所有邮件
	private void DelAllEMailDeal(GameObject g)
	{
		systemType = 0;
		SystemTipLabel.text = "您确定要删除么，可能有您未领取的奖励。删除后不可恢复!";
		OpendSystemTipPanel.SetActive(true);
	}
	//一键提起附件
	private void GetAllAnnexDeal(GameObject g)
	{
		GameInterface_EMail.GetInstance().GetAllAnnexRequest();
	}
	//删除单个邮件
	private void DelSingleEMailDeal()
	{
		systemType = 1;
		SystemTipLabel.text = "该邮件中包含您未领取的奖励，删除后不可恢复!";
		OpendSystemTipPanel.SetActive(true);
	}
	//提起单个附件
	private void GetSingleAnnexDeal()
	{
		//需判断是否有附件？
		if (GameInterface_EMail.GetInstance ().IsAttach (openMailID)) {
			GameInterface_EMail.GetInstance ().GetSingleAnnexRequest (openMailID);
		} else {
			//没有附件提取
			Debug.Log("None annex~ deal @ To do");
		}
	}

	private void ChangeAcceptName()
	{
		acceptName.text = popuplist.value;
	}
	//发送邮件
	private void SendEMailDeal()
	{
		//外部获取好友列表的同时，可以获取通过名称查找到相应名称对应的id
		Debug.Log ("send eMail:"+popuplist.value);
		Debug.Log ("send acceptName:"+acceptName.text);
		Debug.Log ("send writeTitle.text:"+writeTitle.text);
		Debug.Log ("send writeContents:"+writeContents.text);
		Debug.Log ("send money:" + giveMoneys.text);
		GameInterface_EMail.GetInstance ().SendEMailRequest (acceptName.text,writeTitle.text, writeContents.text, int.Parse(giveMoneys.text), 0, 0, 0, null);
		CloseEmail ();
	}
	private void writeSaveAnnexDeal(GameObject btn)
	{
		Debug.Log (btn.name);
		//接入打开物品的界面接口，选着道具作为赠送附件
	}
}
