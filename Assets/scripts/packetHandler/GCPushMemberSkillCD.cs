﻿
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace PacketHandler
{
	public class GCPushMemberSkillCD : IPacketHandler
	{
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.SkillDataController.skillDataController.UpdateCoolDown(packet.protoBody as ChuMeng.GCPushMemberSkillCD);
			}
		}
	}

}