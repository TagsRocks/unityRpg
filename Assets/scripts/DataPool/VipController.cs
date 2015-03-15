/*
Author: Wangjunbo
Email: 1305201219@qq.com
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class VipController : MonoBehaviour
	{
		public static VipController vipController;


		VipInfo vipinfo;                     															   /*角色的vip信息*/
		List<VipLevelItem> viplevelItem = new List<VipLevelItem>();     /*vip等级礼包数据*/
		List<VipFreeItem> vipfreeItem = new List<VipFreeItem>();         /*vip特权礼包数据*/


		void Awake()
		{
			vipController = this;
			DontDestroyOnLoad (gameObject);
		}

		public IEnumerator LoadVip(){
			//加载vip信息
			var packet = new KBEngine.PacketHolder ();
			var listVipInfo = CGLoadVipInfo.CreateBuilder ();
			yield return StartCoroutine(KBEngine.Bundle.sendSimple(this, listVipInfo, packet));
			var vipinfoData = packet.packet.protoBody as GCLoadVipInfo;
			vipinfo = new VipInfo (vipinfoData.VipLevel, vipinfoData.VipExp, vipinfoData.VipRemainTime);

			//加载已领取vip等级礼包
			var listVipLevel = CGLoadVipLevelGiftReceiveInfo.CreateBuilder ();
			yield return StartCoroutine(KBEngine.Bundle.sendSimple(this, listVipLevel, packet));
			var viplevelData = packet.packet.protoBody as GCLoadVipLevelGiftReceiveInfo;

			//加载vip等级礼包
			foreach (VipLevelRewardConfigData v in GameData.VipLevelRewardConfig) {
				if(v.condition > vipinfo.Level){
					var n = new VipLevelItem(v.id, false);
					viplevelItem.Add(n);
				}else
				{
					var n = new VipLevelItem(v.id, true);
					foreach(ReceviedReward m in viplevelData.ReceviedLevelRewardsList){
						if( m.RewardId == v.id){
							n.Enable = false;
							break;
						}
					}
					viplevelItem.Add(n);

				}
			}

			for(int i = vipinfo.Level; i <= 15; i++){
				var n = new VipLevelItem(i, false);
				viplevelItem.Add(n);
			}

			Log.Net("data is" + viplevelData.ToString());
			Log.Net("data is" + viplevelItem.Count);
			/*
			var listVipFree = CGLoadVipFreeGiftReceiveInfo.CreateBuilder ();
			yield return StartCoroutine(KBEngine.Bundle.sendSimple(this, listVipFree, packet));
			var vipfreeData = packet.packet.protoBody as GCLoadVipFreeGiftReceiveInfo;

			//加载vip特权礼包
			foreach (VipMonthRewardConfigData m in GameData.VipMonthRewardConfig) {
				var n = new VipFreeItem(m.id, true);
				foreach(ReceviedReward r in vipfreeData.ReceviedFreeRewardsList){
					if(r.RewardId == m.id){
						n.Enable = false;
					}
				}
				vipfreeItem.Add(n);
			}
			*/
		}

		//获取vip信息
		public VipInfo GetVipInfo(){
			return vipinfo;		
		}
	
		//获取vip等级礼包
		public List<VipLevelItem> GetVipLevelReward(){
			return viplevelItem;
		}

		/*获取vip特权礼包*/
		public List<VipFreeItem> GetVipFreeReward(){
			return vipfreeItem;
		}
	}

	//角色vip信息
	public class VipInfo{
		int vipLevel;
		int vipExp;
		long vipTime;

		/*vip等级*/
		public int Level{
			get{
				return vipLevel;
			}
		}

		/*vip经验*/
		public int Exp{
			get{
				return vipExp;
			}
		}

		/*vip时间*/
		public long Time{
			get{
				return vipTime;
			}
		}

		public VipInfo(int level, int exp, long time){
			vipLevel = level;
			vipExp = exp;
			vipTime = time;
		}
	}

	/*vip等级奖励*/
	public class VipLevelItem
	{
		VipLevelRewardConfigData vipleveldata;
		bool enable;

		/*礼包id*/
		public int Id{
			get{
				return vipleveldata.id;
			}
		}

		public bool Enable{
			get{
				return enable;
			}
			set{
				enable = value; 
			}
		}

		/*礼包说明*/
		public string Describe{
			get{
				return vipleveldata.typeDescribe;
			}
		}

		/*奖励道具*/
		public string rewardProps{
			get{
				return vipleveldata.rewardProps;
			}
		}

		public VipLevelItem(int Id, bool b){
			vipleveldata = GMDataBaseSystem.database.SearchId<VipLevelRewardConfigData> (GameData.VipLevelRewardConfig, Id);
			enable = b;
		}
	}

	/*vip特权奖励*/
	public class VipFreeItem
	{
		//VipMonthRewardConfigData vipfreedata;
		bool enable;
		/*礼包id*/
		public int Id{
			get{
				//return vipfreedata.id;
				return 1;
			}
		}

		public bool Enable{
			get{
				return enable;
			}
			set{
				enable = value;
			}
		}

		/*礼包说明*/
		public string Describe{
			get{
				//return vipfreedata.typeDescribe;
				return "";
			}
		}

		/*奖励道具*/
		public string rewardProps{
			get{
				return "";
				//return vipfreedata.rewardProps;
			}
		}

		public VipFreeItem(int Id, bool b){
			//vipfreedata = GMDataBaseSystem.database.SearchId<VipMonthRewardConfigData> (GameData.VipMonthRewardConfig, Id);
			enable = b;
		}
	}

}