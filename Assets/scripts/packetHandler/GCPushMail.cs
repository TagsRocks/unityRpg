
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace PacketHandler
{
	public class GCPushMail : IPacketHandler
	{
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.MailController.mailController.ReceiveMail(packet.protoBody as ChuMeng.GCPushMail);
			}
		}
	}

}
