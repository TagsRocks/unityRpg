
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace PacketHandler
{
	public class GCPushAttribute2Members : IPacketHandler
	{
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				//ChuMeng.MailController.mailController.ReceiveMail(packet.protoBody as ChuMeng.GCPushMail);
				var msg = packet.protoBody as ChuMeng.GCPushAttribute2Members;

				foreach(ChuMeng.PlayerRolesAttributes pr in msg.PlayerRolesAttributesList) {
					var player = ChuMeng.ObjectManager.objectManager.GetPlayer(pr.UnitId.Id);
					if(player != null) {
						player.GetComponent<ChuMeng.CharacterInfo>().UpdateData(pr);
					}
				}
			}
		}
	}

}
