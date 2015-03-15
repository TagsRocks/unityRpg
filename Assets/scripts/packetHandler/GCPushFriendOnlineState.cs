
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace PacketHandler
{
	public class GCPushFriendOnlineState : IPacketHandler
	{
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.FriendsController.friendsController.OnlineState(packet.protoBody as ChuMeng.GCPushFriendOnlineState);

			}
		}
	}

	public class GCPushFriendLevelChange : IPacketHandler {
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.FriendsController.friendsController.LevelChange(packet.protoBody as ChuMeng.GCPushFriendLevelChange);
				
			}
		}
	}

	public class GCPushFriendInvitedResult : IPacketHandler {
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.FriendsController.friendsController.PushFriendInvitedResult(packet.protoBody as ChuMeng.GCPushFriendInvitedResult);
				
			}
		}
	}


	public class GCPushFriendDeleted : IPacketHandler {
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.FriendsController.friendsController.FriendDelMe(packet.protoBody as ChuMeng.GCPushFriendDeleted);
				
			}
		}
	}
	public class GCPushFriendAddHated : IPacketHandler {
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.FriendsController.friendsController.AddHated(packet.protoBody as ChuMeng.GCPushFriendAddHatred);
				
			}
		}
	}
}