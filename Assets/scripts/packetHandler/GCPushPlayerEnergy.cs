
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace PacketHandler
{
	public class GCPushPlayerEnergy : IPacketHandler
	{
		public override void HandlePacket (KBEngine.Packet packet)
		{
			if (packet.responseFlag == 0) {
				var data = packet.protoBody as ChuMeng.GCPushPlayerEnegry;
				ChuMeng.IndustryController.industryController.UpdateEnergy(data);
			}
		}
	}

}