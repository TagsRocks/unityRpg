using UnityEngine;
using System.Collections;

namespace PacketHandler
{
	public class GCPushTeamInvite : IPacketHandler
	{
		#region implemented abstract members of IPacketHandler
		public override void HandlePacket (KBEngine.Packet packet)
		{
			Log.Net("GCPushTeamInvite");
			var apply = packet.protoBody as ChuMeng.GCPushTeamInvite;
			ChuMeng.TeamController.teamController.ReceiveApply (apply);
		}
		#endregion


	}

}