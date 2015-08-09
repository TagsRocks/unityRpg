using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class StoreItem : IUserInterface 
    {
        UILabel Name;
        StoreUI storeUI;
        void Awake() {
            SetCallback("Buy", OnBuy);
            Name = GetLabel("Name");
        }
        void OnBuy() {
            storeUI.OnBuy(itemId);
        }
        int itemId;
        public void SetId(StoreUI s, int id) {
            storeUI = s;
            itemId = id;
            var item = Util.GetItemData(0, id);
            var count = BackPack.backpack.GetItemCount(0, id); 
            Name.text = string.Format("{0} {1}金币 数量:{2}", item.ItemName, item.GoldCost, count);
        }

    }
}
