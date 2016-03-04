﻿using UnityEngine;
using System.Collections;

namespace PacketHandler
{
    /// <summary>
    /// 金币数量变化
    /// </summary>
	public class GCPushGoodsCountChange : IPacketHandler
	{
		public override void HandlePacket (KBEngine.Packet packet)
		{
			var pushGoods = packet.protoBody as ChuMeng.GCPushGoodsCountChange;
			foreach (ChuMeng.GoodsCountChange gc in pushGoods.GoodsCountChangeList) {
                ChuMeng.BackPack.backpack.UpdateGoodsCount(gc);
			}
			ChuMeng.MyEventSystem.myEventSystem.PushEvent (ChuMeng.MyEvent.EventType.UpdateItemCoffer);
		}
	}

}