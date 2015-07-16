using UnityEngine;
using System.Collections;

using playerData = ChuMeng.PlayerData;

namespace ServerPacketHandler
{
    public class CGSendChat : IPacketHandler
    {
        public override void HandlePacket(KBEngine.Packet packet)
        {
            var inpb = packet.protoBody as ChuMeng.CGSendChat;

            var push = ChuMeng.GCPushChat2Client.CreateBuilder();
            push.PlayerId = 1;
            push.PlayerName = "You";
            push.PlayerLevel = 1;
            push.PlayerJob = 1;
            push.PlayerVip = 1;
            push.TargetId = 3;
            push.Channel = 0;
            push.ChatContent = inpb.Content;
            ChuMeng.ServerBundle.SendImmediatePush(push);
             
            var cmds = inpb.Content.Split(char.Parse(" "));

            try
            {
                if (cmds [0] == "add_gold")
                {
                    playerData.AddGold(System.Convert.ToInt32(cmds [1]));
                } else if (cmds [0] == "add_sp")
                {
                    playerData.AddSkillPoint(System.Convert.ToInt32(cmds [1]));
                } else if (cmds [0] == "add_lvl")
                {
                    playerData.AddLevel(System.Convert.ToInt32(cmds [1]));
                } else if (cmds [0] == "add_exp")
                {
                    playerData.AddExp(System.Convert.ToInt32(cmds [1]));
                } else if (cmds [0] == "pass_lev")
                {
                    playerData.PassLev(System.Convert.ToInt32(cmds [1]));
                } else if (cmds [0] == "kill_all")
                {
                }
            } catch (System.Exception e)
            {
                Log.Critical("ServerException "+e);
            }
                

        }

    }
}
