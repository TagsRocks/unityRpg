
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace ChuMeng {
	public class MyChatInput : MonoBehaviour {
		public UITextList textList;
		//public bool fillWithDummyData = false;

		public UIInput mInput;
		public GameObject button;

		public GameObject closeBut;

		public GameObject world;
		public GameObject team;
		public GameObject privat;
		public GameObject guild;
		public GameObject system;
		public GameObject laba;

		public GameObject current;

		public UILabel currentLabel;
		public Talk.ChannelType channelType = Talk.ChannelType.World;

		public string privateTarget = "";

		void Awake() {
			Talk.talk.sendMsgDelegate += OnSend;
			Talk.talk.recvMsgDelegate += OnRecv;
			UIEventListener.Get (closeBut).onClick = OnClose;

			world = Util.FindChildRecursive (transform, "world").gameObject;
			UIEventListener.Get (world).onClick = OnChannel;
			team = Util.FindChildRecursive (transform, "team").gameObject;
			UIEventListener.Get (team).onClick = OnChannel;

			privat = Util.FindChildRecursive (transform, "private").gameObject;
			UIEventListener.Get (privat).onClick = OnChannel;
			guild = Util.FindChildRecursive (transform, "guild").gameObject;
			UIEventListener.Get (guild).onClick = OnChannel;
			system = Util.FindChildRecursive (transform, "system").gameObject;
			UIEventListener.Get (system).onClick = OnChannel;
			laba = Util.FindChildRecursive (transform, "laba").gameObject;
			UIEventListener.Get (laba).onClick = OnChannel;

			current = Util.FindChildRecursive (transform, "current").gameObject;
			currentLabel = Util.FindChildRecursive(current.transform, "currentLabel").GetComponent<UILabel>();

		}
		public void OnChannel(GameObject g) {
			var n = g.name;
			if (n.Equals ("world")) {
				currentLabel.text = "World";
				channelType = Talk.ChannelType.World;
			} else if (n.Equals ("team")) {
				currentLabel.text = "Team";
				channelType = Talk.ChannelType.Team;
			} else if (n.Equals ("private")) {
				currentLabel.text = "Private";
				channelType = Talk.ChannelType.Private;
			} else if (n.Equals ("guild")) {
				currentLabel.text = "Guild";
				channelType = Talk.ChannelType.Guild;
			} else if (n.Equals ("system")) {
				currentLabel.text = "System";
				channelType = Talk.ChannelType.System;
			} else if (n.Equals ("laba")) {
				currentLabel.text = "Laba";
				channelType = Talk.ChannelType.Laba;
			}
		}

		void OnClose(GameObject g) {
			gameObject.SetActive (false);
		}

		void OnSend(ChuMeng.Talk.HistoryMsg hisMsg) {
		
		}

		void OnRecv(Talk.HistoryMsg hisMsg) {
			textList.Add (hisMsg.recvMsg.PlayerName + " : " + hisMsg.recvMsg.ChatContent);	
		}

		void OnEnable() {
		}
		void OnDisable() {
		}

		void OnDestroy() {
		
		}
		// Use this for initialization
		void Start () {
			mInput.label.maxLineCount = 1;
			UIEventListener.Get (button).onClick = OnSubmit;
		}

		void OnSubmit(GameObject g) {
			Debug.Log ("Submit");
			if (textList != null) {
				// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
				string text = NGUIText.StripSymbols(mInput.value);
				Debug.Log("text "+text);
				if (!string.IsNullOrEmpty(text))
				{
					//textList.Add("You : "+text);
					mInput.value = "";
					mInput.isSelected = false;

					Talk.talk.SendChatMessage(text , channelType);
				}
			}
		}

		// Update is called once per frame
		void Update () {
		
		}
	}

}