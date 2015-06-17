using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public static class DropGoods
    {
        public static void Drop(NpcAttribute mon)
        {
            var treasure = mon.GetDropTreasure();
            if (treasure != null)
            {
                Log.Sys("DropTreasure " + treasure.Count);
                if (treasure != null)
                {
                    var itemData = Util.GetItemData(0, (int)treasure [0]);
                    int num = 1;
                    if (treasure.Count >= 3)
                    {
                        num = (int)treasure [2];
                    }

                    ItemDataRef.MakeDropItem(itemData, mon.transform.position + new Vector3(0, 0.4f, 0), num);

                    //Drop Gold Or 
                    /*
                var g = GameObject.Instantiate(Resources.Load<GameObject>(itemData.DropMesh)) as GameObject;
                var com = NGUITools.AddMissingComponent<ItemDataRef>(g);
                */

                }
            }else {
                Log.Sys("NoDropTreasure ");
            }
        }
    }

}