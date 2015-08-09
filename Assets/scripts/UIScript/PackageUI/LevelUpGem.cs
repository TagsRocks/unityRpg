using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public class LevelUpGem : IUserInterface
    {
        BackpackData backpackData;
        public void SetData(BackpackData bd) {
            backpackData = bd;
        }
        LevelUpEquipRight right;
        List<BackpackData> gems = new List<BackpackData>();
        LevelUpGemLeft left;
        void Awake()
        {
            SetCallback("closeButton", Hide);
            right = GetName("Right").GetComponent<LevelUpEquipRight>();
            right.PutInGem = PutInGem;
            left = GetName("Left").GetComponent<LevelUpGemLeft>();
            left.TakeOffGem = TakeOffGem;
            left.LevelUp = LevelUp;
            regEvt = new List<MyEvent.EventType>() {
                MyEvent.EventType.UpdateItemCoffer,
            };
            RegEvent();
        }
        protected override void OnEvent(MyEvent evt)
        {
            gems.Clear();
            left.SetGems(gems);
            right.SetGems(gems);
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
        void TakeOffGem(BackpackData data) {
            gems.Remove(data);
            left.SetGems(gems);
            right.SetGems(gems);
        }

        void LevelUp() {
            if(gems.Count < 2){
                WindowMng.windowMng.ShowNotifyLog("需要放入两个宝石才能升级!");
            }else {
                var lev = 0;
                foreach(var g in gems){
                    if(g.itemData.Level != lev && lev != 0){
                        WindowMng.windowMng.ShowNotifyLog("宝石品阶不同不能合成");
                        return;
                    }else {
                        lev = g.itemData.Level;
                    }
                }
                var target = GameInterface_Package.GetAllLevGems(lev+1);
                if(target.Count == 0){
                    WindowMng.windowMng.ShowNotifyLog("该宝石无法合成");
                }else {
                    GameInterface_Package.playerPackage.LevelUpGem(gems);
                }
            }
        }
 
    }

}