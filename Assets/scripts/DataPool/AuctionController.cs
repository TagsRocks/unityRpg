
/*
Author: QiuChell
Email: 122595579@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class AuctionController : MonoBehaviour
	{
		public static AuctionController auctionController;
		public GCAuctionInfo auctionInfo;
		public GCLoadSellGoodsInfo goodsInfo;
		public GCSearchAuctionInfo seachInfo;
		void Awake() {
			auctionController = this;
			DontDestroyOnLoad (this);
		}

		//拍卖行  拍卖物品默认显示列表
		public List<AuctionItem> GetItemList()
		{
			List<AuctionItem> list = new List<AuctionItem> ();
			for (int i = 0; i<auctionInfo.AuctionItemsCount; i++) 
			{
				list.Add(auctionInfo.AuctionItemsList[i]);
			}
			return list;
		}
		
		//我的拍卖 物品显示列表
		public List<AuctionItem> GetMyItemList()
		{
			List<AuctionItem> list = new List<AuctionItem> ();
			for (int i = 0; i<goodsInfo.AuctionItemsCount; i++) 
			{
				list.Add(goodsInfo.AuctionItemsList[i]);
			}
			return list;
		}

		//搜索 物品显示列表
		public List<AuctionItem> GetSeachItemList()
		{
			List<AuctionItem> list = new List<AuctionItem> ();
			for (int i = 0; i<seachInfo.AuctionItemsCount; i++) 
			{
				list.Add(seachInfo.AuctionItemsList[i]);
			}
			return list;
		}

		/*
		 * 加载拍卖行信息
		 * */
		public IEnumerator LoadAuctionInfo(int startIndex,int pageSize)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGAuctionInfo.CreateBuilder ();
			load.StartIndex = startIndex;
			load.PageSize = pageSize;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
			auctionInfo = (packet.packet.protoBody as GCAuctionInfo);
		}

		/*
		 * 购买
		 * */
		public IEnumerator AuctionBuyGoods(int id,int count)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGBuyGoods.CreateBuilder ();
			load.Id = id;
			load.Count = count;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 * 寄售物品
		 * */
		public IEnumerator AuctionSellGoods(int id,int type,int price,int count)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGSellGoods.CreateBuilder ();
			load.UserPropsId = id;
			load.GoodsType = type;
			load.PerCost = price;
			load.SellCount = count;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 * 取消,下架寄售 
		 * */
		public IEnumerator CancelSellGoods(int id)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGCancelSellGoods.CreateBuilder ();
			load.Id = id;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
		}

		/*
		 * 获取上架物品信息   我的拍卖物品显示
		 * */
		public IEnumerator LoadSellGoodsInfo()
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGLoadSellGoodsInfo.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
			goodsInfo = (packet.packet.protoBody as GCLoadSellGoodsInfo);
		}

		/*
		 * 搜索拍卖行商品信息
		 * */
		public IEnumerator SearchAuctionInfo(int startIndex,int pageSize,string goodsName,int minLevel,int maxLevel,int rootSort,int sortType)
		{
			var packet = new KBEngine.PacketHolder ();
			var load = CGSearchAuctionInfo.CreateBuilder ();
			load.StartIndex = startIndex;
			load.PageSize = pageSize;
			load.GoodsName = goodsName;
			load.MinLevel = minLevel;
			load.MaxLevel = maxLevel;
			load.RootSort = rootSort;
			load.SortType = sortType;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, load, packet));
			seachInfo = (packet.packet.protoBody as GCSearchAuctionInfo);
		}

	}
}
