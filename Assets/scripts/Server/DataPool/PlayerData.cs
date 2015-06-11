using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    /// <summary>
    /// Player data InServer
    /// </summary>
    public static class PlayerData 
    {
        public static void SetGold(int num){
            ServerData.Instance.playerInfo.Gold = num;
            //Notify
            var gc = GoodsCountChange.CreateBuilder();
            gc.Type = 0;
            gc.BaseId = 4;
            gc.Num = num;
            var n = GCPushGoodsCountChange.CreateBuilder();
            n.AddGoodsCountChange(gc);
            ServerBundle.SendImmediate(n);
        }

        //Notify
        //Props Stack
        public static void AddItemInPackage(int itemId)
        {
            var pinfo = ServerData.Instance.playerInfo;

            //Has Such Objects 
            foreach(var p in pinfo.PackInfoList) {
                if(p.PackEntry.BaseId == itemId) {
                    pinfo.PackInfoList.Remove(p);
                    var newPkinfo = PackInfo.CreateBuilder(p);
                    var newPkEntry = PackEntry.CreateBuilder(p.PackEntry);
                    newPkEntry.Count++;
                    newPkinfo.PackEntry = newPkEntry.Build()    ;
                    var msg = newPkinfo.Build();
                    pinfo.PackInfoList.Add(msg);


                    var push = GCPushPackInfo.CreateBuilder();
                    push.BackpackAdjust = false;
                    push.PackType = PackType.DEFAULT_PACK;
                    push.PackInfoList.Add(msg);

                    ServerBundle.SendImmediate(push);
                    return;
                }
            }
            //new Item
            //all Slot
            PackInfo[] packList = new PackInfo[BackPack.MaxBackPackNumber];
            int maxId = 0;
            foreach(var p in pinfo.PackInfoList) {
                packList[p.PackEntry.Index] = p;
                if(p.PackEntry.Id >= maxId) {
                    maxId++;
                }
            }

            for(int i = 0; i < BackPack.MaxBackPackNumber; i++){
                if(packList[i] == null){
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
                    ServerBundle.SendImmediate(push);
                    return;
                }
            }

            //PackFull
            var notify = GCPushNotify.CreateBuilder();
            notify.Notify = "背包已满";
            ServerBundle.SendImmediate(notify);

        }

    }

}