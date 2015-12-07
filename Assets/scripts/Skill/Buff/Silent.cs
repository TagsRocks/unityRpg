using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class Silent : IEffect 
    {
        public override bool CanUseSkill()
        {
            return false;
        }

    }

}