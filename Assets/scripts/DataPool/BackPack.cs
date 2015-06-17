
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/

/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ChuMeng
{
    /// <summary>
    /// Backpack Data Controller
    /// </summary>
	public class BackPack : MonoBehaviour
	{
		public static int MaxBackPackNumber = 25;

		public static BackPack backpack;
		GameObject uiRoot;
		/*
		 * BackPack Data Init From Network
		 */ 
		private List<BackpackData> SlotData;


		/*
		 * EquipmentData Init From Network
		 */ 
		private List<EquipData> EquipmentData;

		//要卸下来的装备的槽
		ItemData.EquipPosition equipSlot;

		EquipData oldEquip;
		GameObject equipObj;

		//背包物品所在槽
		int slotId;
		//GameObject player;
		//PotionUI pui;
		[ButtonCallFunc()]
		public bool clearEquip;

        public int GetItemId(int itemId){
            foreach(BackpackData bd in SlotData){
                if(bd != null && bd.itemData != null && bd.baseId == itemId && bd.goodsType == (int)GoodsTypeEnum.Props){
                    return (int)bd.id;
                }
            }
            return -1;
        }
		public int GetItemCount(int goodsType, int objId) {
			var count = 0;
			foreach (BackpackData bd in SlotData) {
				if(bd != null && bd.itemData != null && bd.baseId == objId && bd.goodsType == goodsType) {
					count += bd.num;
				}
			}
			return count;
		}

		public void clearEquipMethod ()
		{
#if UNITY_EDITOR
			for (int i = 1; i < EquipmentData.Count; i++) {
				EquipmentData[i].itemData = null;
			}
			EditorUtility.SetDirty(this);
#endif
		}

		//服务器更新背包数据  Don't ClearBag
		public void SetItemInfo(GCPushPackInfo info) {
			//整理背包首先清空
			if (info.BackpackAdjust) {
				UserBagClear ();
			}

			foreach (PackInfo pkinfo in info.PackInfoList) {
				PutItemInBackpackIndex (pkinfo.PackEntry.Index, new BackpackData (pkinfo));
			}
		}

		
		//枚举背包物品
		public BackpackData EnumItem(ChuMeng.PlayerPackage.PackagePageEnum type, int index) {
			var allSlots = SlotData;
			if (index < allSlots.Count) {
				return allSlots [index];
			}
			return null;
		}


		public EquipData EnumAction(ItemData.EquipPosition type) {
			Log.Important("Enum Action "+type);
			Log.Important ("count "+EquipmentData.Count);
			foreach (EquipData ed in EquipmentData) {	

				if (ed.itemData != null && ed.itemData.equipPosition == type) {
					return ed;
				}
			}
			return null;
		}


		/*
		 * 快捷栏信息
		 */ 
		public class ShortCut
		{
			//ShortCutInfo shortCutInfo;

			public ShortCut (ShortCutInfo shortInfo)
			{
				//shortCutInfo = shortInfo;
			}
		}

		public List<ShortCut> shortCuts;

		void Awake ()
		{
			DontDestroyOnLoad (gameObject);

			SlotData = new List<BackpackData> ();
			EquipmentData = new List<EquipData> ();

			for (int i=0; i < MaxBackPackNumber; i++) {
				SlotData.Add(new BackpackData((PackInfo)null));
			}
			backpack = this;


			//uIRoot = GameObject.Find ("UI Root");

			//player = GameObject.FindGameObjectWithTag ("Player");
		}

		ItemData.EquipPosition FindEquipSlot (int backpackSlotId)
		{
			var id = SlotData [backpackSlotId].itemData;
			return id.equipPosition;
		}


		void UseEquipNetwork(GCUserDressEquip result) {
			//旧背包和装备槽的数据
			//var itemAtBag = SlotData [slotId];
			//var itemAtUser = oldEquip;

			if (oldEquip != null) {
				PopEquipData(equipSlot);
			}

			//TODO:服务器告诉清理某个槽 再清理槽 不用现在清理
			//ClearSlot (slotId);

			//新背包和装备槽数据
			var ed = new EquipData (result.DressEquip);
			EquipmentData.Add (ed);

			if (result.HasPackEquip) {
				var newBagItem = new BackpackData (result.PackEquip);
				PutItemInBackpackIndex(newBagItem);
			}

			var evt = new MyEvent (MyEvent.EventType.PackageItemChanged);
			evt.intArg = slotId;
			MyEventSystem.myEventSystem.PushEvent (evt);

			//更新角色属性面板的装备信息
			//以及角色本身的装备信息
			evt = new MyEvent (MyEvent.EventType.CharEquipChanged);
			evt.localID = ObjectManager.objectManager.GetMyLocalId ();
			evt.equipData = ed;
			evt.boolArg = true;
			MyEventSystem.myEventSystem.PushEvent (evt);

			//通知角色属性面板 更新UI上面的icon
			evt = new MyEvent (MyEvent.EventType.RefreshEquip);
			MyEventSystem.myEventSystem.PushEvent (evt);
		}

		//通过网络使用装备
		public IEnumerator UseEquipForNetwork ()
		{
			Log.Important ("Use EquipData For Network");
			var newEquip = SlotData [slotId];

			CGUserDressEquip.Builder equip = CGUserDressEquip.CreateBuilder ();
			equip.DressType = true;
			equip.SrcEquipId = newEquip.id;

			if (oldEquip != null) {
				equip.DestEquipId = oldEquip.id;
			} else {
				equip.DestEquipId = 0;
			}

			var packet = new KBEngine.PacketHolder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, equip, packet));
			if (packet.packet.responseFlag == 0) {
				var useResult = packet.packet.protoBody as GCUserDressEquip;

				UseEquipNetwork(useResult);
			} else {
			}
		}


		//初始化角色装备
		//只有玩家初始化结束之后 采取 初始化玩家的装备
		void UseEquip (EquipData ed)
		{
			Log.Important ("initial role equip");
			EquipmentData.Add (ed);

			//角色本身的装备信息
			var evt = new MyEvent (MyEvent.EventType.CharEquipChanged);
			evt.localID = ObjectManager.objectManager.GetMyLocalId ();
			evt.equipData = ed;
			evt.boolArg = true;
			MyEventSystem.myEventSystem.PushEvent (evt);
			
			//通知角色属性面板 更新UI上面的icon
			evt = new MyEvent (MyEvent.EventType.RefreshEquip);
			MyEventSystem.myEventSystem.PushEvent (evt);
		}
		int GetIndex(int ind) {
			return ind;
		}
		//设定操作的物品对象
		public void SetSlotItem(BackpackData data) {
			//背包index 和 slot数组的需要不同 index 从1开始 slot从0开始
			slotId = GetIndex(data.index);
			equipSlot = data.itemData.equipPosition;
			oldEquip = GetEquipData (equipSlot);
		}


		public BackpackData GetHpPotion ()
		{
			for (int i = 0; i < SlotData.Count; i++) {
				if (SlotData [i] != null && SlotData [i].itemData != null && SlotData [i].itemData.UnitType == ItemData.UnitTypeEnum.POTION) {
					return SlotData [i];
				}
			}
			return null;
		}

		BackpackData GetMpPotion ()
		{
			for (int i=0; i < SlotData.Count; i++) {
				if (SlotData [i] != null && SlotData [i].itemData != null && SlotData [i].itemData.UnitType == ItemData.UnitTypeEnum.POTION && SlotData [i].itemData.UnitEffect == ItemData.UnitEffectEnum.AddMP) {
					return SlotData [i];
				}
			}
			return null;
		}


		/*
		 * Use HP Potion Add 
		 * TODO: use HP MP Count
		 */ 
		void OnHp (GameObject g)
		{
			/*
			BackpackData bd = GetHpPotion ();
			if (bd != null && bd.num > 0) {
				bd.num--;
				pui.SetHpNum (bd.num);
				player.GetComponent<NpcAttribute> ().AddHp (bd.itemData.Duration, bd.itemData.TotalAdd);
			}
			*/
		}

		void OnMp (GameObject g)
		{
			/*
			BackpackData bd = GetMpPotion ();
			if (bd != null && bd.num > 0) {
				bd.num--;
				pui.SetMpNum (bd.num);
				player.GetComponent<NpcAttribute> ().AddMp (bd.itemData.Duration, bd.itemData.TotalAdd);
			}
			*/
		}	


		//获取某个装备槽上面的装备
		public EquipData GetEquipData (ItemData.EquipPosition slot)
		{
			foreach (EquipData e in EquipmentData) {
				if (e.itemData != null && e.Slot == slot)
					return e;
			}
			return null;
		}

		EquipData PopEquipData (ItemData.EquipPosition slot)
		{
			foreach (EquipData e in EquipmentData) {
				if (e.Slot == slot) {
					EquipmentData.Remove (e);

					return e;
				}
			}
			return null;
		}

		//打怪捡到钱之后，通知单人服务器，通过服务器通知更新数据
		//单人模式下连接本地服务器即可
		public void PutGold (int gold)
		{
			throw new System.NotImplementedException ();
		}

		/*
		 * Put Item In Default Packet
		 * 物品数量更新只从服务器接受报文更新 本地不更新
		 */ 


		public void PutItemInBackpackIndex(BackpackData bd) {
			PutItemInBackpackIndex (bd.index, bd);
		}
		/*
		 * Initial Backpack Data From Network
		 */ 
		void PutItemInBackpackIndex (int index, BackpackData bd)
		{
			Log.Sys ("Backpack::PutItem From Network In backpack");
			if (index >= SlotData.Count || index < 0) {
				Debug.LogError ("BackPack:: index out of Range " + index);
				return;
			}
			int realIndex = GetIndex(index);
			if (SlotData [realIndex] != null && SlotData [realIndex].itemData != null) {
				Debug.LogError ("BackPack:; has object in index " + realIndex);
				//return;
			}

			SlotData [realIndex] = bd;
			Log.Sys ("BackPack:: PutItemInBackpackIndex "+realIndex+" " + SlotData [realIndex].itemData.ItemName);
		}

		void BackpackStateUpdate ()
		{
            /*
			if (pui != null) {
				BackpackData bd2 = GetHpPotion ();
				if (bd2 != null) {
					pui.SetHpNum (bd2.num);
				}
				bd2 = GetMpPotion ();
				if (bd2 != null) {
					pui.SetMpNum (bd2.num);
				}
			}
            */
		}

		/*
		 * Clear All Item From BackPack 
		 * Init From Network
		 */ 
		public void UserBagClear ()
		{
			for (int i = 0; i < SlotData.Count; i++) {
				ClearSlot (i);

			}

			BackpackStateUpdate ();
		}

		/*
		 * Remove All Equip From Slot To Load From Network
		 */ 
		public void UserEquipClear ()
		{
			var values = System.Enum.GetValues (typeof(ItemData.EquipPosition)).Cast<ItemData.EquipPosition> ();
			foreach (ItemData.EquipPosition s in values) {
				PopEquipData (s);
			}
		}



		/*
		 * TODO:放置采集物品到背包
		 */ 
		public void PutInBackpack (GCCollectItems items)
		{
		}

		bool CheckBackpackEmpty ()
		{
			for (int i=0; i < SlotData.Count; i++) {
				if (SlotData [i] == null || SlotData [i].itemData == null)
					return true;
			}
			return false;
		}
		
		// Update is called once per frame
		void Update ()
		{
		
		}

		//Load Default backpack remove this one
		public BackPack InitPlayer ()
		{
			var bp = Instantiate (Resources.Load<GameObject> ("levelPublic/backpackController")) as GameObject;
			bp.transform.parent = GameObject.Find ("levelPublic").transform;
			//SlotData = bp.SlotData;
			//EquipmentData = bp.EquipmentData;
			GameObject.Destroy (gameObject);
			return bp.GetComponent<BackPack> ();
		}

		/*
		 * Network Init EquipmentData
		 */ 
		IEnumerator InitEquipData ()
		{
			Debug.Log ("BackPack::InitEquipData  ");
			UserEquipClear ();

			var packet = new KBEngine.PacketHolder ();
			CGLoadPackInfo.Builder equip = CGLoadPackInfo.CreateBuilder ();
			equip.PackType = PackType.DRESSED_PACK;
			var data = equip.BuildPartial ();
			KBEngine.Bundle bundle = new KBEngine.Bundle ();
			bundle.newMessage (data.GetType ());
			uint fid = bundle.writePB (data);
			yield return StartCoroutine (bundle.sendCoroutine (KBEngine.KBEngineApp.app.networkInterface (), fid, packet));
			if (packet.packet.responseFlag == 0) {
				var ret = packet.packet.protoBody as GCLoadPackInfo;
				foreach (PackInfo pkinfo in ret.PackInfoList) {
					var eqData = new EquipData (pkinfo);
					//EquipmentData.Add(eqData);
					UseEquip (eqData);
					//UpdateEquipUIState(eqData);
				}
			} else {
			}
		}

		/*
		 * 整理背包
		 */ 
		public IEnumerator PackUp ()
		{
			UserBagClear ();
			var packet = new KBEngine.PacketHolder ();
			CGAutoAdjustPack.Builder pack = CGAutoAdjustPack.CreateBuilder ();
			pack.PackType = PackType.DEFAULT_PACK;
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, pack, packet));

			if (packet.packet.responseFlag == 0) {
				var ret = packet.packet.protoBody as GCLoadPackInfo;
				foreach (PackInfo pkinfo in ret.PackInfoList) {
					Log.Sys("Read PackInfo is "+pkinfo.PackEntry.BaseId);
					PutItemInBackpackIndex (pkinfo.PackEntry.Index, new BackpackData (pkinfo));
				}
			}
		}

		/*
		 * Bag Index 
		 * TODO::使用药品
		 * 物品数量更新只从服务器更新本地不更新
		 */ 
		public IEnumerator UseItem (int slotIndex, int count)
		{
			yield return null;
			/*
			var sd = SlotData [slotIndex];
			CGUseUserProps.Builder use = CGUseUserProps.CreateBuilder ();
			use.UserPropsId = sd.id;
			use.Count = count;

			KBEngine.PacketHolder packet = new KBEngine.PacketHolder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, use, packet));
			if (packet.packet.responseFlag == 0) {
				sd.num -= count;
			}
			if (sd.num == 0) {
				ClearSlot (slotIndex);
			} else {
			
			}
			*/
		}

		/*
		 * 商店卖出道具 +Gold
		 */ 
		public IEnumerator ShopSell (int slotIndex)
		{
			var sd = SlotData [slotIndex];
			CGSellUserProps.Builder sell = CGSellUserProps.CreateBuilder ();
			sell.UserPropsId = sd.id;
			sell.GoodsType = sd.goodsType;
			KBEngine.PacketHolder packet = new KBEngine.PacketHolder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, sell, packet));

			if (packet.packet.responseFlag == 0) {
				//推送卖出的金币
				/*
				var price = Resources.Load<GraphData> ("graphics/stat/price_playerbuy_normal");
				var gold = price.GetData (sd.itemData.Level) * sd.num;
				PutGold ((int)gold);
				*/
				ClearSlot (slotIndex);
			}
		}

		/*
		 * 获取快捷栏信息
		 */ 
		public IEnumerator AskSetting ()
		{
			var packet = new KBEngine.PacketHolder ();
			var askShortCut = CGLoadShortcutsInfo.CreateBuilder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple (this, askShortCut, packet));

			if (packet.packet.responseFlag == 0) {
				shortCuts = new List<ShortCut> ();
				for (int i = 0; i < 8; i++) {
					shortCuts.Add (null);
				}

				var shortCut = packet.packet.protoBody as GCLoadShortcutsInfo;
				foreach (ShortCutInfo si in shortCut.ShortCutInfoList) {
					shortCuts [si.Index] = new ShortCut (si);
				}

			}
		}
		 

		/// <summary>
		/// 每次进入一个新场景都重新初始化背包和装备信息
		/// </summary>
		/// <returns>The from network.</returns>
		public IEnumerator InitFromNetwork ()
		{
			Log.Sys ("BackPack::InitFromNetwork ");
			if (KBEngine.KBEngineApp.app == null) {
				Log.Sys ("BackPack:: no network connection");

			} else {
				UserBagClear ();

				var packet = new KBEngine.PacketHolder ();
				CGLoadPackInfo.Builder load = CGLoadPackInfo.CreateBuilder ();
				load.PackType = PackType.DEFAULT_PACK;
				var data = load.BuildPartial ();
				KBEngine.Bundle bundle = new KBEngine.Bundle ();
				bundle.newMessage (data.GetType ());
				uint fid = bundle.writePB (data);
				yield return StartCoroutine (bundle.sendCoroutine (KBEngine.KBEngineApp.app.networkInterface (), fid, packet));
				if (packet.packet.responseFlag == 0) {
					var ret = packet.packet.protoBody as GCLoadPackInfo;
					foreach (PackInfo pkinfo in ret.PackInfoList) {
						Log.Sys("read Pack info is "+pkinfo.PackEntry.BaseId);
						PutItemInBackpackIndex (pkinfo.PackEntry.Index, new BackpackData (pkinfo));
					}
				} else {
				}
				Log.Important("LoadPacketInfo is "+load);
				yield return StartCoroutine (InitEquipData ());
                MyEventSystem.myEventSystem.PushEvent(MyEvent.EventType.UpdateItemCoffer);
			}
		}

		//清空特定槽
		void ClearSlot (int sid)
		{
			SlotData [sid] = null;

			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.UpdateItemCoffer);
		}

		/*
		 * 完成任务 回收任务道具 Collect Quest Item 
		 */
		public void Collect (string itemName)
		{
			int sid = -1;
			foreach (BackpackData b in SlotData) {
				sid++;
				if (b.itemData != null && b.itemData.ItemName == itemName) {
					ClearSlot (sid);
					break;
				}
			}
		}


		/*
		 * 打开一个ItemTips面板，显示Item信息
		 */ 
		public void ShowItem (GCViewChatGoods chatGoods)
		{

		}

		/*
		 * 发送邮件扣除 银币和 物品
		 */ 
		public void SendMail (int silverCoin, List<int> itemIds)
		{
		}

		/*
		 * 获取所有邮件的奖励
		 */ 
		public void ReceiveAll(GCReceiveMailsAllReward rewards) {
		}

		//服务器推送背包状态变化信息
		//客户端时刻和服务器的数据是同步的原则
		public void ShopBuy(int mallId, int count) {
			
		}


		//强化装备
		public IEnumerator ComposeItemCor(int node) {
			CGEquipIntensify.Builder ein = CGEquipIntensify.CreateBuilder ();
			ein.Id = node;
			var packet = new KBEngine.PacketHolder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, ein, packet));
			if (packet.packet.responseFlag == 0) {
				MyEventSystem.myEventSystem.PushEvent(MyEvent.EventType.ComposeOver);
			}
		}

        public void UpdateGoodsCount(GoodsCountChange gc){
            if(gc.BaseId == 4){
                var me = ObjectManager.objectManager.GetMyData();
                me.SetProp(CharAttribute.CharAttributeEnum.GOLD_COIN, gc.Num);
            }
        }
	}
}