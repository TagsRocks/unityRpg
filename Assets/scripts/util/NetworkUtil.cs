using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public static class NetworkUtil
    {
        public static bool IsNet()
        {
            var world = WorldManager.worldManager.GetActive();
            return world.IsNet;
        }

        public static bool IsNetMaster()
        {
            var world = WorldManager.worldManager.GetActive();
            var player = ObjectManager.objectManager.GetMyAttr();
            return world.IsNet && player.IsMaster;
        }

        public static bool IsMaster()
        {
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
        public static Vector3 FloatPos(int x, int y, int z) {
            return new Vector3(x/100.0f, y/100.0f, z/100.0f);
        }

        public static void Broadcast(CGPlayerCmd.Builder cmd)
        {
            var scene = WorldManager.worldManager.GetActive();
            scene.BroadcastMsg(cmd);
        }

        public static void RemoveEntityToNetwork(KBEngine.KBNetworkView view) {
            var cg = CGPlayerCmd.CreateBuilder();
            var ety = EntityInfo.CreateBuilder();
            ety.Id = view.GetServerID();
            cg.EntityInfo = ety.Build();
            cg.Cmd = "RemoveEntity";
            NetworkUtil.Broadcast(cg);
        }
    }
}