using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class MonsterSyncToServer : MonoBehaviour
    {
        EntityInfo lastInfo;
        EntityInfo.Builder info;
        public void SyncToServer() {
            var me = gameObject;
            var pos = me.transform.position;
            var dir = (int)me.transform.localRotation.eulerAngles.y;
            var meAttr = me.GetComponent<NpcAttribute>();

            var cg = CGPlayerCmd.CreateBuilder();
            cg.Cmd = "UpdateEntityData";
            var ainfo = EntityInfo.CreateBuilder();
            ainfo.Id = meAttr.GetNetView().GetServerID();
            ainfo.X = (int)(pos.x * 100);
            ainfo.Z = (int)(pos.z * 100);
            ainfo.Y = (int)(pos.y * 100);
            ainfo.HP = meAttr.HP;

            cg.EntityInfo = ainfo.Build();
            var s = WorldManager.worldManager.GetActive();
            s.BroadcastMsg(cg);
        }
    }
}