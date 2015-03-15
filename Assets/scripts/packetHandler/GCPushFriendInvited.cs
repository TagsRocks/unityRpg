
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace PacketHandler {
	public class GCPushFriendInvited : IPacketHandler {
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.FriendsController.friendsController.PushFriendInvited(packet.protoBody as ChuMeng.GCPushFriendInvited);
			}
		}
	}

}