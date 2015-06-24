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

            if(cmds[0] == "add_gold") {
                playerData.AddGold(System.Convert.ToInt32(cmds[1]));
            }else if(cmds[0] == "add_sp") {
                playerData.AddSkillPoint(System.Convert.ToInt32(cmds[1]));
            }else if(cmds[0] == "add_lvl") {
                playerData.AddLevel(System.Convert.ToInt32(cmds[1]));
            }else if(cmds[0] == "add_exp") {
                playerData.AddExp(System.Convert.ToInt32(cmds[1]));
            }else if(cmds[0] == "pass_lev"){
                playerData.PassLev(System.Convert.ToInt32(cmds[1]));
            }
            /*
            if (inpb.Content == "getAllWeapon")
            {
                Debug.Log("getAllWeapon ");
                    
                int c = 0;
                var pk = new JSONArray();
                var pushPack = GCPushPackInfo.CreateBuilder();
                pushPack.PackType = PackType.DEFAULT_PACK;
                pushPack.BackpackAdjust = true;
                foreach (EquipConfigData ed in GameData.EquipConfig)
                {
                    if (ed.job == selectPlayerJob && ed.equipPosition == 8)
                    {
                        var pinfo = PackInfo.CreateBuilder();
                        var pkentry = PackEntry.CreateBuilder();
                        pkentry.Id = ed.id;
                        pkentry.BaseId = ed.id;
                        pkentry.Index = c;
                        pinfo.CdTime = 0;
                        pkentry.GoodsType = 1;
                        pinfo.PackEntry = pkentry.BuildPartial();
                        pushPack.AddPackInfo(pinfo);
                        var dict = new JSONClass();
                        dict ["id"].AsInt = ed.id;
                        dict ["baseId"].AsInt = ed.id;
                        dict ["index"].AsInt = c;
                        dict ["goodsType"].AsInt = 1;
                        pk.Add(dict);
                        c++;
                    }
                }
                    
                packInf = pk.ToString();
                SendPacket(pushPack, 0);
            } else if (inpb.Content == "getAllEquip")
            {
                Debug.Log("getAllEquip ");
                    
                int c = 0;
                var pk = new JSONArray();
                var pushPack = GCPushPackInfo.CreateBuilder();
                pushPack.PackType = PackType.DEFAULT_PACK;
                pushPack.BackpackAdjust = true;
                foreach (EquipConfigData ed in GameData.EquipConfig)
                {
                    if (ed.job == selectPlayerJob && ed.equipPosition != 8)
                    {
                        var pinfo = PackInfo.CreateBuilder();
                        var pkentry = PackEntry.CreateBuilder();
                        pkentry.Id = ed.id;
                        pkentry.BaseId = ed.id;
                        pkentry.Index = c;
                        pinfo.CdTime = 0;
                        pkentry.GoodsType = 1;
                        pinfo.PackEntry = pkentry.BuildPartial();
                        pushPack.AddPackInfo(pinfo);
                        var dict = new JSONClass();
                        dict ["id"].AsInt = ed.id;
                        dict ["baseId"].AsInt = ed.id;
                        dict ["index"].AsInt = c;
                        dict ["goodsType"].AsInt = 1;
                        pk.Add(dict);
                        c++;
                    }
                }
                    
                packInf = pk.ToString();
                SendPacket(pushPack, 0);

            }
            */                
                

        }

    }
}
