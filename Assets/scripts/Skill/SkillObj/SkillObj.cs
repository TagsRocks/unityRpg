using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class SkillObj
    {
        public virtual bool CheckCondition(GameObject owner){
            return true;
        }
    }

}