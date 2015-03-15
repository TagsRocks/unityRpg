
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	/*
	 * 所有可以操控的对象
	 * 1 物品
	 * 2 技能
	 * 3 生活技能
	 * 
	 * 参考CGActionSystem:: NAMETYPE
	 * 
	 * ITEM_CLASS
	 * EQUIP
	 * MATERIAL
	 * TASKITEM
	 * 
	 * 背包 以及 装备槽 
	 */ 

	public class ActionSystem
	{
		public static ActionSystem actionSystem = new ActionSystem();


		//获取特定的装备槽的Item
		//Position  Filter
		public ActionItem EnumAction(ItemData.EquipPosition type) {
			Log.Important("Enum Action is "+type);
			return BackPack.backpack.EnumAction (type);
		}

	}

}