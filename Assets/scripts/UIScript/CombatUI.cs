using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ChuMeng;

namespace ChuMeng
{
	public class CombatUI : IUserInterface
	{
		public GameObject itemParent;
		public GameObject rightPannel;
		public GameObject item;
		public UIButton closebtn;
		private GCCombat combatInfo;
		private List<CombatLayerInfo> list;
		void Awake() {
			list = CombatController.combatController.GetItemList ();
			combatInfo = CombatController.combatController.combatInfo;
			SetCallback ("CloseButton", Hide);
			initleftpannel ();
			initrightpannel ();
		}

		private void initleftpannel()
		{
			//SetText (obj, "ItemLevel", iteminfo.Level.ToString());
			int Length = list.Count;
			Debug.Log("enemyList:"+Length);
			for (int i = 0; i < Length; i++) {
				var obj = NGUITools.AddChild(itemParent,item);
				obj.SetActive(true);
				obj.name = i.ToString();
				//itemcontentset(list[i],obj);
				UIEventListener.Get(obj.gameObject).onClick = clickListItem;
			}
			GridSet(itemParent.transform);
		}

		private void itemcontentset(CombatLayerInfo info,GameObject obj)
		{
				
		}

		void clickListItem(GameObject btn)
		{
			Debug.Log ("btn click :"+btn.name);
		}

		private void initrightpannel()
		{
			SetText (rightPannel, "MonsterName", combatInfo.CurrentName);
			SetText (rightPannel, "FightNum", combatInfo.CurrentFightNum.ToString());
			SetText (rightPannel, "RecordNum", combatInfo.CurrentChangeLog.ToString());
			SetText (rightPannel, "YesterdayNum", combatInfo.CurrentChangeLog.ToString());
			SetText (rightPannel, "PropertyNum", "");
			SetText (rightPannel, "TimesNum", combatInfo.CurrentCanChange.ToString());
			UIButton[] buttons = rightPannel.GetComponentsInChildren<UIButton> (true);
			foreach (UIButton button in buttons) {
				UIEventListener.Get (button.gameObject).onClick = btnClick;
			}
		}

		void btnClick(GameObject btn)
		{
			Debug.Log ("right  btn click :"+btn.name);
			if(btn.name == "StartButton")
			{
				combatChallenge();
			}else if(btn.name == "Ranking")
			{
				gotoRangPage();
			}
		}

		//开始挑战 接口
		private void combatChallenge()
		{

		}

		//排行版  接口
		private void gotoRangPage()
		{

		}
	}
}
