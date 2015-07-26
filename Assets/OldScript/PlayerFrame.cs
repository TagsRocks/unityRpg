
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace ChuMeng {
	public class PlayerFrame : IUserInterface {
		public UILabel HpLabel;
		public UILabel mpLabel;
		public UISlider hp;
		public UISlider mp;
		UILabel LvNum, ExpLabel;
		void Awake() {
			Log.Important ("Register PlayerFrame Event");
			regEvt = new System.Collections.Generic.List<MyEvent.EventType> () {
				MyEvent.EventType.UnitHP,
				MyEvent.EventType.UnitHPPercent,
				MyEvent.EventType.UnitMP,
				MyEvent.EventType.UnitMPPercent,
				MyEvent.EventType.UpdatePlayerData,
				MyEvent.EventType.UnitExp,
			};
			RegEvent ();


			SetCallback ("Knapsack_Button", OnKnapsack);
			SetCallback ("ReturnCity_Button", OnCopy);
			SetCallback ("Shop_Button", OnMall);
			SetCallback ("HeadButton", OnHead);
			SetCallback ("EmaiL_Button", OnEmail);
			SetCallback ("Social_Button", OnSocial);

			SetCallback ("ChatButton", OnChat);
			SetCallback ("Knightage_Button", OnKinghtage);
			SetCallback ("Achievement_Button", OnAchievement);
			SetCallback ("Task_Button", OnTask);
			SetCallback ("Metallurgy_Button", OnVip);
			//SetCallback ("Arena_Button", OnArena);
			//SetCallback ("Ranking_Button", OnRanking);


			HpLabel = GetLabel ("HPLabel");
			mpLabel = GetLabel ("MPLabel");
			hp = GetSlider ("HP");
			mp = GetSlider ("MP");
			LvNum = GetLabel ("LvNum");
			ExpLabel = GetLabel ("ExpLabel");
		}

		void OnChat(GameObject g) {
			WindowMng.windowMng.PushView ("UI/Chat");
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.UpdateChat);
		}

		bool showHead = false;
		void OnHead(GameObject g) {
			if (!showHead) {
				showHead = true;
				GetName ("RightButton_Panel").SetActive (true);
			} else {
				showHead = false;
				GetName ("RightButton_Panel").SetActive (false);
			}
		}

		//商店
		void OnMall(GameObject g) {
			WindowMng.windowMng.PushView ("UI/shopping");
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.OpenMall);
		}

		//副本
		void OnCopy(GameObject g) {
			WindowMng.windowMng.PushView ("UI/SelectSingle");
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.OpenCopyUI);
		}

		//邮件
		void OnEmail(GameObject g) {
			WindowMng.windowMng.PushView ("UI/Mail_System");
		}

		//社交
		void OnSocial(GameObject g) {
			WindowMng.windowMng.PushView ("UI/SocialFriend");
		}

		//骑士团
		void OnKinghtage(GameObject g) {
			WindowMng.windowMng.PushView ("UI/SocialFriend");
		}

		//背包
		void OnKnapsack(GameObject g) {
			WindowMng.windowMng.PushView ("UI/knapsack");
		}

		//任务
		void OnTask(GameObject g) {
			WindowMng.windowMng.PushView ("UI/Task");
		}

		//成就
		void OnAchievement(GameObject g) {
			WindowMng.windowMng.PushView ("UI/achievement");
		}

		//vip 暂时用炼金按钮代替
		void OnVip(GameObject g){
			WindowMng.windowMng.PushView ("UI/Vip");
		}

		protected override void OnEvent(MyEvent evt) {
			Log.Important ("PlayerFrame Init Event "+evt.localID+" "+evt.type+" "+ObjectManager.objectManager.GetMyLocalId());
			if (evt.localID == ObjectManager.objectManager.GetMyLocalId()) {


				if (evt.type == MyEvent.EventType.UnitHP) {
					HpLabel.text = evt.intArg.ToString ()+"/"+evt.intArg1.ToString();
				} else if (evt.type == MyEvent.EventType.UnitMP) {
					mpLabel.text = evt.intArg.ToString ()+"/"+evt.intArg1.ToString();
				} else if (evt.type == MyEvent.EventType.UnitHPPercent) {
					hp.value = evt.floatArg;
				} else if (evt.type == MyEvent.EventType.UnitMPPercent) {
					mp.value = evt.floatArg;
				}else if(evt.type == MyEvent.EventType.UpdatePlayerData) {
					var myData = ObjectManager.objectManager.GetMyData();
					var attr = myData.GetComponent<NpcAttribute>();
					LvNum.text = ObjectManager.objectManager.GetMyProp(CharAttribute.CharAttributeEnum.LEVEL).ToString();
					HpLabel.text = ObjectManager.objectManager.GetMyProp(CharAttribute.CharAttributeEnum.HP).ToString()+"/"+ObjectManager.objectManager.GetMyProp(CharAttribute.CharAttributeEnum.HP_MAX).ToString();
					mpLabel.text = myData.GetProp(CharAttribute.CharAttributeEnum.MP).ToString ()+"/"+myData.GetProp(CharAttribute.CharAttributeEnum.MP_MAX).ToString();
					var hpv = myData.GetProp(CharAttribute.CharAttributeEnum.HP)*1.0f/myData.GetProp(CharAttribute.CharAttributeEnum.HP_MAX);
					hp.value = hpv;
					var mpv = myData.GetProp(CharAttribute.CharAttributeEnum.MP)*1.0f/myData.GetProp(CharAttribute.CharAttributeEnum.MP_MAX);
					mp.value = mpv;
					ExpLabel.text = myData.GetProp(CharAttribute.CharAttributeEnum.EXP).ToString()+"/"+attr.ObjUnitData.MaxExp.ToString();
				}
			}

		}



	}
}
