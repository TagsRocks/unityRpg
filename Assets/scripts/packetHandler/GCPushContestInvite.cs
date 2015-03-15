
/*
Author: QiuChell	
Email: 122595579@qq.com
*/
using UnityEngine;
using System.Collections;

namespace PacketHandler
{
	public class GCPushContestInvite : IPacketHandler
	{
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.ContestController.contestController.PushContestInvite(packet.protoBody as ChuMeng.GCPushContestInvite);

			}
		}
	}

	public class GCPushContestReplayInvite : IPacketHandler {
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.ContestController.contestController.PushContestReplayInvite(packet.protoBody as ChuMeng.GCPushContestReplayInvite);
				
			}
		}
	}

	public class GCPushContestCountDown : IPacketHandler {
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.ContestController.contestController.PushContestCountDown(packet.protoBody as ChuMeng.GCPushContestCountDown);
				
			}
		}
	}
	public class GCPushContestState : IPacketHandler {
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.ContestController.contestController.PushContestState(packet.protoBody as ChuMeng.GCPushContestState);
				
			}
		}
	}
	public class GCPushContestResult : IPacketHandler {
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.ContestController.contestController.PushContestResult(packet.protoBody as ChuMeng.GCPushContestResult);
				
			}
		}
	}
}