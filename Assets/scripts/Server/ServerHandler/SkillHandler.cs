using UnityEngine;
using System.Collections;
using playerData = ChuMeng.PlayerData;

namespace ServerPacketHandler {
    public class CGLoadSkillPanel : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet)
        {
            Log.Sys("LoadSkillPanelData");
            //var inpb = packet.protoBody as ChuMeng.CGLoadSkillPanel;
            var pd = ChuMeng.ServerData.Instance.playerInfo;
            if(pd.HasSkill){
                ChuMeng.ServerBundle.SendImmediate(pd.Skill.ToBuilder(), packet.flowId);
            }else {
                var ret = ChuMeng.GCLoadSkillPanel.CreateBuilder();
                ChuMeng.ServerBundle.SendImmediate(ret, packet.flowId);
            }
        }
    }

    public class CGSkillLevelUp : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet)
        {
            var inpb = packet.protoBody as ChuMeng.CGSkillLevelUp;
            playerData.LevelUpSkill(inpb.SkillId);
        }
    }

    public class CGLoadShortcutsInfo : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet)
        {
            playerData.GetShortCuts(packet);
        }
    }
}
