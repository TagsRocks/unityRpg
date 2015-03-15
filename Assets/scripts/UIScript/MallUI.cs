using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class MallUI : IUserInterface
	{
		MallType mallType = MallType.Hot;
		List<GameObject> items;
		GameObject shopItem;
		List<MallItem> goods;
		void Awake() {
			regEvt = new System.Collections.Generic.List<MyEvent.EventType> () {
				MyEvent.EventType.OpenMall,
			};
			RegEvent ();

			SetCheckBox ("all", OnHot);
			SetCheckBox ("sales", OnSaleOff);
			SetCheckBox ("fashion", OnFashion);
			SetCheckBox ("material", OnMaterial);
			SetCallback ("CloseButton", Hide);

			items = new List<GameObject> ();
			shopItem = GetName ("shopprioove");
			shopItem.name = "0";
			items.Add (shopItem);

			SetCallback ("top up", OnCharge);
		}

		//显示当前玩家的 UI形象
		void OnEnable() {
			Log.GUI ("Show Fake Object Helm ");
			var evt2 = new MyEvent (MyEvent.EventType.MeshShown);
			if (ObjectManager.objectManager != null) {
				evt2.intArg = ObjectManager.objectManager.GetMyLocalId ();
				Log.GUI("My UI Local Id "+evt2.intArg);
				MyEventSystem.myEventSystem.PushEvent (evt2);
			}
		}

		void OnDisable() {
			var evt = new MyEvent (MyEvent.EventType.MeshHide);
			if (ObjectManager.objectManager != null) {
				evt.intArg = ObjectManager.objectManager.GetMyLocalId ();
				MyEventSystem.myEventSystem.PushEvent (evt);
			}
		}

		void OnCharge(GameObject g) {
			Log.GUI ("Charge Money");
			GameInterface_ShangCheng.ShangCheng.Charge ();
		}

		void OnHot(bool b) {
			if (b) {
				mallType = MallType.Hot;
				UpdateFrame();
			}
		}
		void OnSaleOff(bool b) {
			if (b) {
				mallType = MallType.SaleOff;
				UpdateFrame();
			}
		}
		void OnFashion(bool b) {
			if (b) {
				mallType = MallType.Fashion;
				UpdateFrame();
			}
		}
		void OnMaterial(bool b) {
			if (b) {
				mallType = MallType.Material;
				UpdateFrame();
			}
		}


		protected override void OnEvent (MyEvent evt)
		{
			UpdateFrame ();
		}

		void OnBuy(int gid) {
			Log.GUI ("OnBuy Gid "+gid);
			GameInterface_ShangCheng.ShangCheng.BuyGoods (goods[gid]);
		}
		void OnTry(int gid) {
			Log.GUI ("OnTry Gid " + gid);
			GameInterface_ShangCheng.ShangCheng.TryFashion (goods[gid]);
		}

		void UpdateFrame() {
			goods = GameInterface_ShangCheng.ShangCheng.GetSales (mallType);
			Log.GUI ("Get Goods Of Mall Type "+goods.Count+" "+mallType);

			int c = items.Count;
			while (items.Count < goods.Count) {
				var item = NGUITools.AddChild(shopItem.transform.parent.gameObject, shopItem);
				item.name = c.ToString();
				items.Add(item);
				c++;
			}

			for (int i = 0; i < goods.Count; i++) {
				items[i].SetActive(true);
				var name = Util.FindChildRecursive(items[i].transform, "name").GetComponent<UILabel>();
				name.text = goods[i].data.ItemName;
				var price = Util.FindChildRecursive(items[i].transform, "price").GetComponent<UILabel>();
				price.text = goods[i].price.price.ToString()+""+Util.GetMoney((int)goods[i].price.type);
				var buy = Util.FindChildRecursive(items[i].transform, "buy");
				int temp = i;
				UIEventListener.Get(buy.gameObject).onClick = delegate (GameObject g) {
					OnBuy(temp);
				};
				var icon = Util.FindChildRecursive(items[i].transform, "icon");
				Util.SetIcon(icon.GetComponent<UISprite>(), goods[i].data.IconSheet, goods[i].data.IconName);
				var tryBut = Util.FindChildRecursive(items[i].transform, "try");
				if(goods[i].data.IsFashion()) {
					tryBut.gameObject.SetActive(true);
					UIEventListener.Get(tryBut.gameObject).onClick = delegate (GameObject g){
						OnTry(temp);
					};
				}else {
					tryBut.gameObject.SetActive(false);
				}
			}
			for (int i = goods.Count; i < items.Count; i++) {
				items[i].SetActive(false);	
			}

			shopItem.transform.parent.gameObject.GetComponent<UIGrid> ().Reposition ();
		}
	}

}
