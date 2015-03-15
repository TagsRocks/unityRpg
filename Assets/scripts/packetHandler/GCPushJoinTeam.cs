using UnityEngine;
using System.Collections;

namespace PacketHandler
{
	public class GCPushJoinTeam : IPacketHandler
	{
		#region implemented abstract members of IPacketHandler
		public override void HandlePacket (KBEngine.Packet packet)
		{
			Log.Net ("join Team Suc");
			var joinSuc = packet.protoBody as ChuMeng.GCPushJoinTeam;
			ChuMeng.TeamController.teamController.StartCoroutine(ChuMeng.TeamController.teamController.JoinSuc (joinSuc));
		}
		#endregion

	}

}