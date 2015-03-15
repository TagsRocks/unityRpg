
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 游戏商场道具或者装备
 */ 
namespace ChuMeng
{
	public class Money {
		public int price;
		public MoneyType type;
	}

	public class MallItem {
		SaleGoods goods;
		MallType type;
		ItemData itemData;
		int goodsID;
		int goodsPrice;
		int goodsMoneyType;
		public int Id {
			get {
				return goodsID;
			}
		}
		public ItemData data {
			get {
				return itemData;
			}
		}
		public MallType mallType {
			get {
				return type;
			}
		}
		public Money price {
			get {
				var m = new Money();
				m.price = goodsPrice;
				m.type = (MoneyType)goodsMoneyType;
				return m;
			}
		}
		public MallItem(int _goodsid,int _goodsprice,int _goodsmoneytype,int _propsOrEquip,int _baseid ,MallType t) {
			//goods = g;
			goodsID = _goodsid;
			goodsPrice = _goodsprice;
			goodsMoneyType = _goodsmoneytype;
			type = t;

			itemData = Util.GetItemData (_propsOrEquip, _baseid);
		}
	}

	public class MallController : MonoBehaviour
	{
		public static MallController mallController;


		List<MallItem> allItems = new List<MallItem>();
		void Awake() {
			mallController = this;
			DontDestroyOnLoad (gameObject);
		}

		/*
		 * 初始化Npc商店
		 * Load All Npc Store
		 */ 
		public IEnumerator LoadSaleItems() {
			var packet = new KBEngine.PacketHolder ();
			var load = CGLoadSaleItems.CreateBuilder ();
			load.MallType = MallType.Hot;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, load, packet));
			var data = packet.packet.protoBody as GCLoadSaleItems;
			foreach (SaleGoods g in data.SaleGoodsList) {
				var n = new MallItem(g.Id,g.Price,(int)g.MoneyType,g.PropsOrEquip,g.BaseId, MallType.Hot);
				allItems.Add(n);
			}

			load = CGLoadSaleItems.CreateBuilder ();
			load.MallType = MallType.SaleOff;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, load, packet));
			data = packet.packet.protoBody as GCLoadSaleItems;
			foreach (SaleGoods g in data.SaleGoodsList) {
				var n = new MallItem(g.Id,g.Price,(int)g.MoneyType,g.PropsOrEquip,g.BaseId, MallType.SaleOff);
				allItems.Add(n);
			}

			load = CGLoadSaleItems.CreateBuilder ();
			load.MallType = MallType.Fashion;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, load, packet));
			data = packet.packet.protoBody as GCLoadSaleItems;
			foreach (SaleGoods g in data.SaleGoodsList) {
				var n = new MallItem(g.Id,g.Price,(int)g.MoneyType,g.PropsOrEquip,g.BaseId, MallType.Fashion);
				allItems.Add(n);
			}

			load = CGLoadSaleItems.CreateBuilder ();
			load.MallType = MallType.Material;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, load, packet));
			data = packet.packet.protoBody as GCLoadSaleItems;
			foreach (SaleGoods g in data.SaleGoodsList) {
				var n = new MallItem(g.Id,g.Price,(int)g.MoneyType,g.PropsOrEquip,g.BaseId, MallType.Material);
				allItems.Add(n);
			}

			Log.Sys ("Init SaleItems in Mall is "+allItems.Count);
		}

		public List<MallItem> GetSaleItems(MallType t) {
			var ret = new List<MallItem> ();
			Log.GUI ((allItems.Count == 0)+"?<>?"+allItems.Count);
			if (allItems.Count == 0) {

				foreach (MallConfigData m in GameData.MallConfig) {
					var n = new MallItem(m.id,m.price,m.currencyType,m.goodsType,m.propsId, (MallType)(m.salesStatus));
					allItems.Add(n);
				}
			}
			foreach (MallItem it in allItems) {
				if(it.mallType == t) {
					ret.Add(it);
				}
			}
			return ret;
		}

		/*
		 * 抢购商品
		 */ 
		public IEnumerator SaleGoods(int itemId) {
			var packet = new KBEngine.PacketHolder ();
			var load = GCSaleGoods.CreateBuilder ();
			yield return StartCoroutine(KBEngine.Bundle.sendSimple(this, load, packet));

		}

		/*
		 * 购买商店物品
		 * TODO: 更新背包物品
		 */ 
		public IEnumerator ShopBuy(int mallId, int count) {
			var packet = new KBEngine.PacketHolder ();
			var load = CGBuyMallProps.CreateBuilder ();
			load.MallId = mallId;
			load.Count = count;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, load, packet));

			BackPack.backpack.ShopBuy (mallId, count);
		}
	

		/*
		 * 购买赠送商店的礼物
		 */ 
		public IEnumerator BuyPresentMall(int mallId, int count, int targetId) {
			var packet = new KBEngine.PacketHolder ();
			var buy = CGBuyPresentMallProps.CreateBuilder ();
			buy.MallId = mallId;
			buy.Count = count;
			buy.TargetId = targetId;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, buy, packet));

		}

		public void UpdateMall(GCPushMallOffersCount mall) {
		}

	}

}