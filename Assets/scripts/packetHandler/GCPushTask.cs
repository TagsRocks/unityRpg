
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;

namespace PacketHandler
{
	public class GCPushTask : IPacketHandler
	{
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				ChuMeng.QuestDataController.questDataController.taskList.UpdateTask(packet.protoBody as ChuMeng.GCPushTask);
			}
		}
	}
}
