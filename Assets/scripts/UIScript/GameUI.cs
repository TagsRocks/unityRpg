using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class GameUI : IUserInterface
    {
        UILabel HpLabel;
        UILabel mpLabel;
        UISlider hp;
        UISlider mp;


        UILabel hpLabel;
        void Awake()
        {

            HpLabel = GetLabel ("HPLabel");
            mpLabel = GetLabel ("MPLabel");
            hp = GetSlider ("HP");
            mp = GetSlider ("MP");

            hpLabel = GetLabel("HPNum");
            SetCallback("HPBottle", OnBottle);

            this.regEvt = new System.Collections.Generic.List<MyEvent.EventType>(){
                MyEvent.EventType.UpdateItemCoffer,
                MyEvent.EventType.UpdateMainUI,

            };
            /*
            var view = NGUITools.AddMissingComponent<KBEngine.KBNetworkView>();
            view.SetLocalId(ObjectManager.objectManager.GetMyLocalId());

            this.regLocalEvt = new System.Collections.Generic.List<MyEvent.EventType>(){
                MyEvent.EventType.UnitHP,
                MyEvent.EventType.UnitHPPercent,
                MyEvent.EventType.UnitMP,
                MyEvent.EventType.UnitMPPercent,
                MyEvent.EventType.UpdatePlayerData,
                MyEvent.EventType.UnitExp,
            };
            */
            RegEvent();
            SetCallback("Close", OnQuit);
        }
        void OnQuit(GameObject g){
            WindowMng.windowMng.ShowDialog(delegate(bool ret){
                if(ret){
                    WorldManager.worldManager.WorldChangeScene(2, false);
                }else {
                }
            }); 
        }
        void OnBottle(GameObject g){
            GameInterface_Backpack.UseItem(101);
        }

        void UpdateFrame(){
            hpLabel.text = GameInterface_Backpack.GetHpNum().ToString(); 

            var me = ObjectManager.objectManager.GetMyData();
            HpLabel.text = me.GetProp(CharAttribute.CharAttributeEnum.HP).ToString ()+"/"+me.GetProp(CharAttribute.CharAttributeEnum.HP_MAX).ToString();
            mpLabel.text = me.GetProp(CharAttribute.CharAttributeEnum.MP).ToString ()+"/"+me.GetProp(CharAttribute.CharAttributeEnum.MP_MAX).ToString();

            hp.value = me.GetProp(CharAttribute.CharAttributeEnum.HP)*1.0f/me.GetProp(CharAttribute.CharAttributeEnum.HP_MAX);
            mp.value = me.GetProp(CharAttribute.CharAttributeEnum.MP)*1.0f/me.GetProp(CharAttribute.CharAttributeEnum.MP_MAX);
        }

        protected override void OnEvent(MyEvent evt)
        {
            UpdateFrame();
        }
    }

}