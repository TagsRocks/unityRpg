
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace PacketHandler
{
	public class GCPushAddFriend : IPacketHandler
	{
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.FriendsController.friendsController.PushAddFriend(packet.protoBody as ChuMeng.GCPushAddFriend);
			}
		}
	}
}
