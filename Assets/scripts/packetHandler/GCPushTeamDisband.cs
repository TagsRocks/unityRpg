using UnityEngine;
using System.Collections;

namespace PacketHandler
{
	public class GCPushTeamDisband : IPacketHandler
	{
		#region implemented abstract members of IPacketHandler

		public override void HandlePacket (KBEngine.Packet packet)
		{
			ChuMeng.TeamController.teamController.TeamDisband ();
		}
		#endregion
	}
}
