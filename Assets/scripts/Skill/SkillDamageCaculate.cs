﻿using UnityEngine;
using System.Collections;
namespace ChuMeng {
	public class SkillDamageCaculate {
        public static void DoDamage(GameObject attacker, int WeaponDamagePCT, GameObject enemy) {
            if (enemy.GetComponent<MyAnimationEvent> () != null) {
                var attribute = attacker.GetComponent<NpcAttribute>();
                var rate = 1;
                bool isCritical = false;

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
			if (enemy.GetComponent<MyAnimationEvent> () != null) {
				var attribute = attacker.GetComponent<NpcAttribute>();
				var rd = Random.Range(0, 100);
				var rate = 1;
				bool isCritical = false;
                if(rd < attribute.GetCriticalRate()) {
					rate = 2;
					isCritical = true;
				}
				//在基础攻击力上面提升的比例
				var damage = (int)(attribute.Damage*(1.0f+skillData.skillData.WeaponDamagePCT/100.0f-1.0f+rate-1.0f));
				Log.Sys("calculate Damage Rate "+skillData.skillData.WeaponDamagePCT);
                if(attribute.IsMe()) {
                    var cg = CGPlayerCmd.CreateBuilder();
                    var dinfo = DamageInfo.CreateBuilder();

                    dinfo.Attacker = attribute.GetComponent<KBEngine.KBNetworkView>().GetServerID();
                    dinfo.Enemy = enemy.GetComponent<KBEngine.KBNetworkView>().GetServerID();
                    dinfo.Damage = damage;
                    dinfo.IsCritical = isCritical;
                    cg.DamageInfo = dinfo.Build();
                    cg.Cmd = "Damage";
                    WorldManager.worldManager.GetActive().BroadcastMsg(cg);
                }

                enemy.GetComponent<MyAnimationEvent> ().OnHit (attacker, damage, isCritical);

                MyEventSystem.myEventSystem.PushLocalEvent(attribute.GetLocalId(), MyEvent.EventType.HitTarget);

			}
		}
        public static void DoNetworkDamage(GCPlayerCmd cmd) {
            var eid = cmd.DamageInfo.Enemy;
            var attacker = ObjectManager.objectManager.GetPlayer(cmd.DamageInfo.Attacker);
            //发起方忽略掉网络
            if(attacker != null && attacker.GetComponent<NpcAttribute>().IsMe()) {
                return;
            }


            var enemy = ObjectManager.objectManager.GetPlayer(eid);
            if(enemy != null) {
                enemy.GetComponent<MyAnimationEvent>().OnHit(attacker, cmd.DamageInfo.Damage, cmd.DamageInfo.IsCritical);
            }
            
        }

        /// <summary>
        /// 得到伤害计算层 
        /// </summary>
        /// <returns>The damage layer.</returns>
        public static int GetDamageLayer() {
            return 1 << (int)GameLayer.Npc | 1 << (int) GameLayer.IgnoreCollision2;
        }
	}

}