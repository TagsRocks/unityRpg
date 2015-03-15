
/*
Author: QiuChell	
Email: 122595579@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ChuMeng
{
	public class RankListController : MonoBehaviour
	{
		public static RankListController rankListController;
		private GCLoadTop4Type rankListInfo;
		private int playerRankNum = 0;
		void Awake() {
			rankListController = this;
			DontDestroyOnLoad (gameObject);
		}

		//排行列表
		public List<TopItem> GetItemList()
		{
			Debug.Log ("Get  Ranking ItemList??");
			List<TopItem> list = new List<TopItem> ();
			for (int i = 0; i<rankListInfo.ItemsCount; i++) 
			{
				list.Add(rankListInfo.ItemsList[i]);
			}
			return list;
		}

		//当前排名
		public int GetRanking()
		{
			return playerRankNum;
		}

		/*
		 * 取得指定类型的排行榜
		 */ 
		public IEnumerator LoadTop4Type(TopType type) {
			var packet = new KBEngine.PacketHolder ();
			var load = CGLoadTop4Type.CreateBuilder ();
			load.TopType = type;
			yield return StartCoroutine(KBEngine.Bundle.sendSimple(this, load, packet));
			rankListInfo = (packet.packet.protoBody as GCLoadTop4Type);
		}

		/*
		 * 搜索定位玩家在指定类型排行榜
		 */ 
		public IEnumerator SearchPlayerTop(TopType type,string playerName) {
			var packet = new KBEngine.PacketHolder ();
			var load = CGSearchPlayerTop.CreateBuilder ();
			load.TopType = type;
			load.PlayerName = playerName;
			yield return StartCoroutine(KBEngine.Bundle.sendSimple(this, load, packet));
			playerRankNum = (packet.packet.protoBody as GCSearchPlayerTop).TopIndex;
		}
	}

}