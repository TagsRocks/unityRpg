using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public static class GameInterface_Backpack
    {
        /// <summary>
        /// 拾取某个物品
        /// </summary>
        public static void PickItem(ItemData itemData, int num)
        {
            var pick = CGPickItem.CreateBuilder();
            pick.ItemId = itemData.ObjectId;
            pick.Num = num;
            KBEngine.Bundle.sendImmediate(pick);
        }

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
            var me = ObjectManager.objectManager.GetMyPlayer().GetComponent<AIBase>();
            if(me.GetAI().state.type != AIStateEnum.IDLE){
                //WindowMng.windowMng.ShowNotifyLog("只有在安全的地方才能使用物品");
                me.GetComponent<MyAnimationEvent>().InsertMsg(new MyAnimationEvent.Message(MyAnimationEvent.MsgType.IDLE));
                //return;
            }

            me.GetComponent<NpcAttribute>().StartCoroutine(UseItemCor(itemId));
        }
        static System.Collections.IEnumerator UseItemCor(int itemId){
            yield return new WaitForSeconds(0.1f);
            var id = BackPack.backpack.GetItemId(itemId);
            var itemData = Util.GetItemData(0, itemId);

            var use = CGUseUserProps.CreateBuilder();
            use.UserPropsId = id;
            use.Count = 1;
            var packet = new KBEngine.PacketHolder();
            Log.Net("Send Use Item");
            yield return ClientApp.Instance.StartCoroutine(KBEngine.Bundle.sendSimple(ClientApp.Instance, use, packet));
            Log.Sys("UseResult "+packet.packet.flag);
            if(packet.packet.responseFlag == 0) {
                GameInterface_Skill.MeUseSkill(itemData.triggerBuffId);
            }
        }

    }

}