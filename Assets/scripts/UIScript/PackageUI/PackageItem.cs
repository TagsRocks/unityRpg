using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class PackageItem : IUserInterface
    {
        UILabel Name;
        EquipData equipData;
        BackpackData backpack;
        EmptyDelegate cb;
        void Awake()
        {
            SetCallback("Info", OnInfo);
            Name = GetLabel("Name");
        }
        void OnInfo(GameObject g){
            if(cb != null) {
                cb();
            }else {
                var win = WindowMng.windowMng.PushView("UI/DetailInfo");
                var detail = win.GetComponent<DetailInfo>();
                detail.equipData = equipData;
                detail.backpackData = backpack;
            }
        }
        public void SetButtonCB(EmptyDelegate c){
            cb = c;
        }
        public void SetEquipData(EquipData equip) {
            equipData = equip;
            Name.text = string.Format("[ff9500]{0}(1级)[-]", equip.itemData.ItemName); 
        }
        public void SetData(BackpackData data)
        {
            backpack = data;
            if(data.itemData.IsEquip()) {
                Name.text = string.Format("[ff9500]{0}(1级)[-]", data.GetTitle());
            }else if(data.itemData.IsGem()){
                Name.text = string.Format("[ff9500]{0}({1}阶)[-]\n[0098fc]数量：{2}[-]", data.GetTitle(), data.itemData.Level, data.num);
            }else {
                Name.text = string.Format("[ff9500]{0}[-]\n[0098fc]数量：{1}[-]", data.GetTitle(), data.num);
            }
        }
    }

}