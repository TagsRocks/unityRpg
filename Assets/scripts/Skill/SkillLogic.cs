using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class SkillLogic
    {
        /// <summary>
        /// 释放一个技能状态机 
        /// </summary>
        /// <returns>The skill.</returns>
        /// <param name="attacker">Attacker.</param>
        /// <param name="activeSkill">Active skill.</param>
        /// <param name="position">Position.</param>
        public static IEnumerator  MakeSkill(GameObject attacker, SkillData activeSkill, Vector3 position){
            var skillStateMachine = CreateSkillStateMachine(attacker, activeSkill, position);
            yield return null;
        }

        public static SkillStateMachine CreateSkillStateMachine(GameObject attacker, SkillData activeSkill, Vector3 position, GameObject enemy = null) {
            Log.AI("create Skill State Machine "+activeSkill.SkillName);
            var g = new GameObject ("SkillStateMachine_"+activeSkill.template);
            var skillStateMachine = g.AddComponent<SkillStateMachine> ();
            skillStateMachine.InitPos = position;
            skillStateMachine.transform.parent = ObjectManager.objectManager.transform;
            skillStateMachine.transform.localPosition = Vector3.zero;
            skillStateMachine.transform.localRotation = Quaternion.identity;
            skillStateMachine.transform.localScale = Vector3.one;
            skillStateMachine.attacker = attacker;
            skillStateMachine.skillFullData = new SkillFullInfo(activeSkill);
            skillStateMachine.skillDataConfig = GetSkillInfo(activeSkill);
            skillStateMachine.ownerLocalId = attacker.GetComponent<KBEngine.KBNetworkView> ().GetLocalId ();
			skillStateMachine.target = enemy;
            return skillStateMachine;
        }

        public static SkillDataConfig GetSkillInfo(SkillData activeSkill) {
            Log.AI("active skillName "+activeSkill.SkillName);
            Log.AI ("Get Skill Template is "+activeSkill.template);
            if (activeSkill.template != null)
            {
                var tem =  Resources.Load<SkillDataConfig> ("skills/" + activeSkill.template);
                if(tem == null) {
                    Debug.LogError("NotFind Template "+activeSkill.template);
                }
                return tem;
            }
            return null;
        }

		public static string GetEnemyTag(string tag) {
			string enemyTag;
			if (tag == "Player") {
				enemyTag = "Enemy";
			} else {
				enemyTag = "Player";
			}
			return enemyTag;
		}
		//找到最近的敌人 不考虑朝向方向
		public static GameObject FindNearestEnemy(GameObject attacker) {
			var enemyTag = SkillLogic.GetEnemyTag (attacker.tag);
			LayerMask mask = SkillDamageCaculate.GetDamageLayer();
			var enemies = Physics.OverlapSphere (attacker.transform.position, attacker.GetComponent<NpcAttribute>().AttackRange, mask);
			float minDist = 999999;
			
			GameObject enemy = null;
			var transform = attacker.transform;
			
			foreach (var ene in enemies) {
				if(ene.tag == enemyTag) {
					var d = (ene.transform.position - transform.position).sqrMagnitude;
					if (d < minDist) {
						minDist = d;
						enemy = ene.gameObject;
					}   
					
				}
			}
			
			return enemy;
		}

    }

}