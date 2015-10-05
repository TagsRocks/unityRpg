using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public class PackageUI : IUserInterface
    {
        UIGrid  leftGrid;
        GameObject leftCell;

        List<GameObject> leftCells = new List<GameObject>();

        UILabel goldNum;
        void Awake()
        {
            SetCallback("closeButton", Hide);
            goldNum = GetLabel("GoldNum/Label");
            regEvt = new List<MyEvent.EventType>() {
                MyEvent.EventType.UpdateItemCoffer, //DrugNum
            };
            RegEvent();
            SetCallback("Forge", OnForge);
        }

        void OnForge() {
            WindowMng.windowMng.PushView("UI/ForgeList");
            

        }
           
        protected override void OnEvent(MyEvent evt)
        {
            var me = ObjectManager.objectManager.GetMyData();
            goldNum.text = "金币: "+me.GetProp(CharAttribute.CharAttributeEnum.GOLD_COIN);
        }
        // Use this for initialization
        void Start()
        {
    
        }
    
        // Update is called once per frame
        void Update()
        {
    
        }
    }

}