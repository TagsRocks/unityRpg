
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class AchievementController : MonoBehaviour
	{
		public static AchievementController achievementController;


		List<AchievementItem> achiItem = new List<AchievementItem>();

		void Awake()
		{
			achievementController = this;
			DontDestroyOnLoad (gameObject);
		}


		public IEnumerator LoadAchievenment() {

			foreach (AchieveConfigData ac in GameData.AchieveConfig) {
				var n = new AchievementItem(ac.id);
				achiItem.Add(n);
			}

			var packet = new KBEngine.PacketHolder ();
			var list = CGLoadAchievements.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, list, packet));

			var data = packet.packet.protoBody as GCLoadAchievements;
			foreach (Achievement a in data.AchievementsList) {
				foreach(AchievementItem ac in achiItem){
					if(ac.Id == a.AchievementId){
						ac.State = true;
					}
				}
			}
		}

		//根据类型获取成就数据
		public List<AchievementItem> GetAchievementItem(int t){
			var ret = new List<AchievementItem> ();
			foreach (AchievementItem it in achiItem) {
				if( it.Type	== t ){
					ret.Add(it);
				}		
			}
			return ret;
		}

	}

	public class AchievementItem
	{

		AchieveConfigData achiData;
		bool state;


		/*成就id*/
		public int Id{
			get{
				return achiData.id;
			}
		}

		public int Type{
			get{
				return achiData.achievementType;
			}
		}

		/*成就名称*/
		public string Name{
			get{
				return achiData.achievementName;
			}
		}
	
		/*成就描述*/
		public string Description{
			get{
				return achiData.description;
			}
		}

		//加成
		public string Attribute{
			get{
				return achiData.addAttribute;
			}
		}

		/*属性状态*/
		public bool State{
			get{
				return state;
			}
			set{
				state = value;
			}
		}

		public AchievementItem(int Id){
			achiData = GMDataBaseSystem.SearchIdStatic<AchieveConfigData> (GameData.AchieveConfig, Id);
			state = false;
		}

	}
}