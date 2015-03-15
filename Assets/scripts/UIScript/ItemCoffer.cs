
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	//背包
	public class ItemCoffer : IUserInterface
	{
		public PlayerPackage.PackagePageEnum curSelect = PlayerPackage.PackagePageEnum.All;
		List<GameObject> objs;
		List<GameObject> icons;
		public int gridSelect = -1;
	
		void Awake ()
		{
			regEvt = new List<MyEvent.EventType> () 
			{
				MyEvent.EventType.OpenItemCoffer,
				MyEvent.EventType.PackageItemChanged,
				MyEvent.EventType.UpdateItemCoffer,
				MyEvent.EventType.UnitMoney,
			};

			RegEvent ();


			SetCheckBox ("GeneralButton", OnGeneral);
			SetCheckBox ("EquipButton", OnEquip);
			SetCheckBox ("AllButton", OnAll);
			SetCheckBox ("FashionabledressButton", OnFashion);
			SetCheckBox ("TaskButton", OnTask);

			SetCallback ("ClearUpButton", OnPackUp);

			objs = new List<GameObject> ();
			icons = new List<GameObject> ();
			GameObject lastIcon = null;
			UIButton lastBut = null;

			for (int i = 0; i < BackPack.MaxBackPackNumber; i++) {
				var slot = GetName ("Mesh" + i.ToString ());
				var but = NGUITools.AddMissingComponent<UIButton>(slot);

				if(lastBut == null) {
					lastBut = but;
				}else {
					but.normalSprite = lastBut.normalSprite;
					but.pressedSprite = lastBut.pressedSprite;
				}

				UIEventListener.Get(slot).onClick += OnGrid;
				objs.Add (slot);
				var t = Util.FindChildRecursive (slot.transform, "icon");
				GameObject icon = null;
				if (t == null) {
					icon = NGUITools.AddChild (slot, lastIcon);
					icon.transform.localPosition = Vector3.zero;
				} else {
					icon = t.gameObject;
					lastIcon = icon;
				}
				icons.Add (icon);
				icon.SetActive(false);

			}
		}

		void OnPackUp(GameObject g) {
			PlayerPackage.playerPackage.PackUpPacket (PackType.DEFAULT_PACK);
		}


		void OnGrid(GameObject g) {
			gridSelect = System.Convert.ToInt32 (g.name.Replace ("Mesh", ""));
			Log.Important ("On Grid is "+gridSelect);
			//设置当前选中的物品
			//得到当前BUtton绑定的ActionItem
			var actionItem = PlayerPackage.playerPackage.EnumItem (curSelect, gridSelect);
			if (actionItem != null) {
				actionItem.NotifyTooltipsShow();
			}
			//UpdateFrame ();
		}

		protected override void OnEvent (MyEvent evt)
		{
			UpdateFrame ();
		}


		void OnGeneral (bool b)
		{
			if (b) {
				curSelect = PlayerPackage.PackagePageEnum.General;
				UpdateFrame();
			}
		}
		
		void OnEquip (bool b)
		{
			if (b) {
				curSelect = PlayerPackage.PackagePageEnum.Equip;
				UpdateFrame();
			}
		}

		void OnFashion (bool b)
		{
			if (b) {
				curSelect = PlayerPackage.PackagePageEnum.Fashion;
				UpdateFrame();
			}
		}
		
		void OnAll (bool b)
		{
			if (b) {
				curSelect = PlayerPackage.PackagePageEnum.All;
				UpdateFrame();
			}
		}

		void OnTask (bool b)
		{
			if (b) {
				curSelect = PlayerPackage.PackagePageEnum.Task;
				UpdateFrame();
			}
		}


		public void UpdateFrame ()
		{
			Log.Important ("UpdateFrame ");
			for (int i = 0; i < BackPack.MaxBackPackNumber; i++) {
				icons [i].SetActive (false);
			}

			for (int i = 0; i < BackPack.MaxBackPackNumber; i++) {
				var item = PlayerPackage.playerPackage.EnumItem (curSelect, i);
				if (item != null && item.itemData != null) {
					Log.Important("Item data is "+i+" "+item.itemData.IconSheet+" "+item.itemData.IconName);
					var sp = icons [i].GetComponent<UISprite> ();
					var sheet = item.itemData.IconSheet;
					sp.atlas = Resources.Load<GameObject> ("UI/itemicons/itemicons" + sheet).GetComponent<UIAtlas> ();
					sp.spriteName = item.itemData.IconName;
					icons[i].SetActive(true);
						
					if(item.itemData.IsPackage(curSelect)) {
						sp.color = new Color(1, 1, 1);
					}else {
						sp.color = new Color(0.25f, 0.25f, 0.25f );
					}
				} else {
					
				}
			}

			var player = ObjectManager.objectManager.GetMyPlayer ();
			var info = player.GetComponent<CharacterInfo> ();
		 	GetLabel("Silver").text = info.GetProp(CharAttribute.CharAttributeEnum.SILVER_COIN).ToString();
			GetLabel("SilverRoll").text = info.GetProp(CharAttribute.CharAttributeEnum.SILVER_TICKET).ToString();
			GetLabel("Gold").text = info.GetProp(CharAttribute.CharAttributeEnum.GOLD_COIN).ToString();
			GetLabel("GoldRoll").text = info.GetProp(CharAttribute.CharAttributeEnum.GOLD_TICKET).ToString();
			GetLabel ("Prop").text = info.GetProp(CharAttribute.CharAttributeEnum.REPUTATION).ToString ();

		}


	}
}
