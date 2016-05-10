using UnityEngine;
using System.Collections;

namespace MyLib
{
    /// <summary>
    /// 玩家自己的属性 从服务器上同步 
    /// </summary>
    public class MySelfAttributeSync : MonoBehaviour
    {
        public void NetworkAttribute(AvatarInfo info) {
            var attr = GetComponent<NpcAttribute>();
            Log.Net("MySelfSync: "+info);
            if(info.HasTeamColor) {
                attr.SetTeamColorNet(info.TeamColor);
            }
            if(info.HasIsMaster) {
                attr.SetIsMasterNet(info.IsMaster);
            }
        }

        public void NetworkAttack(SkillAction sk)
        {
            var cmd = new ObjectCommand(ObjectCommand.ENUM_OBJECT_COMMAND.OC_USE_SKILL);
            cmd.skillId = sk.SkillId;
            cmd.skillLevel = sk.SkillLevel;
            cmd.act = sk;
            Log.GUI("Other Player Attack LogicCommand");
            gameObject.GetComponent<LogicCommand>().PushCommand(cmd);
        }

    }
}