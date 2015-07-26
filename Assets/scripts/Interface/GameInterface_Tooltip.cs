
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

/*
 * 包物品tips提示面板
 */
using System.Collections.Generic;

 
namespace ChuMeng
{
	/*
	 * 各类物品的tips的格式汇总
	 * 1:确定有哪些物品
	 * 2:每种物品的tips
	 * 3: tips 格式变化
	 */ 

	public class SuperToolTips
	{

		public static SuperToolTips superToolTips = new SuperToolTips();

		public ActionItem actionItem;

		public string GetTitle() {
			return actionItem.GetTitle();
		}

		public string GetIcon() {
			return null;
		}

		//装备属性值
		public string GetAttributeValue(string name) {
			return null;
		}



		//武器等级
		public int GetItemLevel() {
			return actionItem.itemData.Level;
		}




		public Icon GetItemIcon() {
			var i = new Icon ();
			i.iconId = actionItem.itemData.IconSheet;
			i.iconName = actionItem.itemData.IconName;
			return i;
		}
		//卖给npc的价格
		public int GetItemPrice() {
			return 0;
		}

		//物品的白色属性
		public string GetItemBaseWhiteAttrInfo() {
			return actionItem.GetBaseWhiteAttrInfo();
		}

		//物品精炼属性
		public string GetItemExtBlueAttrInfo( ) {
			return actionItem.GetBlueAttrInfo();
		}

		//物品重练属性
		public string GetItemGreenAttrInfo( ) {
			var green = actionItem.GetGreenAttrInfo ();
			return green;
		}

		//物品类型
		public ActionItem.ItemType GetItemType() {
			return ActionItem.ItemType.PackageItem;
		}

		//材料物品描述文字
		public string GetItemDesc() {
			return actionItem.GetItemDesc ();
		}
		//获取装备品质
		//White
		//Green
		//Blue
		//Purle
		public int GetItemEquipQuantity() {
			var lev = GetItemLevel ();
			if (lev <= 10) {
				return 0;
			}
			if (lev <= 30) {
				return 1;
			}
			if (lev <= 50) {
				return 2;
			}
			return 3;
		}

		public class GemInfo {
			public int num;
			public List<Icon> gems;
			public string gemDes;
		}

		//获取镶嵌宝石属性 以及属性描述文字
		public GemInfo GetGemInEquipInfo() {
			var g = new GemInfo();
			g.num = actionItem.GetGemInEquipCount ();
			g.gems = new List<Icon> ();

			for (int i = 0; i < 4; i++) {
				var icon = actionItem.GetGemIcon (i);
				g.gems.Add(icon);
			}
			g.gemDes = actionItem.GetGemInEquipDesc ();
			return g;
		}


		/*
		 * 技能相关信息
		 */ 
		public int GetSkillLevel() {
			return 0;
		}


		public bool GetIsLearnedSkill() {
			return false;
		}

		/*
		 * 宝石相关信息
		 */ 
		public int GetGemLevel() {
			return 0;
		}

		public string GetGemAttribInfo() {
			return null;
		}

		//宝石品质
		public string GetItemQuantity() {
			return null;
		}



		public IEnumerator SendAskItemInfoMsg() {
			yield return null;
		}

		public void SetActionItem(ActionItem item) {
			actionItem = item;
		}

		public enum ItemClass {
			NONE = -1,
			EQUIP,
			COMMON,
			GEM,
			MATERIAL,
			QUESTITEM,
		}

		public ItemClass GetItemClass() {
			if (actionItem.itemData.IsEquip()) {
				return ItemClass.EQUIP;
			}
			if (actionItem.itemData.IsGem ()) {
				return ItemClass.GEM;
			}
			if (actionItem.itemData.IsMaterial ()) {
				return ItemClass.MATERIAL;
			}
			if (actionItem.itemData.UnitType == ItemData.UnitTypeEnum.QUESTITEM) {
				return ItemClass.QUESTITEM;
			}
			return ItemClass.COMMON;
		}
	}
}
