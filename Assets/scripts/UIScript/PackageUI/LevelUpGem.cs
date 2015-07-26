using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class LevelUpGem : IUserInterface
    {
        BackpackData backpackData;
        public void SetData(BackpackData bd) {
            backpackData = bd;
        }
        LevelUpEquipRight right;
        void Awake()
        {
            right = GetName("Right").GetComponent<LevelUpEquipRight>();
            right.PutInGem = PutInGem;
            
        }

        void PutInGem(BackpackData bd){
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