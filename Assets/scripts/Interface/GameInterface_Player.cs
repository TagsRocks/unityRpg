using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public static class GameInterface_Player
    {
        public static int GetLevel()
        {
            return ObjectManager.objectManager.GetMyData().GetProp(CharAttribute.CharAttributeEnum.LEVEL);
        }

        public static void UpdateExp(GCPushExpChange p){
            ObjectManager.objectManager.GetMyAttr().SetExp(p.Exp);
        }
    }

}