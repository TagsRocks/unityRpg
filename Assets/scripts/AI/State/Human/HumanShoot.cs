using UnityEngine;
using System.Collections;
using MyLib;

public class HumanShoot : AIState
{
    private SkillFullInfo activeSkill;

    public override IEnumerator RunLogic()
    {
        var skillPart = GetSkill();
        skillPart.SetDefaultActive();
        activeSkill = skillPart.GetActiveSkill();
        Log.Sys("HumanShoot: "+activeSkill.skillData.SkillName);

        var skillStateMachine = SkillLogic.CreateSkillStateMachine(GetAttr().gameObject, activeSkill.skillData, GetAttr().transform.position);

        var attackAniName = "rslash_1"; 

        var realAttackTime = activeSkill.skillData.AttackAniTime / GetAttr().GetSpeedCoff();
        var rate = GetAttr().GetComponent<Animation>() [attackAniName].length / realAttackTime;
        var waitTime = realAttackTime * 0.8f;
        PlayAni(attackAniName, rate, WrapMode.Once);
        var passTime = 0.0f;
        do
        {
            if (CheckEvent())
            {
                break;
            }

            if (passTime >= waitTime)
            {
                break;
            }
            passTime += Time.deltaTime;
            yield return null;
        } while(!quit);

        skillStateMachine.Stop();
    }
}
