using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public class LevelUpEquip : IUserInterface
    {
        EquipData equip;
        public LevelUpEquipLeft left;
        public LevelUpEquipRight right;

        List<BackpackData> gems = new List<BackpackData>();
        void Awake()
        {
            SetCallback("closeButton", Hide);
            right = GetName("Right").GetComponent<LevelUpEquipRight>();
            right.PutInGem = PutInGem ;
            left = GetName("Left").GetComponent<LevelUpEquipLeft>();
            left.parent = this;

        }
        public void SetEquip(EquipData ed) {
            equip = ed;
        }
        void Start() {
            left.SetEquip(equip);
        }

        public void PutInGem(BackpackData data)
        {
            if(gems.Count >= 2){
                WindowMng.windowMng.ShowNotifyLog("宝石已满，需要取下宝石才能放入新的!");
            }else {
                gems.Add(data);
                left.SetGems(gems);
                right.SetGems(gems);
            }
        }
        public void TakeOffGem(BackpackData data) 
        {
            gems.Remove(data);
            left.SetGems(gems);
            right.SetGems(gems);
        }
        public void LevelUp(){
            if(gems.Count < 2){
                WindowMng.windowMng.ShowNotifyLog("需要放入两个宝石才能升级!");
            }else {
                PlayerPackage.playerPackage.LevelUpEquip(equip, gems);
            }
        }

    }
}
