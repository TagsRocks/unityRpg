using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ChuMeng;


/*--------------------------------*/
//  作者 ： QiuChell
//  时间 ： 2015年03月04日
//  说明 ： 排行榜系统
/*--------------------------------*/

namespace ChuMeng
{
	public class RankUI : IUserInterface
	{
		public GameObject itemParent;
		public GameObject rankingTips;
		public GameObject item;
		private TopType rankType = TopType.TOP_GS;
		private List<TopItem> list;
		private List<GameObject> itemList = new List<GameObject>();
		private RankListController rankCtl;
		void Awake() {

			itemList.Clear ();
			rankCtl = RankListController.rankListController;

			SetCallback ("CloseButton", Hide);
			SetCheckBox ("PowerToggle", onPowerList);
			SetCheckBox ("CombatToggle", onCombatList);
			SetCheckBox ("PetToggle", onPetList);
			SetCheckBox ("VipToggle", onVipList);
			iniListPannel ();
			GetLabel ("myRankingNum").text = rankCtl.GetRanking ().ToString();
			
		}

		//战斗力
		void onPowerList(bool b)
		{
			if (b) {
				rankType = TopType.TOP_GS;
				updateFrame();
			}
		}


		//斗魂塔
		void onCombatList(bool b)
		{
			if (b) {
				rankType = TopType.TOP_TOWER;
				updateFrame();
			}
		}

		//宠物战力
		void onPetList(bool b)
		{
			if (b) {
				rankType = TopType.TOP_PET;
				updateFrame();
			}
		}

		//VIP
		void onVipList(bool b)
		{
			if (b) {
				rankType = TopType.TOP_VIP;
				updateFrame();
			}
		}


		private void updateFrame()
		{
			clearList ();
			StartCoroutine(requestRankLists());
		}

		private IEnumerator requestRankLists()
		{
			yield return StartCoroutine(rankCtl.LoadTop4Type(rankType));
			//yield return StartCoroutine(rankCtl.SearchPlayerTop(rankType,playerName));
			iniListPannel ();
		}

		private void iniListPannel()
		{
			//SetText (obj, "ItemLevel", iteminfo.Level.ToString());
			
			list = rankCtl.GetItemList ();
			int Length = list.Count;
			Debug.Log("top items:"+Length);
			for (int i = 0; i < Length; i++) {
				var obj = NGUITools.AddChild(itemParent,item);
				obj.SetActive(true);
				obj.newName = i.ToString();
				itemcontentset(list[i],obj);
				itemList.Add(obj);
				UIEventListener.Get(obj.gameObject).onClick = clickListItem;
			}
			GridSet(itemParent.transform);
		}

		private void itemcontentset(TopItem info,GameObject obj)
		{
			SetText (obj, "rank", info.Index.ToString());
			SetText (obj, "name", info.PlayerName.ToString());
			SetText (obj, "occupation", Util.GetJobName((int)(info.Job)));
			SetText (obj, "fightingCapacity", info.Gs.ToString());
		}

		void clickListItem(GameObject btn)
		{
			Debug.Log ("btn click :"+btn.newName);
		}

	/*	private void iniListPannel()
		{
			/*SetText (rightPannel, "MonsterName", combatInfo.CurrentName);
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
	*/

		private void clearList()
		{
			foreach (GameObject g in itemList) 
			{
				Destroy(g);
			}
			itemList.Clear ();
		}

		void btnClick(GameObject btn)
		{
			Debug.Log ("right  btn click :"+btn.newName);
		}

		//查看信息 接口
		private void checkAnotherInfo()
		{

		}

	}
}
