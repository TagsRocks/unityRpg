/*
Author :   Wangjunbo
Emial :     1305201219@qq.com
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class GameInterface_Vip{
		public static GameInterface_Vip vipInterface = new GameInterface_Vip();

		//获取角色vip信息
		public VipInfo GetVipInfo(){
			return VipController.vipController.GetVipInfo ();		
		}

		//获取vip经验
		public int GetVipNeedExp(int viplevel){
			return GMDataBaseSystem.database.SearchId<VipLevelConfigData> (GameData.VipLevelConfig, viplevel).vipExp;		
		}

		//获取vip特权
		public string GetVipDescribe(int id){
			VipDescribeConfigData v = GMDataBaseSystem.database.SearchId<VipDescribeConfigData> (GameData.VipDescribeConfig, 1);
			return v.vipDescribe;
		}

		/*获取vip等级礼包接口*/
		public List<VipLevelItem> GetVipLevelAward(){

			return VipController.vipController.GetVipLevelReward ();
		}

		/*获取vip特权礼包接口*/
		public List<VipFreeItem> GetVipFreeAward(){
			return VipController.vipController.GetVipFreeReward ();
		}
	}
}