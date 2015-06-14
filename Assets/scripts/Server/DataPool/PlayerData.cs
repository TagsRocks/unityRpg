using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    /// <summary>
    /// Player data InServer
    /// </summary>
    public static class PlayerData
    {
        public static void AddGold(int num)
        {
            var has = ServerData.Instance.playerInfo.Gold;
            SetGold(has + num);
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

        //Notify
        //Props Stack
        public static void AddItemInPackage(int itemId)
        {
            var pinfo = ServerData.Instance.playerInfo;

            //Has Such Objects 
            foreach (var p in pinfo.PackInfoList)
            {
                if (p.PackEntry.BaseId == itemId)
                {
                    pinfo.PackInfoList.Remove(p);
                    var newPkinfo = PackInfo.CreateBuilder(p);
                    var newPkEntry = PackEntry.CreateBuilder(p.PackEntry);
                    newPkEntry.Count++;
                    newPkinfo.PackEntry = newPkEntry.Build();
                    var msg = newPkinfo.Build();
                    pinfo.PackInfoList.Add(msg);


                    var push = GCPushPackInfo.CreateBuilder();
                    push.BackpackAdjust = false;
                    push.PackType = PackType.DEFAULT_PACK;
                    push.PackInfoList.Add(msg);

                    ServerBundle.SendImmediatePush(push);
                    return;
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
            if(maxId < 0){
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
                    pkentry.Count = 1;
                    pkentry.Index = i;

                    pkInfo.PackEntry = pkentry.Build();
                    var msg = pkInfo.Build();
                    pinfo.PackInfoList.Add(msg);

                    var push = GCPushPackInfo.CreateBuilder();
                    push.BackpackAdjust = false;
                    push.PackType = PackType.DEFAULT_PACK;
                    push.PackInfoList.Add(msg);
                    ServerBundle.SendImmediatePush(push);
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
                    if(pk.Count < 0) {
                        SendNotify("道具数量不足");
                        return false;
                    }

                    player.PackInfoList.Remove(pinfo);
                    np.SetPackEntry(pk);
                    if(pk.Count == 0) {
                    }else {
                        player.AddPackInfo(np);
                    }


                    var push = GCPushPackInfo.CreateBuilder();
                    push.BackpackAdjust = false;
                    push.PackType = PackType.DEFAULT_PACK;
                    push.PackInfoList.Add(np.Build());
                    ServerBundle.SendImmediatePush(push);

                    return true;
                }
            }
            SendNotify("未找到该道具");
            return false;
        }

        public static void  SendNotify(string str){
            var no = GCPushNotify.CreateBuilder();
            no.Notify = str;
            ServerBundle.SendImmediatePush(no);
        }
    }

}