
/*
Author: QiuChell
Email: 122595579@qq.com
*/
using UnityEngine;
using System.Collections;

namespace PacketHandler
{

	/*
	 * 推送公会资源和贡献更新
	 */ 
	public class GCPushGuildDonateUpdate : IPacketHandler
	{
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.GuildController.guildController.PushGuildDonateUpdate(packet.protoBody as ChuMeng.GCPushGuildDonateUpdate);

			}
		}
	}

	/*
	 * 推送玩家退出公会信息
	 */ 
	public class GCPushExitGuild : IPacketHandler {
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.GuildController.guildController.PushExitGuild(packet.protoBody as ChuMeng.GCPushExitGuild);
				
			}
		}
	}

	//推送通知成为会长
	public class GCPushDevolveMaster : IPacketHandler {
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.GuildController.guildController.PushDevolveMaster(packet.protoBody as ChuMeng.GCPushDevolveMaster);
				
			}
		}
	}

	//推送玩家加入公会成功
	public class GCPushPlayerJoinGuildSuccess : IPacketHandler {
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.GuildController.guildController.PushPlayerJoinGuildSuccess(packet.protoBody as ChuMeng.GCPushPlayerJoinGuildSuccess);
				
			}
		}
	}


	//推送公会成员,谁加入了公会
	public class GCPushPlayerJoinGuild : IPacketHandler {
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.GuildController.guildController.PushPlayerJoinGuild(packet.protoBody as ChuMeng.GCPushPlayerJoinGuild);
				
			}
		}
	}

	//推送解散公会
	public class GCPushDisbandGuild : IPacketHandler {
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.GuildController.guildController.PushDisbandGuild(packet.protoBody as ChuMeng.GCPushDisbandGuild);
				
			}
		}
	}

	// 推送踢出公会
	public class GCPushDismissMember : IPacketHandler {
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.GuildController.guildController.PushDismissMember(packet.protoBody as ChuMeng.GCPushDismissMember);
				
			}
		}
	}
}