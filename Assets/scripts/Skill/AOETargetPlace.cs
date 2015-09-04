using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    /// <summary>
    /// 在目标位置产生一个AOE技能伤害目标位置附近单位，类似于子弹飞射，最后目标位置碰撞爆炸 
    /// </summary>
    public class AOETargetPlace : MonoBehaviour
    {
        public GameObject DieParticle;
        public float AOERadius;
        string enemyTag;

        SkillLayoutRunner runner;
        // Use this for initialization
        void Start()
        {
            runner = transform.parent.GetComponent<SkillLayoutRunner>();
            var attacker = runner.stateMachine.attacker; 
            enemyTag = SkillLogic.GetEnemyTag (attacker.tag);
            StartCoroutine(WaitExplosive());
        }

        IEnumerator WaitExplosive() {
            yield return new WaitForSeconds(0.5f);
            if(runner.BeamTargetPos != Vector3.zero){
                transform.position = runner.BeamTargetPos;
                if(DieParticle != null) {
                    GameObject g = Instantiate (DieParticle) as GameObject;
                    NGUITools.AddMissingComponent<RemoveSelf> (g);
                    g.transform.parent = ObjectManager.objectManager.transform;
                    g.transform.position = runner.BeamTargetPos;
                }

                Collider[] col = Physics.OverlapSphere (transform.position, AOERadius, SkillDamageCaculate.GetDamageLayer());
                foreach (Collider c in col) {
                    DoDamage (c);
                }
            }else {
                Debug.LogError("BeamTargetPos ");
            }
        }

        /// <summary>
        ///子弹伤害计算也交给skillLayoutRunner执行 
        /// </summary>
        /// <param name="other">Other.</param>
        void DoDamage(Collider other){
            if (other.tag == enemyTag) {
                var skillData = runner.stateMachine.skillFullData.skillData;
                if(!string.IsNullOrEmpty(skillData.HitSound)) {
                    BackgroundSound.Instance.PlayEffect(skillData.HitSound);
                }
                if(runner != null) {
                    runner.DoDamage(other.gameObject);
                }
            }
        }
       
    }
}
