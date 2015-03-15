
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	/*
	 * 角色数据UI相关接口
	 * 
	 * 1 首先设置全局玩家，
	 * 2 接着获得这个玩家的数据
	 */ 
	public class Character
	{
		/*
		 * 当前UI界面操作的玩家
		 */ 
		//public static CharacterInfo charInfo = null;
		public static Character character = new Character();

		/*
		 * 获取基本属性
		 * 
		 * string Attribute Name
		 * 参考attribute.json
		 */ 
		public int GetData(CharAttribute.CharAttributeEnum attribute) {
			/*
			if (charInfo != null) {
				return charInfo.GetProp(attribute);
			}
			*/
			return 0;
		}

		/*
		 * 获取装备
		 */ 
		public EquipData GetEquip(ItemData.EquipPosition slotId) {
			return null;
		}

		/*
		 * 获取操作玩家的名称
		 */ 
		public string GetName() {
			return null;
		}


	}
}