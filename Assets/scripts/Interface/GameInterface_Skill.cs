using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public class GameInterface_Skill
    {
        public static GameInterface_Skill skillInterface = new GameInterface_Skill();

        public List<SkillFullInfo> GetActiveSkill()
        {
            return SkillDataController.skillDataController.activeSkillData;
        }

        public List<SkillFullInfo> GetPassitiveSkill()
        {
            return SkillDataController.skillDataController.passive;
        }

        public void SkillLevelUp(int skId)
        {
            //SkillDataController.skillDataController.StartCoroutine (
            SkillDataController.skillDataController.SkillLevelUpWithSp(skId);
            //);
        }

        public void ResetSkill(int skId)
        {
            SkillDataController.skillDataController.StartCoroutine(SkillDataController.skillDataController.DownLevelSkill(skId));
        }

        public void SetSkillShortCut(int skId, int index)
        {
            SkillDataController.skillDataController.StartCoroutine(SkillDataController.skillDataController.SetSkillShortCut(skId, index));
        }

        public SkillData GetSkillData(int skillId, int level)
        {
            return Util.GetSkillData(skillId, level);
        }

        public SkillData GetShortSkillData(int shortId)
        {
            var shortData = SkillDataController.skillDataController.GetShortSkillData(shortId);
            Log.Sys("GetShortSkillData " + shortId + " d " + shortData);
            return shortData;
        }

        /// <summary>
        /// 从ShortSkillData中获取技能的位置比较可靠 
        /// </summary>
        /// <returns>The skill position.</returns>
        public static int GetSkillPos(int skillId)
        {
            for (var i = 0; i < 4; i++)
            {
                var sk = GameInterface_Skill.skillInterface.GetShortSkillData(i);
                if (sk != null)
                {
                    if (sk.Id == skillId)
                    {
                        return i + 1;
                    }
                }
            }
            return 0;
        }


        public string GetSkillDesc(SkillData sk)
        {
            var str = sk.SkillDes + "\n";
            str += string.Format("额外增加{0}武器伤害", sk.WeaponDamagePCT);
            //其它效果
            return str;
        }

        public int GetLeftSp()
        {
            return SkillDataController.skillDataController.TotalSp - SkillDataController.skillDataController.DistriSp;
        }

        public int DistriSp()
        {
            return SkillDataController.skillDataController.DistriSp;
        }

        /// <summary>
        ///我的角色使用技能 
        /// </summary>
        /// <param name="skillId">Skill identifier.</param>
        public static void MeUseSkill(int skillId)
        {
            Log.Sys("MeUseSkill " + skillId);
            if (skillId == 0)
            {
                return;
            }
            Log.GUI("ItemUseSkill " + skillId);
            var skillData = Util.GetSkillData(skillId, 1);
            UseSkill(skillData);

        }
        /// <summary>
        /// 本地使用技能同时通知代理
        /// 绕过LogicCommand 本地执行不需要LogicCommand队列 
        /// </summary>
        /// <param name="skillData">Skill data.</param>
        static void UseSkill(SkillData skillData) {
            ObjectManager.objectManager.GetMyPlayer().GetComponent<MyAnimationEvent>().OnSkill(skillData);

            NetDateInterface.FastMoveAndPos();
            NetDateInterface.FastUseSkill(skillData.Id);
        }

        /// <summary>
        /// UI点击使用技能 
        /// </summary>
        /// <param name="skIndex">Sk index.</param>
        public static void OnSkill(int skIndex)
        {
            var skillData = SkillDataController.skillDataController.GetShortSkillData(skIndex);
            if (skillData != null)
            {
                var mana = ObjectManager.objectManager.GetMyData().GetProp(CharAttribute.CharAttributeEnum.MP);
                var cost = skillData.ManaCost;
                if (cost > mana)
                {
                    WindowMng.windowMng.ShowNotifyLog("魔法不足");
                    return;
                }
                var npc = ObjectManager.objectManager.GetMyAttr();
                npc.ChangeMP(-cost);
                //ObjectManager.objectManager.GetMyPlayer().GetComponent<MyAnimationEvent>().OnSkill(skillData);
                UseSkill(skillData);
            }
        }

        public static void UpdateSkillPoint(GCPushSkillPoint sp)
        {
            SkillDataController.skillDataController.TotalSp = sp.SkillPoint;
            MyEventSystem.myEventSystem.PushEvent(MyEvent.EventType.UpdateSkill);
        }

        public static void UpdateLevel(GCPushLevel lev)
        {
            var charInfo = ObjectManager.objectManager.GetMyAttr();
            charInfo.ChangeLevel(lev.Level);
        }

        public static void UpdateShortcutsInfo(GCPushShortcutsInfo inpb)
        {
            SkillDataController.skillDataController.UpdateShortcutsInfo(inpb.ShortCutInfoList);
        }
    }
}
