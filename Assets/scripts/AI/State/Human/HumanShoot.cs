using UnityEngine;
using System.Collections;
using MyLib;

public class HumanShoot : AIState
{
    private SkillFullInfo activeSkill;
    public SkillAction skillAction;

    public override IEnumerator RunLogic()
    {
        var skillPart = GetSkill();
        skillPart.SetDefaultActive();
        activeSkill = skillPart.GetActiveSkill();
        Log.Sys("HumanShoot: "+activeSkill.skillData.SkillName);

        var pos = NetworkUtil.FloatPos(skillAction.X, skillAction.Y, skillAction.Z);
        var dir = skillAction.Dir;
        var forward = Quaternion.Euler(new Vector3(0, dir, 0)) * Vector3.forward;

        var skillStateMachine = SkillLogic.CreateSkillStateMachine(GetAttr().gameObject, activeSkill.skillData, pos);
        skillStateMachine.SetForwardDirection(forward);

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
