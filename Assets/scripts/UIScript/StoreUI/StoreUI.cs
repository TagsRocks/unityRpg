﻿using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class StoreUI : IUserInterface
    {
        UILabel goldNum;
        void Awake()
        {
            SetCallback("Buy", OnBuy);
            this.regEvt = new System.Collections.Generic.List<MyEvent.EventType>(){
                MyEvent.EventType.PackageItemChanged, //GoldNum
                MyEvent.EventType.UpdateItemCoffer, //DrugNum
            };
            RegEvent();

            goldNum = GetLabel("gNum");
            //UpdateFrame();
        }
        protected override void OnEvent(MyEvent evt)
        {
            UpdateFrame();
        }
        void UpdateFrame(){
            var me = ObjectManager.objectManager.GetMyData();
            goldNum.text = "金币: "+me.GetProp(CharAttribute.CharAttributeEnum.GOLD_COIN);
        }
        void OnBuy(GameObject g)
        {
            var ret = GameInterface_Backpack.BuyItem(101);
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