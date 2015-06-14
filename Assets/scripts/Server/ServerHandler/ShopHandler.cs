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
                ChuMeng.ServerBundle.SendImmediatePush(notify);
            }else {
                playerData.SetGold(has-data.goldCoin);
                playerData.AddItemInPackage(itemId);
            }

        }
    }
    public class CGUseUserProps : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet)
        {
            var inpb = packet.protoBody as ChuMeng.CGUseUserProps;
            var ret = playerData.ReduceItem((int)inpb.UserPropsId);
            var pb = ChuMeng.GCUseUserProps.CreateBuilder();
            if(ret) {
                ChuMeng.ServerBundle.SendImmediate(pb, packet.flowId);
            }else {
                ChuMeng.ServerBundle.SendImmediateError(pb, packet.flowId, 1);
            }
        }
    }
}
