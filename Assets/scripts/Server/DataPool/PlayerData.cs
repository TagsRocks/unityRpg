using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    /// <summary>
    /// Player data InServer
    /// </summary>
    public static class PlayerData
    {
        public static void AddExp(int add)
        {
            var pinfo = ServerData.Instance.playerInfo;
            pinfo.Exp += add;
            var push = GCPushExpChange.CreateBuilder();
            push.Exp = pinfo.Exp;
            ServerBundle.SendImmediatePush(push);
        }

        public static void AddLevel(int add)
        {
            Log.Net("AddLevel "+add);
            var pinfo = ServerData.Instance.playerInfo;
            pinfo.Roles.Level += add;
            AddSkillPoint(1);
            SendNotify("升级技能点+1");

            var push = GCPushLevel.CreateBuilder();
            push.Level = pinfo.Roles.Level;
            ServerBundle.SendImmediatePush(push);
        }

        public static void AddSkillPoint(int sp)
        {
            var pinfo = ServerData.Instance.playerInfo;
            if (!pinfo.HasSkill)
            {
                pinfo.Skill = GCLoadSkillPanel.CreateBuilder().Build();
            }
            pinfo.Skill.TotalPoint += sp;

            var psp = GCPushSkillPoint.CreateBuilder();
            psp.SkillPoint = pinfo.Skill.TotalPoint;
            ServerBundle.SendImmediatePush(psp);
        }

        public static void AddGold(int num)
        {
            var itemData = Util.GetItemData(0, (int)ItemData.ItemID.GOLD);
            var has = ServerData.Instance.playerInfo.Gold;
            SetGold(has + num);
            SendNotify(string.Format("{0}+{1}", itemData.ItemName, num));
        }

        public static void SetGold(int num)
        {
            ServerData.Instance.playerInfo.Gold = num;
            //Notify
            var gc = GoodsCountChange.CreateBuilder();
            gc.Type = 0;
            gc.BaseId = 4;
            gc.Num = num;
            var n = GCPushGoodsCountChange.CreateBuilder();
            n.AddGoodsCountChange(gc);
            ServerBundle.SendImmediatePush(n);
        }

        public static bool IsPackageFull(int itemId, int num)
        {
            var pinfo = ServerData.Instance.playerInfo;
            var itemData = Util.GetItemData(0, itemId);
            if (itemData.UnitType == ItemData.UnitTypeEnum.GOLD)
            {
                return false;
            }

            if (itemData.MaxStackSize > 1)
            {
                Log.Sys("AddItemInStack");
                foreach (var p in pinfo.PackInfoList)
                {
                    if (p.PackEntry.BaseId == itemId)
                    {
                        return false;
                    }
                }
            }

            PackInfo[] packList = new PackInfo[BackPack.MaxBackPackNumber];
            foreach (var p in pinfo.PackInfoList)
            {
                packList [p.PackEntry.Index] = p;
            }

            for (int i = 0; i < BackPack.MaxBackPackNumber; i++)
            {
                if (packList [i] == null)
                {
                    return false;
                }
            }
            return true;
        }
        //Notify
        //Props Stack
        public static void AddItemInPackage(int itemId, int num)
        {
            var pinfo = ServerData.Instance.playerInfo;
            var itemData = Util.GetItemData(0, itemId);
            if (itemData.UnitType == ItemData.UnitTypeEnum.GOLD)
            {
                PlayerData.AddGold(num);
                return;
            }

            //Has Such Objects 
            if (itemData.MaxStackSize > 1)
            {
                Log.Sys("AddItemInStack");
                foreach (var p in pinfo.PackInfoList)
                {
                    if (p.PackEntry.BaseId == itemId)
                    {
                        pinfo.PackInfoList.Remove(p);
                        var newPkinfo = PackInfo.CreateBuilder(p);
                        var newPkEntry = PackEntry.CreateBuilder(p.PackEntry);
                        newPkEntry.Count += num;
                        newPkinfo.PackEntry = newPkEntry.Build();
                        var msg = newPkinfo.Build();
                        pinfo.PackInfoList.Add(msg);


                        var push = GCPushPackInfo.CreateBuilder();
                        push.BackpackAdjust = false;
                        push.PackType = PackType.DEFAULT_PACK;
                        push.PackInfoList.Add(msg);

                        ServerBundle.SendImmediatePush(push);

                        SendNotify(string.Format("{0}+{1}", itemData.ItemName, num));
                        return;
                    }
                }
            }
            //new Item
            //all Slot
            PackInfo[] packList = new PackInfo[BackPack.MaxBackPackNumber];
            int maxId = 0;
            foreach (var p in pinfo.PackInfoList)
            {
                packList [p.PackEntry.Index] = p;
                if (p.PackEntry.Id >= maxId)
                {
                    maxId++;
                }
            }
            if (maxId < 0)
            {
                maxId++;
            }

            for (int i = 0; i < BackPack.MaxBackPackNumber; i++)
            {
                if (packList [i] == null)
                {
                    var pkInfo = PackInfo.CreateBuilder();
                    var pkentry = PackEntry.CreateBuilder();
                    pkInfo.CdTime = 0;

                    pkentry.Id = maxId;
                    pkentry.BaseId = itemId;
                    pkentry.GoodsType = 0;
                    pkentry.Count = num;
                    pkentry.Index = i;

                    pkInfo.PackEntry = pkentry.Build();
                    var msg = pkInfo.Build();
                    pinfo.PackInfoList.Add(msg);

                    var push = GCPushPackInfo.CreateBuilder();
                    push.BackpackAdjust = false;
                    push.PackType = PackType.DEFAULT_PACK;
                    push.PackInfoList.Add(msg);
                    ServerBundle.SendImmediatePush(push);

                    SendNotify(string.Format("{0}+{1}", itemData.ItemName, num));
                    return;
                }
            }

            //PackFull
            var notify = GCPushNotify.CreateBuilder();
            notify.Notify = "背包已满";
            ServerBundle.SendImmediatePush(notify);

        }

        public static bool ReduceItem(int userPropsId)
        {
            var player = ChuMeng.ServerData.Instance.playerInfo;
            foreach (var pinfo in player.PackInfoList)
            {
                if (pinfo.PackEntry.Id == userPropsId)
                {
                    var np = ChuMeng.PackInfo.CreateBuilder(pinfo);
                    var pk = ChuMeng.PackEntry.CreateBuilder(np.PackEntry);
                    pk.Count--;
                    if (pk.Count < 0)
                    {
                        SendNotify("道具数量不足");
                        return false;
                    }
                    var pkMsg = pk.Build();

                    player.PackInfoList.Remove(pinfo);
                    np.SetPackEntry(pkMsg);
                    var npmsg = np.Build();
                    if (pkMsg.Count == 0)
                    {
                    } else
                    {
                        player.AddPackInfo(npmsg);
                    }


                    var push = GCPushPackInfo.CreateBuilder();
                    push.BackpackAdjust = false;
                    push.PackType = PackType.DEFAULT_PACK;
                    push.PackInfoList.Add(npmsg);
                    ServerBundle.SendImmediatePush(push);

                    return true;
                }
            }
            SendNotify("未找到该道具");
            return false;
        }

        public static void  SendNotify(string str)
        {
            var no = GCPushNotify.CreateBuilder();
            no.Notify = str;
            ServerBundle.SendImmediatePush(no);
        }

        public static void LevelUpSkill(int skId)
        {
            var pinfo = ServerData.Instance.playerInfo;
            int totalSP = 0;
            int skillLevel = 0;
            if (pinfo.HasSkill)
            {
                totalSP = pinfo.Skill.TotalPoint;
                foreach (var s in pinfo.Skill.SkillInfosList)
                {
                    if (s.SkillInfoId == skId)
                    {
                        skillLevel = s.Level;
                        break;
                    }
                }
            }
            var skData = Util.GetSkillData(skId, skillLevel);
            var maxLev = skData.MaxLevel;
            var playerLev = pinfo.Roles.Level;
            var next = Util.GetSkillData(skId, skillLevel + 1);
            var needLev = next.LevelRequired;

            if (totalSP > 0 && skillLevel < maxLev && playerLev >= needLev)
            {
                pinfo.Skill.TotalPoint--;
                bool find = false;
                foreach (var sk in pinfo.Skill.SkillInfosList)
                {
                    if (sk.SkillInfoId == skId)
                    {
                        sk.Level++;
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    var newinfo = SkillInfo.CreateBuilder();
                    newinfo.SkillInfoId = skId;
                    newinfo.Level = 1;
                    pinfo.Skill.SkillInfosList.Add(newinfo.Build());
                }

                //pinfo.Skill = newSk.Build();

                var activeSkill = GCPushActivateSkill.CreateBuilder();
                activeSkill.SkillId = skId;
                activeSkill.Level = skillLevel + 1;
                ServerBundle.SendImmediatePush(activeSkill);

            } else if (totalSP <= 0)
            {
                SendNotify("剩余技能点不足");
            } else if (playerLev < needLev)
            {
                SendNotify("等级不足");
            } else
            {
                SendNotify("技能已经升到满级");
            }
            PushSkillShortcutsInfo();
        }
        static void PushSkillShortcutsInfo(){
            var au = GCPushShortcutsInfo.CreateBuilder();
            var pinfo = ServerData.Instance.playerInfo;
            if(pinfo.HasSkill){
                int ind = 0;
                foreach (var sk in pinfo.Skill.SkillInfosList)
                {
                    var sh = ShortCutInfo.CreateBuilder();
                    sh.Index = ind;
                    sh.BaseId = sk.SkillInfoId;
                    au.AddShortCutInfo(sh);
                    ind++;
                }
            }
            ServerBundle.SendImmediatePush(au);
        }


        public static void GetShortCuts(KBEngine.Packet packet)
        {
            var au = GCLoadShortcutsInfo.CreateBuilder();
            var pinfo = ServerData.Instance.playerInfo;
            if (pinfo.HasSkill)
            {
                int ind = 0;
                foreach (var sk in pinfo.Skill.SkillInfosList)
                {
                    var sh = ShortCutInfo.CreateBuilder();
                    sh.Index = ind;
                    sh.BaseId = sk.SkillInfoId;
                    au.AddShortCutInfo(sh);
                    ind++;
                }
            }
            ServerBundle.SendImmediate(au, packet.flowId);
        }

        public static void GetProp(KBEngine.Packet g)
        {
            var pinfo = ServerData.Instance.playerInfo;
            var getChar = g.protoBody as CGGetCharacterInfo;
            var au = GCGetCharacterInfo.CreateBuilder();
            foreach (var k in getChar.ParamKeyList)
            {
                var att = RolesAttributes.CreateBuilder();
                var bd = BasicData.CreateBuilder();
                att.AttrKey = k;
                bd.Indicate = 1;
                if (k == (int)CharAttribute.CharAttributeEnum.LEVEL)
                {
                    bd.TheInt32 = pinfo.Roles.Level; 
                } else if (k == (int)CharAttribute.CharAttributeEnum.EXP)
                {
                    bd.TheInt32 = pinfo.Exp;
                } else if (k == (int)CharAttribute.CharAttributeEnum.GOLD_COIN)
                {
                    bd.TheInt32 = pinfo.Gold;
                }
                att.BasicData = bd.Build();
                au.AddAttributes(att);
            }

            ServerBundle.SendImmediate(au, g.flowId);
        }

        public static void AddProp(KBEngine.Packet p)
        {
            Log.Net("ServerAdddProp");
            var pinfo = ServerData.Instance.playerInfo;
            var inpb = p.protoBody as CGAddProp;
            if (inpb.Key == (int)CharAttribute.CharAttributeEnum.EXP)
            {
                //if (!pinfo.HasExp)
                /*
                {
                    pinfo.Exp = 0;
                }
                */
                pinfo.Exp += inpb.Value; 
            } else if (inpb.Key == (int)CharAttribute.CharAttributeEnum.LEVEL)
            {
                AddLevel(inpb.Value);
            }
        }

        public static void SetProp(KBEngine.Packet p)
        {
            var pinfo = ServerData.Instance.playerInfo;
            var inpb = p.protoBody as CGSetProp;
            if (inpb.Key == (int)CharAttribute.CharAttributeEnum.EXP)
            {
                pinfo.Exp = inpb.Value;
            }
        }

        public static void CreateCharacter(KBEngine.Packet packet)
        {
            var inpb = packet.protoBody as CGCreateCharacter;
            var au = GCCreateCharacter.CreateBuilder();
            var role = RolesInfo.CreateBuilder();
            role.Name = inpb.PlayerName;
            role.PlayerId = 100;
            //playerId++;
            role.Level = 1;
            role.Job = inpb.Job;
            var msg = role.Build();
            ServerData.Instance.playerInfo.Roles = msg;

            var pkinfo = PackInfo.CreateBuilder();
            var pkEntry = PackEntry.CreateBuilder();
            pkEntry.Id = 1; 
            pkEntry.BaseId = 97;
            pkEntry.GoodsType = 1;
            pkinfo.PackEntry = pkEntry.Build();

            var pinfo = ServerData.Instance.playerInfo;
            pinfo.AddDressInfo(pkinfo);

            au.AddRolesInfos(msg);


            ServerBundle.SendImmediate(au, packet.flowId);    
        }

        /// <summary>
        /// First Chapter Lev 
        /// </summary>
        /// <param name="openLev">Open lev.</param>
        public static void PassLev(int openLev)
        {
            var pinfo = ServerData.Instance.playerInfo;
            if (!pinfo.HasCopyInfos)
            {
                var au = GCCopyInfo.CreateBuilder();
                var cin = CopyInfo.CreateBuilder();
                cin.Id = 101;
                cin.IsPass = false;
                au.AddCopyInfo(cin);
                var msg = au.Build();
                pinfo.CopyInfos = msg;
            }
            int levId = 100 + openLev;
            //int levId = -1;
            //int count = -1;
            bool find = false;
            foreach (var c in pinfo.CopyInfos.CopyInfoList)
            {
                if (c.Id == levId)
                {
                    c.IsPass = true;
                    find = true;
                    break;
                }
            }
            if (!find)
            {
                var cinfo = new CopyInfo();
                cinfo.Id = levId;
                cinfo.IsPass = true;
                pinfo.CopyInfos.CopyInfoList.Add(cinfo);
            }
            foreach (var c in pinfo.CopyInfos.CopyInfoList)
            {
                if (c.Id < levId)
                {
                    c.IsPass = true;
                }
            }

            var open = GCPushLevelOpen.CreateBuilder();
            open.Chapter = 1;
            open.Level = openLev;
            ServerBundle.SendImmediatePush(open);

        }

        public static void LoadPackInfo(KBEngine.Packet packet)
        {
            var inpb  = packet.protoBody as CGLoadPackInfo;
            var au = GCLoadPackInfo.CreateBuilder();
            if (inpb.PackType == PackType.DEFAULT_PACK)
            {
                au.PackType = PackType.DEFAULT_PACK;
                var pkInfo = ServerData.Instance.playerInfo.PackInfoList;
                au.AddRangePackInfo(pkInfo);
            } else if(inpb.PackType == PackType.DRESSED_PACK)
            {
                au.PackType = PackType.DRESSED_PACK;
                var pkInfo = ServerData.Instance.playerInfo.DressInfoList;
                au.AddRangePackInfo(pkInfo);
            }
            ServerBundle.SendImmediate(au, packet.flowId);
        }

        public static void GetKeyValue(KBEngine.Packet packet){
            var playerInfo = ServerData.Instance.playerInfo;
            var inpb = packet.protoBody as CGGetKeyValue;
            var key = inpb.Key;
            var get = GCGetKeyValue.CreateBuilder();
            bool find = false;
            foreach(var kv1 in playerInfo.GameStateList){
                if(kv1.Key == key) {
                    get.Value = kv1.Value;
                    find = true;
                    break;
                }
            }
            if(!find){
                get.Value = "";
            }
            ServerBundle.SendImmediate(get, packet.flowId);
        }
        public static void SetKeyValue(KBEngine.Packet packet){
            var playerInfo = ServerData.Instance.playerInfo;
            var inpb = packet.protoBody as CGSetKeyValue;
            bool find = false;
            foreach(var kv in playerInfo.GameStateList) {
                if(kv.Key == inpb.Kv.Key) {
                    kv.Value = inpb.Kv.Value;
                    find = true;
                    break;
                }
            }
            if(!find){
                playerInfo.GameStateList.Add(inpb.Kv);
            }
        }
    }   



}