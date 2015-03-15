
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
/*
 * 增加Buff给角色
 */ 
namespace PacketHandler
{
	public class GCPushUnitAddBuffer : IPacketHandler
	{
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				var player = ChuMeng.ObjectManager.objectManager.GetMyPlayer();
				var objCmd = new ChuMeng.ObjectCommand();
				objCmd.commandID = ChuMeng.ObjectCommand.ENUM_OBJECT_COMMAND.OC_UPDATE_IMPACT;
				objCmd.buffInfo = packet.protoBody as ChuMeng.GCPushUnitAddBuffer;
				player.GetComponent<ChuMeng.LogicCommand>().PushCommand(objCmd);
			}
		}
	}

}