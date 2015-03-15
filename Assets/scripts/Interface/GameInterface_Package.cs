
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

		/*
		 * 整理背包
		 */ 
		public void PackUpPacket(PackType type) {
			CGAutoAdjustPack.Builder pack = CGAutoAdjustPack.CreateBuilder ();
			pack.PackType = type;
			KBEngine.Bundle.sendImmediate (pack);
		}

		/*
		 * 打开仓库
		 */ 
		public void OpenBank() {
		}

		//打开商店
		public void OpenStore() {
		}

		//打开拍卖行
		public void OpenAuction() {
		}

		//获取钱币数量
		public void GetMoney() {
			
		}

		//背包物品点击
		public void ItemClicked(int index) {
		}

	}
}
