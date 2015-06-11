﻿using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class MainUI : IUserInterface
    {
        UILabel hpLabel;
        void Awake(){
            hpLabel = GetLabel("HPNum");

            SetCallback("ReturnCity_Button", OnCopy);
            SetCallback("Knapsack_Button", OnBag);
            SetCallback("Skill_Button", OnSkill);
            SetCallback("NormalATKButton", OnTalk);
            SetCallback("StoreButton", OnStore);
            this.regEvt = new System.Collections.Generic.List<MyEvent.EventType>(){
                MyEvent.EventType.UpdateItemCoffer,
            };
            RegEvent();
        }
        void UpdateFrame(){
            hpLabel.text = GameInterface_Backpack.GetHpNum().ToString(); 
        }
        protected override void OnEvent(MyEvent evt)
        {
            UpdateFrame();
        }

        void OnTalk(GameObject g){
        }
        void OnCopy(GameObject g){
            WindowMng.windowMng.PushView ("UI/CopyList", true);
            MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.OpenCopyUI);
        }

        void OnBag(GameObject g){
        }
        void OnSkill(GameObject g){
        }

        void OnStore(GameObject g){
            WindowMng.windowMng.PushView ("UI/StoreUI", true);
            MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.PackageItemChanged);
        }

    }
}
