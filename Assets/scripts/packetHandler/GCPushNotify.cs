using UnityEngine;
using System.Collections;

namespace PacketHandler
{
    public class GCPushNotify : IPacketHandler 
    {

        public override void HandlePacket (KBEngine.Packet packet)
        {
            var pushGoods = packet.protoBody as ChuMeng.GCPushNotify;
            ChuMeng.WindowMng.windowMng.ShowNotifyLog(pushGoods.Notify);
        }
    }
}
