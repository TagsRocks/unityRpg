using UnityEngine;
using System.Collections;

namespace MyLib
{
    public class TankShoot : AIState
    {
        private SkillFullInfo activeSkill;

        public override IEnumerator RunLogic()
        {
            activeSkill = GetAttr().GetComponent<SkillInfoComponent>().GetActiveSkill();
            yield return GetAttr().StartCoroutine(Shoot());
        }

        private string GetAttackAniName()
        {
            var name = string.Format("rslash_{0}", 1);
            return name;
        }

        private string attackAniName;

        IEnumerator Shoot()
        {
            var trans = GetAttr().transform;
            var enemy = SkillLogic.FindNearestEnemy(trans.gameObject);
            var physic = GetAttr().GetComponent<TankPhysicComponent>();
            Log.Sys("FindEnemyIs: "+enemy);
            if(enemy != null) {
                var dir = enemy.transform.position- trans.position;
                dir.y = 0;
                Log.Sys("EnemyIs: "+dir);
                //physic.TurnToImmediately(dir);
                physic.TurnTower(dir);
            }

            attackAniName = GetAttackAniName(); 
            /*
            var realAttackTime = activeSkill.skillData.AttackAniTime / GetAttr().GetSpeedCoff();
            var rate = GetAttr().animation [attackAniName].length / realAttackTime;
            PlayAni(attackAniName, rate, WrapMode.Once);
            */
            yield return GetAttr().StartCoroutine(WaitForAttackAnimation(GetAttr().animation));
            yield return new WaitForSeconds(0.1f);
        }

        private IEnumerator WaitForAttackAnimation(Animation animation)
        {
            //var rd = Random.Range(1, 3);
            //BackgroundSound.Instance.PlayEffect("onehandswinglarge" + rd);
            var skillStateMachine = SkillLogic.CreateSkillStateMachine(GetAttr().gameObject, activeSkill.skillData, GetAttr().transform.position);
            Log.AI("Wait For Combat Animation");
            float passTime = 0;
            var realAttackTime = activeSkill.skillData.AttackAniTime / GetAttr().GetSpeedCoff();
            do
            {
                if (passTime >= realAttackTime * 0.8f)
                {
                    break;
                }
                passTime += Time.deltaTime;

                yield return null;
            } while(!quit);
            
            Log.Ani("Animation is Playing stop ");
            skillStateMachine.Stop();
        }
    }
}