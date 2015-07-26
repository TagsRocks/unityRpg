
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;
/*
 * 
	背包物品结构：
		BackPackData ----> ItemData + EquipExtraInfo
 */ 
namespace ChuMeng
{

	public class BackpackData : ActionItem
	{
        Guid gid = Guid.NewGuid();
        public Guid InstanceID {
            get{
                return gid;
            }
        }

		public long cdTime = 0;
		public long id {
			get {
				return entry.Id;
			}
		}

		public int baseId {
			get {
				return entry.BaseId;
			}
		}

		public int goodsType {
			get {
				return entry.GoodsType;
			}
		}

		public int num {
			get {
				return entry.Count;
			}
		}

		public int index {
			get {
				return entry.Index;
			}
		}

		public PackType pack = PackType.DEFAULT_PACK;
		public bool binding = false;
		public int rarity = 0;
		public bool trading = false;
		public bool sell = false;
	
		public override OwnerType GetOwnerType ()
		{
			return OwnerType.Backpack;
		}
		


		public override int GetNeedLevel ()
		{
			return itemData.Level;
		}


		//处理背包中物品 装备
		void DoAction_Packet ()
		{
			if (itemData.IsEquip ()) {
				GameInterface.gameInterface.PacketItemUserEquip (this);
			}
		}
	
		public override void DoAction1 ()
		{
			DoAction_Packet ();
		}
	
		void DoActionSell() {
			//物品是否可以卖出 Rule规则列表 
			BackPack.backpack.StartCoroutine (BackPack.backpack.ShopSell (index));

		}

		public override void DoAction2 ()
		{
			DoActionSell ();
		}

		public override void DoAction3 ()
		{	
			GameInterface_Compose.compose.selEquip = (BackpackData)this;
			WindowMng.windowMng.PushView ("UI/Intensify_Panel");
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.OpenComposeItem);
		}

		public override string GetItemDesc ()
		{
			return itemData.Description;
		}

		public override void InitItemData ()
		{
			InitWhiteAttribute ();
		}

	
		public override Icon GetGemIcon (int ind)
		{
			if (ind >= extraInfo.EquipAttachGem.Count) {
				return null;
			}
		
			var gemId = extraInfo.EquipAttachGem [ind];
			if (gemId != -1) {
				var itemD = Util.GetItemData(0, gemId);
				var id = itemD.IconSheet;
				var iconName = itemD.IconName;
				return new Icon (id, iconName);
			}
		
			return null;
		}

	
		public override string GetBaseWhiteAttrInfo ()
		{
			var str = "";
		
			foreach (AttachAtt at in extraInfo.WhiteAttributes) {
				Log.Important ("White Attribute is " + at.type + " " + at.value);
				//if(at.type > AttachAttEnum.WHITE_START && at.type < AttachAttEnum.WHITE_END) {
				var s = SetAttri (at);
				str += s;
				//}
			}
			return str;
		}
	
		public override string GetGreenAttrInfo ()
		{
			var str = "";
			foreach (AttachAtt at in extraInfo.EquipAttributes) {
				//if(at.type > AttachAttEnum.GREEN_START && at.type < AttachAttEnum.GREEN_END) {
				str += SetAttri (at);
				//}
			}
			return str;
		}
	
		void AddAtt (List<AttachAtt> at, AttachAtt newAt)
		{
			foreach (AttachAtt a in at) {
				if (a.type == newAt.type) {
					a.value += newAt.value;
					return;
				}
			}
			at.Add (newAt);
		}
	
		//精炼每颗星属性整合到一起
		public override string GetBlueAttrInfo ()
		{
			List<AttachAtt> allBlueAtt = new List<AttachAtt> ();
			var str = "";
			foreach (List<AttachAtt> att in extraInfo.StarEquipAttributes) {
				foreach (AttachAtt at in att) {
					AddAtt (allBlueAtt, at);
				}
			}
			foreach (AttachAtt b in allBlueAtt) {
				str += SetAttri (b);
			}
			return str;
		}
	
		public int GetGemCount ()
		{
			return extraInfo.EquipAttachGem.Count;
		}
	
		public override string GetGemInEquipDesc ()
		{
			string str = "";
			int c = GetGemCount ();
			for (int i = 0; i < c; i++) {
				str += GetGemExtAttr (i);
			}
			return str;
		}
	
		string GetGemExtAttr (int i)
		{		
			var strDb = GMDataBaseSystem.database.GetJsonDatabase (GMDataBaseSystem.DBName.StrDictionary);
		
			var gemId = extraInfo.EquipAttachGem [i];
			if (gemId == -1) {
				return "";
			}
			var gemData = Util.GetItemData (0, gemId);
			string str = "";
			foreach (ItemData.Attr attr in gemData.whiteAttr) {
				str += string.Format ("{0} +{1}\n", strDb.SearchForKey (attr.k.ToString()), attr.v);
			}
			return str;
		}
	
		//格式化属性字符串
		string SetAttri (AttachAtt at)
		{
			var db = GMDataBaseSystem.database.GetJsonDatabase (GMDataBaseSystem.DBName.StrDictionary);
		
			string str = "";
			if (at.value > 0) {
				str = string.Format ("{0}+{1}\n", db.SearchForKey (at.type.ToString ()), at.value);
			} else {
				str = string.Format ("{0}{1}\n", db.SearchForKey (at.type.ToString ()), at.value);
			}
		
			return str;
		}
	
		public PackInfo packInfo = null;
		public PackEntry entry = null;

		/*
		 * 从服务器初始化背包数据
		 */ 
		public BackpackData (PackInfo pinfo)
		{
			if (pinfo != null) {
				packInfo = pinfo;
				entry = packInfo.PackEntry;
				//num = entry.Count;
				/*
			 * BaseId Load ItemData Template 
			 * Resources units items  --->ItemData
			 */
				Log.Important ("Init ItemData is " + pinfo.PackEntry.GoodsType);
				itemData = Util.GetItemData (pinfo.PackEntry.GoodsType, baseId);
				if (itemData == null) {
					Debug.LogError ("BackpackData:: Init Error " + baseId);
				}
			}
		}
	
		public BackpackData(PackEntry e) {
			packInfo = null;
			entry = e;
			//num = entry.Count;
			itemData = Util.GetItemData (e.GoodsType, baseId);
		}
	
	}

}
