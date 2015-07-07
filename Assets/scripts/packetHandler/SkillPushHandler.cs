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

    public class GCPushExpChange : IPacketHandler
    {
        public override void HandlePacket(KBEngine.Packet packet)
        {
            ChuMeng.GameInterface_Player.UpdateExp(packet.protoBody as ChuMeng.GCPushExpChange);
        }
    }
    public class GCPushShortcutsInfo : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet)
        {
            ChuMeng.GameInterface_Skill.UpdateShortcutsInfo(packet.protoBody as ChuMeng.GCPushShortcutsInfo);
        }
    }
}