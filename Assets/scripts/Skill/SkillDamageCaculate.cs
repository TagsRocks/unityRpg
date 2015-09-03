using UnityEngine;
using System.Collections;
namespace ChuMeng {
	public class SkillDamageCaculate : MonoBehaviour {
        public static void DoDamage(GameObject attacker, int WeaponDamagePCT, GameObject enemy) {
            if (enemy.GetComponent<MyAnimationEvent> () != null) {
                var attribute = attacker.GetComponent<NpcAttribute>();
                var rate = 1;
                bool isCritical = false;
                /*
                var rd = Random.Range(0, 100);
                if(rd < attribute.ObjUnitData.CriticalHit) {
                    rate = 2;
                    isCritical = true;
                }
                */
                //在基础攻击力上面提升的比例
                var damage = (int)(attribute.Damage*(1.0f+WeaponDamagePCT/100.0f-1.0f+rate-1.0f));
                Log.Sys("calculate Damage Rate SimpleDamage "+WeaponDamagePCT);
                enemy.GetComponent<MyAnimationEvent> ().OnHit (attacker, damage, isCritical);
                
            }
        }
		//TODO::支持单人副本和多人副本功能 取决于  是否直接通知MyAnimationEvent
		//根据技能信息和玩家信息 得到实际的 伤害  NpcAttribute  SkillFullInfo 
		/*
		伤害计算过程
		1：伤害对象判定  客户端做
		2：伤害数值确定   服务端 或者客户端 
		3：伤害效果施展 例如击退  服务端 或者 客户端
		*/
		public static void DoDamage(GameObject attacker, SkillFullInfo skillData, GameObject enemy) {
			//if (WorldManager.worldManager.sceneType == WorldManager.SceneType.Single) {
			if (enemy.GetComponent<MyAnimationEvent> () != null) {
				var attribute = attacker.GetComponent<NpcAttribute>();
				var rd = Random.Range(0, 100);
				var rate = 1;
				bool isCritical = false;
				if(rd < attribute.ObjUnitData.CriticalHit) {
					rate = 2;
					isCritical = true;
				}
				//在基础攻击力上面提升的比例
				var damage = (int)(attribute.Damage*(1.0f+skillData.skillData.WeaponDamagePCT/100.0f-1.0f+rate-1.0f));
				Log.Sys("calculate Damage Rate "+skillData.skillData.WeaponDamagePCT);
				enemy.GetComponent<MyAnimationEvent> ().OnHit (attacker, damage, isCritical);

			}
			//}
		}
	}

}