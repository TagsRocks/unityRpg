
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace PacketHandler
{
	public class GCPushCoerceExitDungeon : IPacketHandler
	{
		public override void HandlePacket (KBEngine.Packet packet)
		{
			if (packet.responseFlag == 0) {
				var data = packet.protoBody as ChuMeng.GCPushCoerceExitDungeon;
				ChuMeng.DungeonController.dungeonController.CoerceExitDungeon(data);
			}
		}
	}


}