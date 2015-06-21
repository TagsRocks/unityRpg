using UnityEngine;
using System.Collections;

namespace PacketHandler
{
    public class GCPushSkillPoint  : IPacketHandler 
    {
        public override void HandlePacket(KBEngine.Packet packet) {
            ChuMeng.GameInterface_Skill.UpdateSkillPoint(packet.protoBody as ChuMeng.GCPushSkillPoint);
        }

    }

    public class GCPushLevel : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet) {
            ChuMeng.GameInterface_Skill.UpdateLevel(packet.protoBody as ChuMeng.GCPushLevel);
        }
    }
}