using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng{

	public class GameInterface_Achievement{

		public static GameInterface_Achievement AchievementInterface = new GameInterface_Achievement();


		public List<AchievementItem> GetAchievement(int type){
			return AchievementController.achievementController.GetAchievementItem(type);
		}
		
	}
}
