using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public static class NetworkUtil
    {
        public static bool IsNetMaster()
        {
            var world = WorldManager.worldManager.GetActive();
            var player = ObjectManager.objectManager.GetMyAttr();
            return world.IsNet && player.IsMaster;
        }
        public static bool IsMaster() {
            var player = ObjectManager.objectManager.GetMyAttr();
            return player.IsMaster;
        }

        public static int[] ConvertPos(Vector3 pos)
        {
            var ret = new int[3];
            ret [0] = (int)(pos.x * 100);
            ret [1] = (int)(pos.y * 100);
            ret [2] = (int)(pos.z * 100);
            return ret;
        }
    }
}