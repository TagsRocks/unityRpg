using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public static class GameInterface_Backpack
    {
        public static bool BuyItem(int itemId)
        {
            var buyItem = CGBuyShopProps.CreateBuilder();
            buyItem.ShopId = itemId;
            buyItem.Count = 1;
            KBEngine.Bundle.sendImmediate(buyItem);
            return true;
        }
        public static int GetHpNum(){
            var hp = BackPack.backpack.GetHpPotion();
            if(hp == null) {
                return 0;
            }
            return hp.num;
        }
    }

}