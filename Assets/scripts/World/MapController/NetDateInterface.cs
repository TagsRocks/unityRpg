using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public static class NetDateInterface
    {

        public static void FastMoveAndPos() {
            var me = ObjectManager.objectManager.GetMyPlayer();
            if (me == null)
            {
                return;
            }
            var pos = me.transform.position;
            var dir = (int)me.transform.localRotation.eulerAngles.y;

            var cg = CGPlayerCmd.CreateBuilder();
            cg.Cmd = "Move";
            var ainfo = AvatarInfo.CreateBuilder();
            ainfo.X = (int)(pos.x*100);
            ainfo.Z = (int)(pos.z*100);
            ainfo.Y = (int)(pos.y*100);
            ainfo.Dir = dir;
            cg.AvatarInfo = ainfo.Build();

            var s = WorldManager.worldManager.GetActive();
            s.BroadcastMsg(cg);
        }

        public static void SyncPosAndDir()
        {
            var me = ObjectManager.objectManager.GetMyPlayer();
            if (me == null)
            {
                return;
            }
            var pos = me.transform.position;
            var dir = (int)me.transform.localRotation.eulerAngles.y;

            var cg = CGPlayerCmd.CreateBuilder();
            cg.Cmd = "UpdateData";
            var ainfo = AvatarInfo.CreateBuilder();
            ainfo.X = (int)(pos.x*100);
            ainfo.Z = (int)(pos.z*100);
            ainfo.Y = (int)(pos.y*100);
            ainfo.Dir = dir;
            cg.AvatarInfo = ainfo.Build();

            var s = WorldManager.worldManager.GetActive();
            s.BroadcastMsg(cg);
        }
    }
}
