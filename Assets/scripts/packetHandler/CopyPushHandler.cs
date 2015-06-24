using UnityEngine;
using System.Collections;

namespace PacketHandler
{
    public class GCPushLevelOpen : IPacketHandler {
        public override void HandlePacket(KBEngine.Packet packet)
        {
            ChuMeng.CopyController.copyController.OpenLev(packet.protoBody as ChuMeng.GCPushLevelOpen);
        }
    }
}
