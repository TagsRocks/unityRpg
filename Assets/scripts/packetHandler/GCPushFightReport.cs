
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace PacketHandler
{
	public class GCPushFightReport : IPacketHandler
	{
		public override void HandlePacket (KBEngine.Packet packet)
		{
			if (packet.responseFlag == 0) {
				var data = packet.protoBody as ChuMeng.GCPushFightReport;
				var player = ChuMeng.ObjectManager.objectManager.GetPlayer(data.Active.Id);
				if(player != null) {
					var msg = new ChuMeng.MyAnimationEvent.Message(ChuMeng.MyAnimationEvent.MsgType.DoSkill);
					msg.fightReport = data;
					player.GetComponent<ChuMeng.MyAnimationEvent>().InsertMsg(msg);
				}
			}
		}
	}
}
