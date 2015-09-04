using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class SkillObj : MonoBehaviour
    {
        public virtual bool CheckCondition(GameObject owner){
            return true;
        }
    }

}