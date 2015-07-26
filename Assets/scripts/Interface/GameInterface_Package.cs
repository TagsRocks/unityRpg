
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 背包UI获取数据接口
 * 
 * 1. 实例化一个全局背包接口对象
 * 2. 调用接口获取数据
 */ 

namespace ChuMeng {
	public class PlayerPackage {
		public static PlayerPackage playerPackage = new PlayerPackage();

		public enum PackagePageEnum
		{
			General = 1,
			Equip,
			Fashion,
			All,
			Task,
		}

		/*
		 * 背包类型：
		 * BackpackData
		 * 
		 * PackType
		 */ 

		public BackpackData EnumItem(PackagePageEnum type, int index) {
			return BackPack.backpack.EnumItem (type, index);
		}

        public void LevelUpEquip(EquipData eqData, List<BackpackData> gems){
            var lev = CGLevelUpEquip.CreateBuilder();
            lev.EquipId = eqData.id;
            foreach(var g in gems){
                lev.AddGemId(g.id);
            }
            KBEngine.Bundle.sendImmediate(lev);
        }
	}
}
