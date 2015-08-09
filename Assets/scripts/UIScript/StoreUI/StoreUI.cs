using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    /// <summary>
    ///主城购买药品商店UI 
    /// </summary>
    public class StoreUI : IUserInterface
    {
        UILabel goldNum;
        GameObject Cell;
        UIGrid Grid;
        List<int> storeList;
        List<GameObject> Cells = new List<GameObject>();
        void Awake()
        {
            storeList = new List<int>(){
                101, 
                102,
            };
            Grid = GetName("Grid").GetComponent<UIGrid>();
            Cell = GetName("Cell");
            //SetCallback("Buy", OnBuy);
            this.regEvt = new System.Collections.Generic.List<MyEvent.EventType>(){
                MyEvent.EventType.UpdateItemCoffer, //DrugNum GoldNum
            };
            RegEvent();

            goldNum = GetLabel("gNum");
            SetCallback("Close", Hide);
        }

        protected override void OnEvent(MyEvent evt)
        {
            UpdateFrame();
        }
        void UpdateFrame(){
            foreach(var c in Cells){
                GameObject.Destroy(c);
            }
            Cell.SetActive(false);
            foreach(var id in storeList) {
                var c = GameObject.Instantiate(Cell) as GameObject;
                c.transform.parent = Cell.transform.parent;
                Util.InitGameObject(c);
                c.SetActive(true);
                var pak = c.GetComponent<StoreItem>();
                pak.SetId(this, id);
                Cells.Add(c);
            }
            Grid.repositionNow = true;


            var me = ObjectManager.objectManager.GetMyData();
            goldNum.text = "金币: "+me.GetProp(CharAttribute.CharAttributeEnum.GOLD_COIN);
        }
        public void OnBuy(int id)
        {
            GameInterface_Backpack.BuyItem(id);
        }
    }

}