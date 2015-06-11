
/*
Author: QiuChell
Email: 122595579@qq.com
*/
using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	public class GameInterface_Knightage 
	{
		public static GameInterface_Knightage instance = new GameInterface_Knightage();
	
		public static GameInterface_Knightage GetInstance()
		{
			if (instance == null)
			{
				instance = new GameInterface_Knightage();
			}
			return instance;
		}


		

	}
	public class GuildConfigInfo
	{
		GuildConfigData guildData;
		
		//升下一级所需资源
		public int UpgradeResourse{
			get{
				return guildData.upgradeResourse;
			}
		}
		
		//维护消耗木材
		public int MaintainExpend{
			get{
				return guildData.maintainExpend;
			}
		}
		
		//讲武堂等级
		public int WuGuanLevel{
			get{
				return guildData.wuGuanLevel;
			}
		}
		
		//成员上限
		public int MemberLimit{
			get{
				return guildData.memberLimit;
			}
		}
		
		//Id  公会等级id
		public GuildConfigInfo(int Id){
			guildData = GMDataBaseSystem.SearchIdStatic<GuildConfigData> (GameData.GuildConfig, Id);
		}
	}
}
