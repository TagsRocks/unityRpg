using UnityEngine;
using System.Collections;

namespace PacketHandler
{
	public class GCPushGoodsCountChange : IPacketHandler
	{
		public override void HandlePacket (KBEngine.Packet packet)
		{
			var pushGoods = packet.protoBody as ChuMeng.GCPushGoodsCountChange;
			foreach (ChuMeng.GoodsCountChange gc in pushGoods.GoodsCountChangeList) {
				if(gc.Num > 0) {
					ChuMeng.BackPack.backpack.AddItemInfo (gc);	
				}else if(gc.Num < 0) {
					ChuMeng.BackPack.backpack.RemoveItemInfo(gc);
				}
			}
			ChuMeng.MyEventSystem.myEventSystem.PushEvent (ChuMeng.MyEvent.EventType.PackageItemChanged);
		}
	}

}