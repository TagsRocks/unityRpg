using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class MainUI : IUserInterface
    {
        UILabel hpLabel;
        UILabel level;
        void Awake(){
            hpLabel = GetLabel("HPNum");
            level = GetLabel("Level");

            SetCallback("ReturnCity_Button", OnCopy);
            //SetCallback("Knapsack_Button", OnBag);
            SetCallback("Skill_Button", OnSkill);
            SetCallback("NormalATKButton", OnTalk);
            SetCallback("StoreButton", OnStore);
            this.regEvt = new System.Collections.Generic.List<MyEvent.EventType>(){
                MyEvent.EventType.UpdateItemCoffer,
                MyEvent.EventType.UpdateMainUI,
                MyEvent.EventType.UpdatePlayerData,
            };
            RegEvent();
        }
        void UpdateFrame(){
            hpLabel.text = GameInterface_Backpack.GetHpNum().ToString(); 
            var lev = GameInterface_Player.GetLevel();
            Log.GUI("lev "+lev );
            level.text = "[ff9500]等级:"+lev+"[-]";
        }
        protected override void OnEvent(MyEvent evt)
        {
            Log.GUI("OnEvent "+evt.type);
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
            WindowMng.windowMng.PushView("UI/SkillUI", true);
            MyEventSystem.myEventSystem.PushEvent(MyEvent.EventType.UpdateSkill);
        }

        void OnStore(GameObject g){
            WindowMng.windowMng.PushView ("UI/StoreUI", true);
            MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.PackageItemChanged);
        }

    }
}
