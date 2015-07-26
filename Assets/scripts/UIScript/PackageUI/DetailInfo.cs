using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class DetailInfo : IUserInterface 
    {
        public BackpackData backpackData;
        public EquipData equipData;
        UILabel Name;

        void Awake() {
            Name = GetLabel("Name");
            SetCallback("closeButton", Hide);
            SetCallback("LevelUp", OnLevelUp);
            SetCallback("Equip", OnEquip);
            SetCallback("Sell", OnSell);
        }
        void OnLevelUp() {
            if(equipData != null) {
                var lv = WindowMng.windowMng.PushView("UI/LevelUpEquip");
                var eq = lv.GetComponent<LevelUpEquip>();
                eq.SetEquip(equipData);
            }else { 
                var lv = WindowMng.windowMng.PushView("UI/LevelUpGem");
                var gem = lv.GetComponent<LevelUpGem>();
                gem.SetData(backpackData);
            }
        }
        void OnEquip(){
        }
        void OnSell() {
        }

        // Use this for initialization
        void Start()
        {
            if(equipData != null) {
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
                string extarAttr = "";
                //[fc0000]额外攻击:1000[-]\n[fc0000]额外防御:100[-]\n

                Name.text = string.Format( "[ff9500]{0}({1}级)[-]\n[0098fc]{2}金币[-]\n{3}{4}[fcfc00]{5}[-]",
                    equipData.itemData.ItemName, 
                    1,
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
                    string extarAttr = "";
                    //[fc0000]额外攻击:1000[-]\n[fc0000]额外防御:100[-]\n
                    
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
    
        // Update is called once per frame
        void Update()
        {
    
        }
    }

}