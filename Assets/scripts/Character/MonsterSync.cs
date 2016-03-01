using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    /// <summary>
    /// 同步服务器上的怪物属性 
    /// </summary>
    public class MonsterSync : MonoBehaviour
    {
        public void DoNetworkDamage(GCPlayerCmd cmd)
        {
            var eid = cmd.DamageInfo.Enemy;
            var attacker = ObjectManager.objectManager.GetPlayer(cmd.DamageInfo.Attacker);
            if(attacker != null) {
                gameObject.GetComponent<MyAnimationEvent>().OnHit(attacker, cmd.DamageInfo.Damage, cmd.DamageInfo.IsCritical);
            }
        }

        public void SyncAttribute(GCPlayerCmd cmd) {
            var info = cmd.EntityInfo;
            var attr = gameObject.GetComponent<NpcAttribute>();
            if(info.HasHP) {
                attr.SetHPNet(info.HP);
            }
        }
    }

}