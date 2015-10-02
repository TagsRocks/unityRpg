using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public class DetailInfo : IUserInterface 
    {
        private BackpackData _bd;
        public BackpackData backpackData {
            get {
                return _bd;
            }
            set {
                _bd = value;
                InitButton();
            }
        }
        private EquipData equipData;
        UILabel Name;
        GameObject OneKey;
        void Awake() {
            Name = GetLabel("Name");
            SetCallback("closeButton", Hide);
            SetCallback("LevelUp", OnLevelUp);
            SetCallback("Equip", OnEquip);
            SetCallback("Sell", OnSell);
            OneKey = GetName("OneKey");
            SetCallback("OneKey", OnOneKey);
            OneKey.SetActive(false);

            //GetName("Sell").SetActive(false);
            GetName("Equip").SetActive(false);

            regEvt = new System.Collections.Generic.List<MyEvent.EventType>() {
                MyEvent.EventType.UpdateItemCoffer,
                MyEvent.EventType.UpdateDetailUI,
            };
            RegEvent();
        }
        void InitButton() {
            if(equipData != null) {
                OneKey.SetActive(false);
            }else {
                OneKey.SetActive(true);
            }
        }

        void OnOneKey() {
            if(backpackData != null) {
                if(backpackData.entry.Count >= 2) {
                    GameInterface_Package.playerPackage.LevelUpGem(new List<BackpackData>(){backpackData});
                    WindowMng.windowMng.PopView();
                }else {
                    Util.ShowMsg("宝石数量需要大于2个才能合成");
                }
            }
        }

        void OnLevelUp() {
            if(equipData != null) {
                var lev = equipData.entry.Level;
                var needLevel = GMDataBaseSystem.SearchIdStatic<EquipLevelData>(GameData.EquipLevel, lev);
                var myLev = ObjectManager.objectManager.GetMyAttr().Level;
                if(needLevel.level > myLev) {
                    WindowMng.windowMng.ShowNotifyLog(string.Format("只有自身达到{0}级,才能掌控更高级装备", needLevel.level));
                    return;
                }

                var lv = WindowMng.windowMng.PushView("UI/LevelUpEquip");
                var eq = lv.GetComponent<LevelUpEquip>();
                eq.SetEquip(equipData);
            }else { 


                var lv = WindowMng.windowMng.PushView("UI/LevelUpGem");
                var gem = lv.GetComponent<LevelUpGem>();
                gem.SetData(backpackData);
                MyEventSystem.PushEventStatic(MyEvent.EventType.UpdateItemCoffer);
            }
        }
        void OnEquip(){
        }
        public void SetEquip(EquipData ed) {
            equipData = ed;
            InitButton();
        }
        protected override void OnEvent(MyEvent evt)
        {
            if(equipData != null) {
                var allEquip = BackPack.backpack.GetEquipmentData();
                foreach(var e in allEquip) {
                    if(e.id == equipData.id) {
                        SetEquip(e);
                        break;
                    }
                }

                var label = GetLabel("Equip/Label");
                label.text = "卸下";
                GetName("Equip").SetActive(false);
                GetName("Sell").SetActive(false);

                string baseAttr = "";
                if(equipData.itemData.Damage > 0) {
                    baseAttr += string.Format("[9800fc]攻击力:{0}[-]\n", equipData.itemData.Damage);
                }
                if(equipData.itemData.RealArmor > 0) {
                    baseAttr += string.Format("[8900cf]防御力:{0}[-]\n", equipData.itemData.RealArmor);
                }
                //string extarAttr = "";
                string extarAttr = string.Format("[fc0000]额外攻击:{0}[-]\n[fc0000]额外防御:{1}[-]\n", 
                                                 equipData.entry.ExtraAttack,
                                                 equipData.entry.ExtraDefense);
                //[fc0000]额外攻击:1000[-]\n[fc0000]额外防御:100[-]\n

                Name.text = string.Format( "[ff9500]{0}({1}级)[-]\n[0098fc]{2}金币[-]\n{3}{4}[fcfc00]{5}[-]",
                    equipData.itemData.ItemName, 
                    equipData.entry.Level,
                    equipData.itemData.GoldCost,
                    baseAttr,
                    extarAttr,
                    equipData.itemData.Description);
            }else if(backpackData != null) {
                if(backpackData.itemData.IsGem()) {
                    GetName("Equip").SetActive(false); 
                    Name.text = string.Format("[ff9500]{0}({1}阶)[-]\n[0098fc]{2}金币[-]\n[0098fc]数量{3}[-]\n[fcfc00]{4}[-]",
                                backpackData.itemData.ItemName,
                                  backpackData.itemData.Level,
                                  backpackData.itemData.GoldCost,
                                  backpackData.num,
                                  backpackData.itemData.Description
                              );

                }else if(backpackData.itemData.IsProps()) {
                    GetName("Equip").SetActive(false);
                    GetName("LevelUp").SetActive(false);
                    Name.text = string.Format("[ff9500]{0}[-]\n[0098fc]{1}金币[-]\n[0098fc]数量{2}[-]\n[fcfc00]{3}[-]",
                                backpackData.itemData.ItemName,
                                  backpackData.itemData.GoldCost,
                                  backpackData.num,
                                  backpackData.itemData.Description
                              );

                
                }else {
                    string baseAttr = "";
                    if(equipData.itemData.Damage > 0) {
                        baseAttr += string.Format("[9800fc]攻击力:{0}[-]\n", backpackData.itemData.Damage);
                    }
                    if(equipData.itemData.RealArmor > 0) {
                        baseAttr += string.Format("[8900cf]防御力:{0}[-]\n", backpackData.itemData.RealArmor);
                    }
                    string extarAttr = string.Format("[fc0000]额外攻击:{0}[-]\n[fc0000]额外防御:{1}[-]\n", 
                                                     backpackData.entry.ExtraAttack,
                                                     backpackData.entry.ExtraDefense);
                    
                    Name.text = string.Format( "[ff9500]{0}({1}级)[-]\n[0098fc]{2}金币[-]\n{3}{4}[fcfc00]{5}[-]",
                                             backpackData .itemData.ItemName, 
                                              1,
                                              backpackData.itemData.GoldCost,
                                              baseAttr,
                                              extarAttr,
                                              backpackData.itemData.Description); 
                }
            }

        }
        void OnSell() {
            GameInterface_Package.SellItem(backpackData);
            WindowMng.windowMng.PopView();
        }

    }

}