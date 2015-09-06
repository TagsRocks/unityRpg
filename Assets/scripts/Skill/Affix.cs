using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    /// <summary>
    /// Buff的配置参数Key列表 
    /// </summary>
    public enum PairEnum {
        Percent = 0,
        Abs = 1,
    }
    [System.Serializable]
    public struct Pair
    {
        public PairEnum key;
        public string value;
    }

    [System.Serializable]
    public class Affix
    {
        //效果类型
        public enum EffectType
        {
            None,
            SummonDuration, //召唤物持续时间 
            DefenseAdd, //防御增加
            KnockBack, //击退怪物
            AddHPMP, //增加HP和MP
            Fushi, //腐蚀降低防御力
            ReduceHP, //持续降低HP
            Frozen, //冰冻降低移动攻击速度 50% 不能叠加
            FanTan, //反弹伤害
            Ghost, //鬼魂化
            IgnoreCol, //忽略一切的碰撞
        }

        public EffectType effectType = EffectType.None;
        public float Duration = 10;//Buff Time 

        public GameObject UnitTheme;//buff 期间的单位粒子效果
        public Vector3 ThemePos = Vector3.zero;

        //TODO:防御增加的数值 从数值表中读取
        //根据技能等级 以10%的比例上升 或者相对于某条数值曲线 上面取值
        public int addDefense = 0;

        public enum TargetType
        {
            Self,
            Pet,
            Enemy,
        }

        //附加buff到攻击者还是宠物身上
        public TargetType target = TargetType.Self;
        public List<Pair> affixParameters;

        public string GetPara(PairEnum key){
            foreach(var kv in affixParameters){
                if(kv.key == key){
                    return kv.value;
                }
            }
            Debug.LogError("NotFindAffixPara "+effectType+" key "+key);
            return null;
        }

        /// <summary>
        /// 燃烧不能叠加 
        /// </summary>
        public bool IsOnlyOne = false;

    }
}
