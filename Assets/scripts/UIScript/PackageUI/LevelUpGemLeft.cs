using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class LevelUpGemLeft : IUserInterface
    {
        void Awake()
        {
            SetCallback("LevelUp", OnLevelUp);
        }
        void OnLevelUp(){
            
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
