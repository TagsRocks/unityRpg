
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public enum GoodsTypeEnum {
        Props = 0,
        Equip = 1,
    }
	public class ItemData
	{
		private IConfig config = null;
		private EquipConfigData equipConfig = null;
		private PropsConfigData propsConfig = null;

		public enum GoodsType
		{
			Props = 0,
			Equip = 1,
		}
		//服务器上装备的位置
		public enum EquipPosition
		{
			HEAD = 1,
			TROUSERS = 2,
			SHOES = 3,
			GLOVES = 4,
			RING = 5,
			NECK = 6,
			BODY = 7,
			WEAPON = 8,
		}

		//普通道具的类
		public enum UnitTypeEnum
		{
			None = 0,
			POTION = 1,
			GEM = 2,
            UPGRADE = 3,

			QUESTITEM = 9,
			GOLD = 12,
			MATERIAL = 13,

		}
        public enum ItemID {
            GOLD = 4,
        }

		//装备对应的EquipData位置



		public enum UnitEffectEnum
		{
			None,
			AddHP,
			AddMP,
		}

		public int ObjectId {
			get {
				return config.id;
			}
		}

		public UnitEffectEnum UnitEffect = UnitEffectEnum.None;

		public float Duration {
			get {
				return propsConfig.buffEffectTime / 1000.0f;
			}
		}

        public int triggerBuffId{
            get {
                return propsConfig.triggerBuffId;
            }
        }

		public float TotalAdd {
			get {
				return propsConfig.attrValue;
			}
		}

		public UnitTypeEnum UnitType {
			get {
				//道具类型
				if (propsConfig != null) {
					return (UnitTypeEnum)propsConfig.propsType;
				}
				Log.Important ("Not a Props " + ItemName);
				throw new System.NotSupportedException ("Not a Props");
			}
		}

		public EquipPosition equipPosition {
			get {
				if (equipConfig != null) {
					return (EquipPosition)equipConfig.equipPosition;
				}
				Log.Important ("Not a Equip " + ItemName);
				throw new System.NotImplementedException ("Not a Equip");
			}
		}

		public int Rarity {
			get {
				return 0;
			}
		}
		/*
	 	* Potion Stack Number
	 	*/ 
		public int MaxStackSize {
			get {
				return propsConfig.maxAmount;
			}
		}

		//400HP in 4 seconds
		//explode trap
		//skeleton trap

		public int Damage {
			get {
				return equipConfig.attack;
			}
		}

		//远程武器的攻击范围
		//public float Range = 0.6f;

		//武器攻击音效
		/*
		public AudioClip FallSound;
		public AudioClip TakeSound;
		public AudioClip LandSound;
		public AudioClip StrikeSound;
		*/

		//装备模型名称
		public string ModelName {
			get {
				if (propsConfig != null) {
					return propsConfig.modelId;
				}
				return equipConfig.modelId;
			}
		}

		//Binded是物品的动态信息
		public enum BindInfo
		{
			Free,
			Pick,
			Equip,
			Binded,
		}

		public BindInfo bindInfo {
			get {
				if (propsConfig != null) {
					return (BindInfo)propsConfig.bindingType;
				}
				return (BindInfo)equipConfig.bindingType;
			}
		}

		public int RealDamage {
			get {
				return Damage;
			}
		}

		public int RealArmor {
			get {
				return equipConfig.defense;
			}
		}

		public string ItemName {
			get {
				if (propsConfig != null) {
					return propsConfig.name;
				}
				return equipConfig.name;
			}
		}

		public int IconSheet {
			get {
				if (propsConfig != null) {
					return propsConfig.sheet;
				}
				return equipConfig.sheet;
			}
		}

		public string IconName {
			get {
				if (propsConfig != null) {
					return propsConfig.icon;
				}
				return equipConfig.icon;
			}
		}

		public string DropMesh {
			get {
				if (propsConfig != null) {
					return propsConfig.dropIcon;
				}
				return equipConfig.dropIcon;
			}
		}

		public string Description {
			get {
				if (propsConfig != null) {
					return propsConfig.description;
				}
				return equipConfig.description;
			}
		}

		//使用等级
		public int Level {
			get {
				if (propsConfig != null) {
					return propsConfig.useLevel;
				}
				return equipConfig.useLevel;
			}
		}
		/*
		 * Weapon1  Point3 
		 * Weapon2
		 */ 
		//如果是武器是否有刀鞘
		public bool HasScabbard {
			get {
				return equipConfig.hasScabbard;
			}
		}

		//装备所在职业
		public Job equipClass {
			get {
				return (Job)equipConfig.job;
			}
		}


		public class InitItemDataRet
		{
			public int RealDamage;
			public int RealArmor;
		}

		public InitItemDataRet InitItemData ()
		{
			var it = new InitItemDataRet ();
			it.RealDamage = RealDamage;
			it.RealArmor = RealArmor;

			return it;
		}

		public bool IsEquip ()
		{
			return equipConfig != null;
		}

		public bool IsFashion ()
		{
			return IsEquip () && equipConfig.isFashion;
		}

		public bool IsTask ()
		{
			return IsProps () && UnitType == UnitTypeEnum.QUESTITEM;
		}

		public bool IsGeneral ()
		{
			return false;
		}

		public bool IsGem ()
		{
			return IsProps () && UnitType == UnitTypeEnum.GEM;
		}

		public bool IsMaterial ()
		{
			return IsProps () && UnitType == UnitTypeEnum.MATERIAL;
		}

		public bool IsProps ()
		{
			return propsConfig != null;
		}


		//根据BaseID 以及装备类型获得ItemData
		public ItemData (int goodsType, int baseId)
		{
			if (goodsType == 0) {
				propsConfig =  GMDataBaseSystem.SearchIdStatic<PropsConfigData> (GameData.PropsConfig, baseId);
				config = propsConfig;
			} else {
				equipConfig = GMDataBaseSystem.SearchIdStatic<EquipConfigData> (GameData.EquipConfig, baseId);
				config = equipConfig;
			}
			if (config == null) {
				Log.Critical ("Init ItemData Error " + goodsType + " " + baseId);
			}
		}

		public class Attr
		{
			public CharAttribute.CharAttributeEnum k;
			public int v;

			public Attr (CharAttribute.CharAttributeEnum k1, int v1)
			{
				k = k1;
				v = v1;
			}
		}

		public IEnumerable<Attr> whiteAttr {	
			get {
				foreach (CharAttribute.CharAttributeEnum e in ActionItem.WhiteAttributeEnum) {
					Log.Important ("Config is what " + config);
					//Log.Important("Field is "+e.ToString().ToLower());
					var fie = config.GetType ().GetField (e.ToString ().ToLower ());
					if (fie != null) {
						var val = (int)fie.GetValue (config);
						yield return new Attr (e, val);
					}
				}
			}
		}

		List<Attr> allAttr ()
		{
			List<Attr> a = new List<Attr> ();
			foreach (Attr attr in whiteAttr) {
				a.Add (attr);
			}
			return a;
		}

		public bool IsPackage (PlayerPackage.PackagePageEnum pkg)
		{
			if (pkg == PlayerPackage.PackagePageEnum.All) {
				return true;
			}
			if (pkg == PlayerPackage.PackagePageEnum.Equip) {
				return IsEquip ();
			}
			if (pkg == PlayerPackage.PackagePageEnum.Fashion) {
				return IsFashion ();
			}
			if (pkg == PlayerPackage.PackagePageEnum.Task) {
				return IsTask ();
			}

			if (pkg == PlayerPackage.PackagePageEnum.General) {
				return IsGeneral ();
			}

			return false;
		}

		//比较装备的新旧属性
		public string CompareWhiteAttribute (ItemData curData)
		{
			string str = "";
			var myAttr = allAttr ();
			var otherAttr = curData.allAttr ();
			foreach (Attr a in myAttr) {
				var oldAtt = otherAttr.Find (delegate(Attr att) {
					return att.k == a.k;
				});
				if (a.v > 0) {
					if (oldAtt == null) {
						str += SetAttri (a);
					} else {
						str += SetAttri (new Attr (a.k, a.v - oldAtt.v));
					}
				}
			}
			return str;
		}

		string SetAttri (Attr newAtt)
		{
			var db = GMDataBaseSystem.database.GetJsonDatabase (GMDataBaseSystem.DBName.StrDictionary);

			string str = "";
			str = string.Format ("{0}+{1}\n", db.SearchForKey (newAtt.k.ToString ()), newAtt.v);
			return str;
		}
	}
}
