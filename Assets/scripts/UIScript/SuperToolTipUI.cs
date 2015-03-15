
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	public class SuperToolTipUI : IUserInterface
	{
		void Awake() {
			regEvt = new System.Collections.Generic.List<MyEvent.EventType> (){
				MyEvent.EventType.ShowSuperToolTip,
				MyEvent.EventType.UpdateSuperTip,
			};
			RegEvent ();

			SetCallback ("closeBut", Hide);
			SetCallback ("equipBut", OnEquip);
			SetCallback ("sellBut", OnSell);
			SetCallback ("enforceBut", OnEnforce);
		}

		void OnEquip(GameObject g) {
			Hide (null);
			SuperToolTips.superToolTips.actionItem.DoAction1 ();
		}

		void OnSell(GameObject g) {
			Hide (null);
			SuperToolTips.superToolTips.actionItem.DoAction2 ();
		}

		void OnEnforce(GameObject g) {
			Hide (null);
			SuperToolTips.superToolTips.actionItem.DoAction3 ();
		}

		protected override void OnEvent (MyEvent evt)
		{
			if (evt.type == MyEvent.EventType.ShowSuperToolTip) {
				//SuperToolTips.superToolTips.SendAskItemInfoMsg();
				UpdateFrame();
			} else if (evt.type == MyEvent.EventType.UpdateSuperTip) {
				UpdateFrame();
			}
		}

		public void UpdateFrame() {
			ClearAllGem ();
			ClearText ();

			var type = SuperToolTips.superToolTips.GetItemType ();
			if (type == ActionItem.ItemType.PackageItem) {
				ItemTips();
			} else if (type == ActionItem.ItemType.Equip) {
				Equip();
			}
		}

		void ClearAllGem() {
		}
		void ClearText() {
		}

		//背包物品
		void ItemTips() {
			var ic = SuperToolTips.superToolTips.GetItemClass ();
			GetName ("equipBut").SetActive (false);
			GetName ("sellBut").SetActive (false);
			GetName ("enforceBut").SetActive (false);

			if (ic == SuperToolTips.ItemClass.EQUIP) {
				Equip ();
			} else if (ic == SuperToolTips.ItemClass.COMMON) {
				CommonItem ();
			} else if (ic == SuperToolTips.ItemClass.GEM) {
				GemItem ();
			} else if (ic == SuperToolTips.ItemClass.MATERIAL) {
				MaterialItem ();
			} else if (ic == SuperToolTips.ItemClass.QUESTITEM) {
				QuestItem();
			}
		}

		
		//任务物品
		void QuestItem() {
			var title = SuperToolTips.superToolTips.GetTitle ();
			GetLabel ("title").text = title;
			
			var icon = SuperToolTips.superToolTips.GetItemIcon ();
			SetIcon ("icon", icon.iconId, icon.iconName);
			
			var desc = SuperToolTips.superToolTips.GetItemDesc ();
			GetLabel ("Attribute").text = desc;

			GetName ("equipBut").SetActive (false);
			GetName ("sellBut").SetActive (false);

		}

		//材料物品
		void MaterialItem() {
			var title = SuperToolTips.superToolTips.GetTitle ();
			GetLabel ("title").text = title;
			
			var icon = SuperToolTips.superToolTips.GetItemIcon ();
			SetIcon ("icon", icon.iconId, icon.iconName);

			var desc = SuperToolTips.superToolTips.GetItemDesc ();
			GetLabel ("Attribute").text = desc;

			GetLabel ("equipLabel").text = "卖出";
			GetName ("equipBut").SetActive (true);
			GetName ("sellBut").SetActive (false);
		}


		//宝石物品
		void GemItem() {
			//Title
			//Icon
			//Attribute
			var title = SuperToolTips.superToolTips.GetTitle ();
			GetLabel ("title").text = title;
			
			var icon = SuperToolTips.superToolTips.GetItemIcon ();
			SetIcon ("icon", icon.iconId, icon.iconName);
			
			var strBaseInfo = SuperToolTips.superToolTips.GetItemBaseWhiteAttrInfo ();
			GetLabel ("Attribute").text = strBaseInfo;

			GetLabel ("equipLabel").text = "卖出";
			GetName ("equipBut").SetActive (true);
			GetName ("sellBut").SetActive (false);
		}



		//普通物品包括
		void CommonItem() {
			//商店和背包同时打开
			//背包物品直接出售即可
			//是否显示价格部分

			var title = SuperToolTips.superToolTips.GetTitle ();
			GetLabel ("title").text = title;

			var icon = SuperToolTips.superToolTips.GetItemIcon ();
			SetIcon ("icon", icon.iconId, icon.iconName);

			var strBaseInfo = SuperToolTips.superToolTips.GetItemBaseWhiteAttrInfo ();
			GetLabel ("Attribute").text = strBaseInfo;
			
		}

		//身上装备
		void Equip() {
			var title = SuperToolTips.superToolTips.GetTitle ();
			var quantity = SuperToolTips.superToolTips.GetItemEquipQuantity ();
			string str = "";
			if (quantity == 0) {	
				str = title;
			} else if (quantity == 1) {
				str = "[00FF00]" + title + "[FFFFFF]";
			} else if (quantity == 2) {
				str = "[0000FF]" + title + "[FFFFFF]";
			} else if (quantity == 3) {
				str = "[FF00FF]" + title + "[FFFFFF]";
			}

			GetLabel ("title").text = str;
			var lev = SuperToolTips.superToolTips.GetItemLevel ();

			var playerLev = ObjectManager.objectManager.GetMyProp (CharAttribute.CharAttributeEnum.LEVEL);
			if (playerLev < lev) {
				str = "[FF0000]" + lev + "[FFFFFF]";
			} else {
				str = lev.ToString();
			}
			GetLabel ("needLevel").text = str;

			var binfo = SuperToolTips.superToolTips.GetItemBindInfo ();
			if (binfo == ItemData.BindInfo.Free) {
				GetLabel ("bindInfo").text = "";
			} else if (binfo == ItemData.BindInfo.Pick) {
				GetLabel ("bindInfo").text = "[00FF00]拾取绑定[FFFFFF]";
			} else if (binfo == ItemData.BindInfo.Equip) {
				GetLabel ("bindInfo").text = "[00FF00]装备绑定[FFFFFF]";
			} else if (binfo == ItemData.BindInfo.Binded) {
				GetLabel ("bindInfo").text = "[00FF00]已绑定[FFFFFF]";
			}

			var icon = SuperToolTips.superToolTips.GetItemIcon ();
			SetIcon ("icon", icon.iconId, icon.iconName);

			//显示宝石数量
			var gem = SuperToolTips.superToolTips.GetGemInEquipInfo ();
			for (int i =0; i < gem.num; i++) {
				SetIcon("gem"+i, gem.gems[i].iconId, gem.gems[i].iconName);
			}

			//显示物品基本属性
			var strBaseWhiteInfo = SuperToolTips.superToolTips.GetItemBaseWhiteAttrInfo ();
			//重练属性
			var strGreen = "[00ff00]"+SuperToolTips.superToolTips.GetItemGreenAttrInfo ()+"[ffffff]";
			//精炼属性
			var strBlue = "[0000ff]"+SuperToolTips.superToolTips.GetItemExtBlueAttrInfo()+"[ffffff]";

			//宝石属性
			var strGem = "[00ff00]"+gem.gemDes+"[ffffff]";

			var strInfo = strBaseWhiteInfo + strGreen + strBlue + strGem;
			GetLabel ("Attribute").text = strInfo;

			Log.Important("Equipment SuperTooltips");

			var ot = SuperToolTips.superToolTips.actionItem.GetOwnerType ();
			if (ot == ActionItem.OwnerType.Backpack) {
				GetLabel ("equipLabel").text = "装备";
				GetName ("equipBut").SetActive (true);
				GetLabel ("sellLabel").text = "卖出";
				GetName ("sellBut").SetActive (true);
				GetName("enforceBut").SetActive(true);
				GetLabel("enforceLabel").text = "强化";
			} else if(ot == ActionItem.OwnerType.Equip) {
				//GetLabel ("equipLabel").text = "脱下";
				GetName ("equipBut").SetActive (false);
				GetName ("sellBut").SetActive (false);
			}

		}
	}

}