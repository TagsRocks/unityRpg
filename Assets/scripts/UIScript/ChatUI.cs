/*************************/
//Chat System  聊天系统
//Author : wangjunbo
//Time : 2014年12月30日
/*************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class ChatUI : IUserInterface
	{
		public UITextList textList;        //聊天内容文本显示区
		
		public UIInput mInput;      //聊天输入框
		
		public UIPopupList mPopuplist;  //发言频道选择菜单
		
		public GameObject all;             //全部标签
		public GameObject nearby;     //附近标签
		public GameObject world;       //世界标签
		public GameObject team;        //组队标签
		public GameObject privat;      //密语标签
		public GameObject guild;       //公会标签
		public GameObject system;      //系统标签
		
		public GameObject SendButton;  //发送按钮

		public GameObject TipsPannel;
		
		//发送消息默认是附近
		public Talk.ChannelType channelType = Talk.ChannelType.Nearby;     

		//接受消息默认是全部
		Talk.ChannelType recvChannel = Talk.ChannelType.all;

		void Awake ()
		{
			/*--------------------------------------------------------------*/
			//给频道标签添加响应的事件
			all = Util.FindChildRecursive (transform, "all").gameObject;
			Debug.Log(all);
			UIEventListener.Get (all).onClick = OnChannel;

			nearby = Util.FindChildRecursive (transform, "nearby").gameObject;
			Debug.Log (nearby);
			UIEventListener.Get (nearby).onClick = OnChannel;

			world = Util.FindChildRecursive (transform, "world").gameObject;
			Debug.Log(world);
			UIEventListener.Get (world).onClick = OnChannel;
			
			team = Util.FindChildRecursive (transform, "team").gameObject;
			Debug.Log(team);
			UIEventListener.Get (team).onClick = OnChannel;
			
			privat = Util.FindChildRecursive (transform, "privat").gameObject;
			Debug.Log(privat);
			UIEventListener.Get (privat).onClick = OnChannel;
			
			guild = Util.FindChildRecursive (transform, "guild").gameObject;
			Debug.Log(guild);
			UIEventListener.Get (guild).onClick = OnChannel;
			
			system = Util.FindChildRecursive (transform, "system").gameObject;
			Debug.Log(system);
			UIEventListener.Get (system).onClick = OnChannel;

			SendButton = Util.FindChildRecursive (transform, "send").gameObject;
			Debug.Log (SendButton);
			UIEventListener.Get (SendButton).onClick = OnChatSubmit;
			
			EventDelegate.Add (mPopuplist.onChange, SelectChannel);
			/*--------------------------------------------------------------*/


			SetCallback ("close", Hide);
			SetCallback ("textListLabel", OnlistName);
			SetCallback ("sure", OnSure);
			SetCallback ("cannel", OnCannel);

			//注册接收消息事件
			regEvt = new List<MyEvent.EventType> (){
				MyEvent.EventType.NewChatMsg,
			};
			RegEvent ();
		}

		//接受消息处理
		protected override void OnEvent (MyEvent evt)
		{
			if (evt.type == MyEvent.EventType.NewChatMsg) {
				OnChannel(null);
			}
		}
			
		//频道标签Event
		public void OnChannel (GameObject g)
		{
			Log.GUI ("Update Chat Content");
			//频道切换  从服务器把每个频道的信息拿出来
			var n = "all";
			if (g != null) {
				n = g.name;
			}
			Talk.Channel channel = null;

			if (n.Equals ("all")) {
				textList.Clear ();
				channel = GameInterface_Chat.chatInterface.GetChannelMsg(Talk.ChannelType.all);
				AddTextlist(channel);
			} else if (n.Equals ("world")) {
				textList.Clear();
				channel = GameInterface_Chat.chatInterface.GetChannelMsg(Talk.ChannelType.World);
				AddTextlist(channel);
			}else if(n.Equals("team")){
				textList.Clear();
				channel = GameInterface_Chat.chatInterface.GetChannelMsg(Talk.ChannelType.Team);
				AddTextlist(channel);
			}else if(n.Equals("privat")){
				textList.Clear();
				channel = GameInterface_Chat.chatInterface.GetChannelMsg(Talk.ChannelType.Private);
				AddTextlist(channel);
			}else if(n.Equals("guild")){
				textList.Clear();
				channel = GameInterface_Chat.chatInterface.GetChannelMsg(Talk.ChannelType.Guild);
				AddTextlist(channel);
			}else if(n.Equals("system")){
				textList.Clear();
				channel = GameInterface_Chat.chatInterface.GetChannelMsg(Talk.ChannelType.System);
				AddTextlist(channel);
			}else if(n.Equals("nearby")){
				textList.Clear();
				channel = GameInterface_Chat.chatInterface.GetChannelMsg(Talk.ChannelType.Nearby);
				AddTextlist(channel);
			}
		}

		//切换频道提取消息
		private void AddTextlist( Talk.Channel channeltest ){
			Log.GUI("channel Message is "+channeltest.recvHisQue.Count);
			for (int i = 0; i < channeltest.recvHisQue.Count; i++) {
				string name = channeltest.recvHisQue[i].recvMsg.PlayerName;
				string word = channeltest.recvHisQue[i].recvMsg.ChatContent;
				string text = "[url=" + channeltest.recvHisQue[i].recvMsg.PlayerId + "]" + name +":"+ "[/url]" + word;
				//获取玩家名称并添加聊天信息
				Log.GUI("message is "+text);
				textList.Add(text);			
			}	
		}

		private string url;
		void OnlistName(GameObject g)
		{
			UILabel lbl = g.GetComponent<UILabel>();

			url = lbl.GetUrlAtPosition(UICamera.lastWorldPosition);

			TipsPannel.SetActive (true);
		}

		void OnSure(GameObject g)
		{
			FriendsController.friendsController.StartCoroutine(FriendsController.friendsController.InviteFriend(int.Parse(url)));
			Debug.Log ("发送邀请成功！");
			TipsPannel.SetActive (false);
		}

		void OnCannel(GameObject g)
		{
			TipsPannel.SetActive (false);
		}

		//选择频道
		private void SelectChannel()
		{
					
			if(mPopuplist.value == "世界"){
				channelType = Talk.ChannelType.World;
			}else if(mPopuplist.value == "组队"){
				channelType = Talk.ChannelType.Team;
			}else if(mPopuplist.value == "密语"){
				channelType = Talk.ChannelType.Private;
			}else if(mPopuplist.value == "公会"){
				channelType = Talk.ChannelType.Guild;
			}else if(mPopuplist.value == "附近"){
				channelType = Talk.ChannelType.Nearby;
			}else if(mPopuplist.value == "喇叭"){
				channelType = Talk.ChannelType.Laba;
			}
		}
		
		//提交内容
		public void OnChatSubmit (GameObject g)
		{
			Debug.Log ("SendMsg");
			if (textList != null){
				//不让玩家输入输入符号改变颜色
				string text = NGUIText.StripSymbols (mInput.value);
				Debug.Log ("Text:" + text);
				
				if (!string.IsNullOrEmpty (text)){
					mInput.value = "";
					mInput.isSelected = false;
					//发送消息到对应的频道
					GameInterface_Chat.chatInterface.SendChatMsg(text, (int)channelType);

				}	
			}
		}
		
		
		
		
	}
	
	
}