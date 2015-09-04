using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class Skill123 : SkillObj
    {
        float lastTime = 0;

        public override bool CheckCondition(GameObject owner)
        {
            if (Time.time - lastTime > 3)
            {
                lastTime = Time.time;
                var target = owner.GetComponent<CommonAI>().targetPlayer;
                if (target != null)
                {
                    var diff = Util.XZSqrMagnitude(owner.transform.position, target.transform.position);
                    Log.Sys("Skill117 diff " + diff);
                    if (diff < 36)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

     
    }
}
