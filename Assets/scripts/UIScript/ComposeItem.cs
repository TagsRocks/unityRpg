using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace ChuMeng
{
	public class ComposeItem : IUserInterface
	{
		UIPopupList list;
		List<EquipIntensifyConfigData> allPres = null;
		EquipIntensifyConfigData curSel;

		void Awake() {
			regEvt = new System.Collections.Generic.List<MyEvent.EventType> () {
				MyEvent.EventType.OpenComposeItem,
				MyEvent.EventType.ComposeOver,
			};
			RegEvent ();

			SetList ("PopList", OnPop);
			list = GetList ("PopList");

			SetCallback ("IntensifyButton", OnOk);
			SetCallback ("CancelButton", OnCancel);
		}

		void OnOk(GameObject g) {
			GameInterface_Compose.compose.ComposeItemBegin (curSel);
		}

		void OnCancel(GameObject g) {
			Hide (g);
		}


		void OnPop(string sel) {
			if (allPres != null) {
				curSel = null;
				foreach (EquipIntensifyConfigData ed in allPres) {
					if (sel == ed.targetEquipName) {
						curSel = ed;
						break;
					}
				}

				if(curSel != null) {
					var mats = curSel.needMaterial.Split('|');
					int c = 1;
					foreach(string mat in mats) {
						var minfo = mat.Split('_');
						var objId = Convert.ToInt32(minfo[0]);
						var goodsType = Convert.ToInt32(minfo[1]);
						var count = Convert.ToInt32(minfo[2]);
						var itemData = Util.GetItemData(goodsType, objId);
						GetName("icon"+c).SetActive(true);
						SetIcon("icon"+c, itemData.IconSheet, itemData.IconName);
						GetName("mat"+c).SetActive(true);
						GetLabel("mat"+c).text = count.ToString()+"/"+BackPack.backpack.GetItemCount(goodsType, objId);
						c++;
					}

					for(int i = c; i <= 3; i++) {
						GetName("icon"+c).SetActive(false);	
						GetName("mat"+c).SetActive(false);
					}


					var target = Util.GetItemData(1, curSel.targetEquipId);
					var curData = Util.GetItemData(1, curSel.equipId);
					var res = "[00ff00]"+target.CompareWhiteAttribute(curData)+"[-]";
					GetLabel("AttrLabel").text = res;

					GetLabel("money").text = curSel.pay.ToString()+""+Util.GetMoney(curSel.moneyType);
				}
			}
		}
		
		protected override void OnEvent (MyEvent evt)
		{
			if (evt.type == MyEvent.EventType.ComposeOver) {
				Hide (null);
			} else {
				UpdateFrame ();
			}
		}

		//显示需要的材料 和 可能的配方以及 合成后的效果
		void UpdateFrame() {
			var compose = GameInterface_Compose.compose;
			var objId = compose.selEquip.itemData.ObjectId;
			allPres = compose.GetIntensify (objId);

			list.items = new System.Collections.Generic.List<string> ();
			foreach (EquipIntensifyConfigData ed in allPres) {
				list.items.Add(ed.targetEquipName);
			}
		}

	}

}