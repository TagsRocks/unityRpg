using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public class LevelUpEquipLeft : IUserInterface
    {
        public LevelUpEquip parent;
        EquipData equipData;

        List<GameObject> Cells = new List<GameObject>();
        UIGrid Grid;
        GameObject Cell;
        List<BackpackData> gems = new List<BackpackData>();
        public void SetGems(List<BackpackData> g) {
            gems = g;
            UpdateFrame();
        }
        public void SetEquip(EquipData equip)
        {
            equipData = equip;
            UpdateFrame();
        }

        UILabel Name;

        void Awake()
        {
            Grid = GetName("Grid").GetComponent<UIGrid>();
            Cell = GetName("Cell");

            Name = GetLabel("Name");
            SetCallback("closeButton", Hide);
            SetCallback("LevelUp", OnLevelUp);
            regEvt = new System.Collections.Generic.List<MyEvent.EventType>() {
                MyEvent.EventType.UpdateItemCoffer,
                MyEvent.EventType.PackageItemChanged,
            };
            RegEvent();
        }

        protected override void OnEvent(MyEvent evt)
        {
            UpdateFrame();
        }
        void OnLevelUp()
        {
            parent.LevelUp();
        }

        void UpdateFrame()
        {
            string baseAttr = "";
            if (equipData.itemData.Damage > 0)
            {
                baseAttr += string.Format("[9800fc]攻击力:{0}[-]\n", equipData.itemData.Damage);
            }
            if (equipData.itemData.RealArmor > 0)
            {
                baseAttr += string.Format("[8900cf]防御力:{0}[-]\n", equipData.itemData.RealArmor);
            }
            string extarAttr = "";
            //[fc0000]额外攻击:1000[-]\n[fc0000]额外防御:100[-]\n

            Name.text = string.Format("[ff9500]{0}({1}级)[-]\n[0098fc]{2}金币[-]\n{3}{4}[fcfc00]{5}[-]",
                    equipData.itemData.ItemName, 
                    1,
                    equipData.itemData.GoldCost,
                    baseAttr,
                    extarAttr,
                    equipData.itemData.Description);
            InitList();
        }
        void InitList(){
            foreach(var c in Cells){
                GameObject.Destroy(c);
            }
            Cell.SetActive(false);

            for(int i = 0; i < gems.Count; i++) {
                var item = gems[i];
                var temp = item;
                if(item != null) {
                    var c = GameObject.Instantiate(Cell) as GameObject;
                    c.transform.parent = Cell.transform.parent;
                    Util.InitGameObject(c);
                    c.SetActive(true);
                    var pak = c.GetComponent<PackageItem>();
                    pak.SetData(item);
                    pak.SetButtonCB(delegate(){
                        parent.TakeOffGem(temp);
                    });
                    Cells.Add(c);

                }

            }

            Grid.repositionNow = true;
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