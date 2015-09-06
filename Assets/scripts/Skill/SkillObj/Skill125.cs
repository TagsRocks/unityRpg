﻿using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class Skill125 : SkillObj 
    {
        private float lastTime = 0;
        public override bool CheckCondition(GameObject owner)
        {
            if(Time.time-lastTime > 5) {
                lastTime = Time.time;
                var target = owner.GetComponent<CommonAI>().targetPlayer;
                if(target != null) {
                    var dis = Util.XZSqrMagnitude(owner.transform.position, target.transform.position);
                    if(dis < 25) {
                        return true;
                    }
                }
            }
            return false;
        }

    }

}