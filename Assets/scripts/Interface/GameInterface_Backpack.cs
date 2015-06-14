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

        public static void UseItem(int itemId){
            ClientApp.Instance.StartCoroutine(UseItemCor(itemId));
            
        }
        static System.Collections.IEnumerator UseItemCor(int itemId){
            var id = BackPack.backpack.GetItemId(itemId);
            var itemData = Util.GetItemData(0, itemId);

            var use = CGUseUserProps.CreateBuilder();
            use.UserPropsId = id;
            use.Count = 1;
            var packet = new KBEngine.PacketHolder();
            yield return KBEngine.Bundle.sendSimple(ClientApp.Instance, use, packet) ;
            if(packet.packet.flag == 0) {
                GameInterface_Skill.MeUseSkill(itemData.triggerBuffId);
            }
        }
    }

}