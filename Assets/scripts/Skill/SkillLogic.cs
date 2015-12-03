using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public static class SkillLogic
    {
        /// <summary>
        /// 释放一个技能状态机 
        /// </summary>
        /// <returns>The skill.</returns>
        /// <param name="attacker">Attacker.</param>
        /// <param name="activeSkill">Active skill.</param>
        /// <param name="position">Position.</param>
        public static IEnumerator  MakeSkill(GameObject attacker, SkillData activeSkill, Vector3 position)
        {
            var skillStateMachine = CreateSkillStateMachine(attacker, activeSkill, position);
            yield return null;
        }

        public static SkillStateMachine CreateSkillStateMachine(GameObject attacker, SkillData activeSkill, Vector3 position, GameObject enemy = null)
        {
            Log.AI("create Skill State Machine " + activeSkill.SkillName);
            var g = new GameObject("SkillStateMachine_" + activeSkill.template);
            var skillStateMachine = g.AddComponent<SkillStateMachine>();
            skillStateMachine.InitPos = position;
            skillStateMachine.transform.parent = ObjectManager.objectManager.transform;
            skillStateMachine.transform.localPosition = Vector3.zero;
            skillStateMachine.transform.localRotation = Quaternion.identity;
            skillStateMachine.transform.localScale = Vector3.one;
            skillStateMachine.attacker = attacker;
            skillStateMachine.skillFullData = new SkillFullInfo(activeSkill);
            skillStateMachine.skillDataConfig = GetSkillInfo(activeSkill);
            skillStateMachine.ownerLocalId = attacker.GetComponent<KBEngine.KBNetworkView>().GetLocalId();
            skillStateMachine.target = enemy;
            return skillStateMachine;
        }

        private static Dictionary<string, GameObject> skillConfigCache = new Dictionary<string, GameObject>();

        public static SkillDataConfig GetSkillInfo(SkillData activeSkill)
        {
            Log.AI("active skillName " + activeSkill.SkillName);
            Log.AI("Get Skill Template is " + activeSkill.template);
            if (activeSkill.template != null)
            {
                if (!skillConfigCache.ContainsKey(activeSkill.template) || skillConfigCache[activeSkill.template] == null)
                {
                    var tem = Resources.Load<GameObject>("skills/" + activeSkill.template);
                    if (tem == null)
                    {
                        Debug.LogError("NotFind Template " + activeSkill.template);
                        return null;
                    }else {
                        //切换场景不要摧毁对象
                        var go = GameObject.Instantiate(tem) as GameObject;
                        GameObject.DontDestroyOnLoad(go);
                        skillConfigCache.Add(activeSkill.template, go);
                    }
                }
                return skillConfigCache[activeSkill.template].GetComponent<SkillDataConfig>();
            }
            return null;
        }

        private static string GetEnemyTag(string tag)
        {
            string enemyTag;
            if (tag == "Player")
            {
                enemyTag = "Enemy";
            } else
            {
                enemyTag = "Player";
            }
            return enemyTag;
        }
        //找到最近的敌人 不考虑朝向方向
        public static GameObject FindNearestEnemy(GameObject attacker)
        {
            LayerMask mask = SkillDamageCaculate.GetDamageLayer();
            var enemies = Physics.OverlapSphere(attacker.transform.position, attacker.GetComponent<NpcAttribute>().AttackRange, mask);
            float minDist = 999999;
			
            GameObject enemy = null;
            var transform = attacker.transform;
			
            foreach (var ene in enemies)
            {
                if (IsEnemy(attacker, ene.gameObject))
                {
                    var d = (ene.transform.position - transform.position).sqrMagnitude;
                    if (d < minDist)
                    {
                        minDist = d;
                        enemy = ene.gameObject;
                    }   
					
                }
            }
			
            return enemy;
        }

        /// <summary>
        /// 首先判断场景模式：
        ///     普通场景
        ///     网络场景 
        ///    
        /// 
        /// </summary>
        /// <returns><c>true</c> if is enemy the specified a b; otherwise, <c>false</c>.</returns>
        /// <param name="a">The alpha component.</param>
        /// <param name="b">The blue component.</param>
        public static bool IsEnemy(GameObject a, GameObject b)
        {
            var scene = WorldManager.worldManager.GetActive();
            if (scene.IsNet)
            {
                if (a != b)
                {
                    return true;
                }
            }
            var enemyTag = SkillLogic.GetEnemyTag(a.tag);
            if (b.tag == enemyTag)
            {
                return true;
            }
            return false;
        }

    }

}