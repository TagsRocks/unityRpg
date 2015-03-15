
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace PacketHandler
{
	public class GCPushMallOffersCount : IPacketHandler
	{
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.MallController.mallController.UpdateMall(packet.protoBody as ChuMeng.GCPushMallOffersCount);
			}
		}
	}
}
