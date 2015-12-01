
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/

/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

using System;
using KBEngine;

namespace ChuMeng
{

    /// <summary>
    /// 网络对象的本地代理
    /// Proxy 接受网络同步 
    /// </summary>
	public class PlayerSync : KBEngine.MonoBehaviour
	{
		/*
		 * Write Message Send To Server
		 * PlayerManagerment  PhotonView Manager 
		 */ 
        public void NetworkMove(AvatarInfo info) {
            var mvTarget = new Vector3(info.X/100.0f, info.Y/100.0f+0.2f, info.Z/100.0f);
            var cmd = new ObjectCommand();
            cmd.targetPos = mvTarget;
            cmd.dir = info.Dir;
            cmd.commandID = ObjectCommand.ENUM_OBJECT_COMMAND.OC_MOVE;
            GetComponent<LogicCommand>().PushCommand(cmd);
            if(info.HasHP) {
                GetComponent<NpcAttribute>().SetHPNet(info.HP);    
            }
        }

        public void NetworkAttack(SkillAction sk) {
            var cmd = new ObjectCommand (ObjectCommand.ENUM_OBJECT_COMMAND.OC_USE_SKILL);
            cmd.skillId = sk.SkillId;
            Log.GUI ("Other Player Attack LogicCommand");
            gameObject.GetComponent<LogicCommand> ().PushCommand (cmd);
        }

        public void SetLevel(AvatarInfo info) {
            GetComponent<NpcAttribute>().ChangeLevel(info.Level);
        }

        public void SetPositionAndDir(AvatarInfo info) {
            Vector3 vxz = new Vector3(info.X/100.0f, info.Y/100.0f+0.2f, info.Z/100.0f);
            Log.Sys("SetPosition: "+info+" vxz "+vxz+" n "+gameObject.name);
            transform.position = new Vector3(vxz.x, vxz.y, vxz.y);
            transform.rotation = Quaternion.Euler (new Vector3(0, info.Dir, 0));
            StartCoroutine(SetPos(vxz));
        }
        /// <summary>
        /// 稳定一下初始化位置 
        /// </summary>
        /// <returns>The position.</returns>
        /// <param name="p">P.</param>
        IEnumerator SetPos(Vector3 p) {
            var c = 0;
            while(c <= 3) {
                transform.position = p;
                c++;
                yield return null;
            }
        }
	

        /// <summary>
        /// 本地控制对象接受网络命令
        /// 本地代理接受网络命令
        /// </summary>
        /// <param name="cmd">Cmd.</param>
        public void DoNetworkDamage(GCPlayerCmd cmd)
        {
            var eid = cmd.DamageInfo.Enemy;
            var attacker = ObjectManager.objectManager.GetPlayer(cmd.DamageInfo.Attacker);
            if(attacker != null) {
                gameObject.GetComponent<MyAnimationEvent>().OnHit(attacker, cmd.DamageInfo.Damage, cmd.DamageInfo.IsCritical);
            }
        }

        public void NetworkBuff(GCPlayerCmd cmd) {
            var attacker = ObjectManager.objectManager.GetPlayer(cmd.BuffInfo.Attacker);
            if(attacker != null) {
                var sk = Util.GetSkillData(cmd.BuffInfo.SkillId, 1);
                var skConfig = SkillLogic.GetSkillInfo(sk);
                var evt = skConfig.GetEvent(cmd.BuffInfo.EventId);
                if(evt != null) {
                    gameObject.GetComponent<BuffComponent>().AddBuff(evt.affix, attacker);
                }
            }
        }
	}

}