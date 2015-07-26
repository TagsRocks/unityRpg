using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng {
	public class GameInterface_Compose {
		public static GameInterface_Compose compose = new GameInterface_Compose();

		//当前选择要强化的装备
		public BackpackData selEquip = null;

		//开始合成装备 根据配方编号
		//服务器通知背包物品消失，以及新的物品产生  GCPushGoodsCountChange 
		//消耗金钱
		public void ComposeItemBegin(EquipIntensifyConfigData curSel) {
			//BackPack.backpack.StartCoroutine(BackPack.backpack.ComposeItemCor (curSel.node));
		}

		public List<EquipIntensifyConfigData> GetIntensify(int equipId) {
			var l = new List<EquipIntensifyConfigData> ();
			foreach (EquipIntensifyConfigData ed in GameData.EquipIntensifyConfig) {
				if(ed.equipId == equipId) {
					l.Add(ed);
				}
			}
			return l;
		}

	}
}
