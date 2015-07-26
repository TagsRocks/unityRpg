using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public class LevelUpEquipRight : IUserInterface
    {
        /*
        public LevelUpEquip parent{
            private get; set;
        }
        */
        public System.Action<BackpackData> PutInGem;
        List<GameObject> Cells = new List<GameObject>();
        UIGrid Grid;
        GameObject Cell;

        void Awake()
        {
            Grid = GetName("Grid").GetComponent<UIGrid>();
            Cell = GetName("Cell");
            regEvt = new System.Collections.Generic.List<MyEvent.EventType>() {
                MyEvent.EventType.UpdateItemCoffer,
                MyEvent.EventType.PackageItemChanged,
            };
            RegEvent();
        }

        List<BackpackData> gems = new List<BackpackData>();

        public void SetGems(List<BackpackData> g)
        {
            gems = g;
            UpdateFrame();
        }

        bool CheckContainGem(long id)
        {
            foreach (var g in gems)
            {
                if (g.id == id)
                {
                    return true;
                }
            }
            return false;
        }

        void UpdateFrame()
        {
            foreach (var c in Cells)
            {
                GameObject.Destroy(c);
            }
            Cell.SetActive(false);
            for (int i = 0; i < BackPack.MaxBackPackNumber; i++)
            {
                var item = PlayerPackage.playerPackage.EnumItem(PlayerPackage.PackagePageEnum.All, i);
                var temp = item;
                if (item != null && item.itemData.IsGem())
                {
                    if (CheckContainGem(item.id))
                    {
                    } else
                    {
                        var c = GameObject.Instantiate(Cell) as GameObject;
                        c.transform.parent = Cell.transform.parent;
                        Util.InitGameObject(c);
                        c.SetActive(true);
                        var pak = c.GetComponent<PackageItem>();
                        pak.SetData(item);
                        pak.SetButtonCB(delegate()
                        {
                            PutInGem(temp);
                        });
                        Cells.Add(c);
                    }
                }
            }
        }

        protected override void OnEvent(MyEvent evt)
        {
            UpdateFrame();
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