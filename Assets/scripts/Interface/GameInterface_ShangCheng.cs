using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class GameInterface_ShangCheng
	{
		public static GameInterface_ShangCheng ShangCheng = new GameInterface_ShangCheng();

		public List<MallItem> GetSales (MallType mallType)
		{
			return MallController.mallController.GetSaleItems (mallType);
		}

		public void BuyGoods(MallItem goods) {
			MallController.mallController.StartCoroutine(MallController.mallController.ShopBuy (goods.Id, 1));
		}

		//更新FakeObject CurrentPlayer的装备信息  参考npcEquipment 以及背包的实现 SelfEquip 
		//以及ToolTip 上使用按钮 导致的UI状态显示变化  DoAction1
		public void TryFashion(MallItem goods) {
            /*
			var evt = new MyEvent (MyEvent.EventType.TryEquip);
			evt.localID = ObjectManager.objectManager.GetMyLocalId ();
			evt.boolArg = true;
			evt.equipData = new EquipData(goods);
			Log.GUI ("TryFashion is "+evt.localID);
			MyEventSystem.myEventSystem.PushEvent (evt);
            */
		}

		//TODO: 重置界面和功能
		public void Charge() {
		}
	}

}
