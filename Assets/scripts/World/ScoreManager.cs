using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyLib
{
    public struct ScoreData {
        public int killed;
        public int beKilled;
    }
    /// <summary>
    /// 统计得分
    /// 倒计时
    /// 显示游戏结束面板 
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        public float leftTime;
        public Dictionary<int, ScoreData> score = new Dictionary<int, ScoreData>();
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