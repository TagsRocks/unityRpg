
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using System.Collections.Generic;

namespace ChuMeng
{

	//装备或者数据需要显示在tips面板上面的属性



	public class ActionItem
	{
		public static List<CharAttribute.CharAttributeEnum> WhiteAttributeEnum = new List<CharAttribute.CharAttributeEnum> () {
			CharAttribute.CharAttributeEnum.ATTACK,
			CharAttribute.CharAttributeEnum.DEFENSE,
			CharAttribute.CharAttributeEnum.HIT,
			CharAttribute.CharAttributeEnum.DODGE,
			CharAttribute.CharAttributeEnum.CRITICAL,
		};
		//装备动态属性
		/*
	 * 参考角色的属性名称 得到装备物品属性名称
	 */
		
		public class AttachAtt {
			
			static Dictionary<string, CharAttribute.CharAttributeEnum> _cache = null;
			public static Dictionary<string, CharAttribute.CharAttributeEnum> CacheEnum{
				get {
					if(_cache == null) {
						Log.Important("Init Cache "+_cache);
						_cache = new Dictionary<string, CharAttribute.CharAttributeEnum>();
						foreach(CharAttribute.CharAttributeEnum e in (CharAttribute.CharAttributeEnum[])System.Enum.GetValues(typeof(CharAttribute.CharAttributeEnum))) {
							Log.Important("cache "+e.ToString());
							_cache[e.ToString()] = e;
						}
					}
					return _cache;
				}
			}
			
			
			public CharAttribute.CharAttributeEnum type;
			public int value;
			public AttachAtt(CharAttribute.CharAttributeEnum t, int v) {
				type = t;
				value = v;
			}
		}

		/*
	 * 装备额外动态属性从网络或者存档中加载
	 */ 
		public class EquipExtraDefine {
			//装备基本属性
			public List<AttachAtt> WhiteAttributes = new List<AttachAtt>();
			
			
			//装备重铸属性
			public List<AttachAtt> EquipAttributes = new List<AttachAtt>();
			
			//装备精炼属性
			//每个星星对应一套属性
			public List<List<AttachAtt>> StarEquipAttributes = new List<List<AttachAtt>>();
			
			//宝石属性
			//GemID
			//GemDatabase
			public List<int> EquipAttachGem = new List<int>();
			
			public int sellPrice;
			public ItemData.BindInfo bindInfo = ItemData.BindInfo.Free;
			
			//镶嵌宝石数
			public int enableGemCount = 4;
			
		}

		public enum ItemType
		{
			Skill,
			LifeSkill,
			PackageItem,
			BankItem,
			BoothItem,
			LootItem,
			Equip,
		}

		public enum OwnerType
		{
			Backpack,
			Equip,
		}

		public int ID;
		ItemData _itemData = null;
		public EquipExtraDefine extraInfo = new EquipExtraDefine ();



		//从配置里面收集白色属性 设置物品 或者装备 基础属性
		protected void InitWhiteAttribute ()
		{
			if (itemData != null) {
				foreach(ItemData.Attr e in itemData.whiteAttr) {
					extraInfo.WhiteAttributes.Add(new AttachAtt(e.k, e.v));
				}
			}
		}

		public virtual OwnerType GetOwnerType ()
		{
			throw new System.NotImplementedException ();
		}


 		//装备扩展信息
		public ItemData itemData {  //物品基础信息
			set {
				_itemData = value;
				InitItemData ();
			}
			get {
				return _itemData;
			}
		}

		//BackpackData  
		//EquipData 
		//DynamicData inherit  ActionItem

		public string GetTitle ()
		{
			return itemData.ItemName;
		}


		public virtual void InitItemData ()
		{
		}

		public virtual int GetGemInEquipCount ()
		{
			return 0;
		}

		public virtual Icon GetGemIcon (int i)
		{
			return null;
		}

		public virtual string GetItemDesc ()
		{
			return "";
		}

		public virtual string GetBaseWhiteAttrInfo ()
		{
			return "";
		}

		public virtual string GetGreenAttrInfo ()
		{
			return "";
		}

		public virtual string GetBlueAttrInfo ()
		{
			return "";
		}

		public virtual string GetGemInEquipDesc ()
		{
			return "";
		}

		//激活第一个动作（装备 按钮）
		public virtual void DoAction1 ()
		{
			throw new System.NotImplementedException ();
		}

		public virtual void DoAction2 ()
		{
			throw new System.NotImplementedException ();
		}

		//强化
		public virtual void DoAction3() {
			throw new System.NotImplementedException ();

		}

		public virtual void NotifyTooltipsShow ()
		{
			SuperToolTips.superToolTips.actionItem = this;

			WindowMng.windowMng.PushView ("UI/ToolTips");
			//产生UI事件，设置UI位置
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.ShowSuperToolTip);
		}

		public virtual int GetNeedLevel ()
		{
			return -1;
		}
	}


}