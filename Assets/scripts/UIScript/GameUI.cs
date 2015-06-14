using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class GameUI : IUserInterface
    {
        UILabel hpLabel;
        void Awake()
        {
            hpLabel = GetLabel("HPNum");
            SetCallback("HPBottle", OnBottle);

            this.regEvt = new System.Collections.Generic.List<MyEvent.EventType>(){
                MyEvent.EventType.UpdateItemCoffer,
                MyEvent.EventType.UpdateMainUI,
            };
            RegEvent();
        }
        void OnBottle(GameObject g){
            GameInterface_Backpack.UseItem(101);
        }

        void UpdateFrame(){
            hpLabel.text = GameInterface_Backpack.GetHpNum().ToString(); 
        }

        protected override void OnEvent(MyEvent evt)
        {
            UpdateFrame();
        }
    }

}