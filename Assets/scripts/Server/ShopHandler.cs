using UnityEngine;
using System.Collections;
using playerData = ChuMeng.PlayerData;

namespace ServerPacketHandler {
    public class CGBuyShopProps : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet)
        {
            Log.Net("ShopHandler");
            var buy = packet.protoBody as ChuMeng.CGBuyShopProps;
            var itemId = buy.ShopId;
            var data = ChuMeng.GMDataBaseSystem.SearchIdStatic<ChuMeng.PropsConfigData>(ChuMeng.GameData.PropsConfig, itemId);
            var player = ChuMeng.ServerData.Instance.playerInfo;
            var has = player.Gold;
            //var playerData = ChuMeng.PlayerData;
            if(has < data.goldCoin) {
                var notify = ChuMeng.GCPushNotify.CreateBuilder();
                notify.SetNotify("金币不足");
                ChuMeng.ServerBundle.SendImmediate(notify);
            }else {
                playerData.SetGold(has-data.goldCoin);
                playerData.AddItemInPackage(itemId);
            }

        }
    }
}
