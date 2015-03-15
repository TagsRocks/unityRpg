
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng {
	public class SelfEquip : IUserInterface {
		GameObject HEAD;            //头
		GameObject BODY;            //身体
		GameObject NECK;             //脖子
		GameObject TROUSERS;    //裤子
		GameObject FINGER;          //手指
		GameObject GLOVES;         //手套
		GameObject WEAPON;       //武器
		GameObject SHOES;          //鞋子


		void Awake() {
			regEvt = new System.Collections.Generic.List<MyEvent.EventType>(){
				MyEvent.EventType.OpenItemCoffer,
				MyEvent.EventType.RefreshEquip,

			};
			RegEvent ();

			foreach(ItemData.EquipPosition e in (ItemData.EquipPosition[])System.Enum.GetValues(typeof(ItemData.EquipPosition))) {
				SetCallback(e.ToString(), OnEquipSlot);
			}
		}

		//显示装备槽装备信息
		void OnEquipSlot(GameObject g) {
			var act = GameInterface.gameInterface.EnumAction ((ItemData.EquipPosition)System.Enum.Parse (typeof(ItemData.EquipPosition), g.name));
			act.NotifyTooltipsShow();
		}

		void OnDisable() {
			var evt = new MyEvent (MyEvent.EventType.MeshHide);
			if (ObjectManager.objectManager != null) {
				evt.intArg = ObjectManager.objectManager.GetMyLocalId ();
				MyEventSystem.myEventSystem.PushEvent (evt);
			}
		}

		void OnEnable() {
			var evt2 = new MyEvent (MyEvent.EventType.MeshShown);
			if (ObjectManager.objectManager != null) {
				evt2.intArg = ObjectManager.objectManager.GetMyLocalId ();
				MyEventSystem.myEventSystem.PushEvent (evt2);
			}
		}


		protected override void OnEvent (MyEvent evt)
		{
			Log.Important ("Self Equip Receive Event "+evt.type);
			if (evt.type == MyEvent.EventType.OpenItemCoffer) {
				var evt2 = new MyEvent (MyEvent.EventType.MeshShown);
				evt2.intArg = ObjectManager.objectManager.GetMyLocalId ();
				MyEventSystem.myEventSystem.PushEvent (evt2);

				OnUpdateShow ();
				RefreshEquip ();
			} else if (evt.type == MyEvent.EventType.RefreshEquip) {
				RefreshEquip();
			}
		}

		void OnUpdateShow() {
		}

		void SetActionItem(string name, ActionItem item) {
			Log.Important ("SetActionItem "+name+" "+item);
			if (item == null) {
				GetName(name).SetActive(false);
			} else {
				GetName(name).SetActive(true);
				SetButtonIcon (name, item.itemData.IconSheet, item.itemData.IconName);
			}
		}

		void RefreshEquip() {
			Log.Important ("Update Equip On Self");

			var act = GameInterface.gameInterface.EnumAction (ItemData.EquipPosition.HEAD);
			//刷新装备图标
			SetActionItem ("HEAD", act);

			act = GameInterface.gameInterface.EnumAction (ItemData.EquipPosition.BODY);
			SetActionItem ("BODY", act);

			act = GameInterface.gameInterface.EnumAction (ItemData.EquipPosition.NECK);
			SetActionItem ("NECK", act);

			act = GameInterface.gameInterface.EnumAction (ItemData.EquipPosition.TROUSERS);
			SetActionItem ("TROUSERS", act);

			act = GameInterface.gameInterface.EnumAction (ItemData.EquipPosition.RING);
			SetActionItem ("RING", act);

			act = GameInterface.gameInterface.EnumAction (ItemData.EquipPosition.GLOVES);
			SetActionItem ("GLOVES", act);

			act = GameInterface.gameInterface.EnumAction (ItemData.EquipPosition.WEAPON);
			SetActionItem ("WEAPON", act);

			act = GameInterface.gameInterface.EnumAction (ItemData.EquipPosition.SHOES);
			SetActionItem ("SHOES", act);
		}

	}
}
