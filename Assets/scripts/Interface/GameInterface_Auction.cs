
/*
Author: QiuChell
Email: 122595579@qq.com
*/
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace ChuMeng
{
	public class GameInterface_Auction 
	{
		private static GameInterface_Auction instance;

		public List<AuctionTable> tableList = new List<AuctionTable>();//所有分类标题
		public List<AuctionTable> AllList = new List<AuctionTable>();//存储所有分类
		public static GameInterface_Auction GetInstance()
		{
			if (instance == null)
			{
				instance = new GameInterface_Auction();
			}
			return instance;
		}
		
		public GameInterface_Auction()
		{
			AuctionData ();
			AuctionTableData ();
		}
		public void AuctionTableData() {
			for (int i = 0;i<tableList.Count;i++) {
				AuctionTable arr = new AuctionTable();
				AllList.Add(arr);
				foreach (AuctionTreeConfigData AU in GameData.AuctionTreeConfig) {
					if(tableList[i].tableNode == AU.parentNode)
					{
						AuctionTable at = new AuctionTable();
						at.tableNode = AU.node;
						at.tableParentNode = AU.parentNode;
						at.tableName = AU.nodeName;
						AllList[i].arr.Add(at);
					}
				}
			}
		}
		public void AuctionData() {
			foreach (AuctionTreeConfigData au in GameData.AuctionTreeConfig) {
				if(au.parentNode == 0)
				{
					AuctionTable at = new AuctionTable();
					at.tableNode = au.node;
					at.tableParentNode = au.parentNode;
					at.tableName = au.nodeName;
					tableList.Add(at);
				}
				
			}
		}

		public int GetItemIndex(List<AuctionItem> itemInfo,int id)
		{
			for(int i = 0;i<itemInfo.Count;i++)
			{
				if(itemInfo[i].Id == id)
				{
					return i;
				}
			}
			return 0;
		}
	}


	public class AuctionTable {
		public int tableNode;
		public int tableParentNode;
		public string tableName;
		public List<AuctionTable> arr = new List<AuctionTable>();
	}
}
