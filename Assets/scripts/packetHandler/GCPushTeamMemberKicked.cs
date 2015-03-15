using UnityEngine;
using System.Collections;

namespace PacketHandler
{
	public class GCPushTeamMemberKicked : IPacketHandler
	{
		#region implemented abstract members of IPacketHandler
		public override void HandlePacket (KBEngine.Packet packet)
		{
			var kick = packet.protoBody as ChuMeng.GCPushTeamMemberKicked;
			ChuMeng.TeamController.teamController.KickMember (kick);
		}
		#endregion

	}


}