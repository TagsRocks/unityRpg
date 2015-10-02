using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    /// <summary>
    ///嗜血狼技能 
    /// </summary>
    public class Skill133 : SkillObj
    {
        float lastTime = 0;
        bool useYet = false;
        void Awake() {
            GetComponent<MyAnimationEvent>().AddCallBackLocalEvent(MyEvent.EventType.HitTarget, HitTarget);
        }
        int hitNum = 0;
        void HitTarget(MyEvent evt) {
            hitNum++;
        }

        /// <summary>
        /// 远离攻击目标 
        /// </summary>
        /// <returns><c>true</c>, if condition was checked, <c>false</c> otherwise.</returns>
        /// <param name="owner">Owner.</param>
        public override bool CheckCondition(GameObject owner)
        {
            if (Time.time - lastTime > 3)
            {
                if(hitNum >= 1 && !useYet){
                    useYet = true;
                    return true;
                }
            }
            return false;
        }

       
    }
}
