using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	public class LiuMangTuCombat : CombatState
	{
		float FastRotateSpeed = 10;
		float WalkSpeed = 3;
		float RunSpeed = 5;


		int attackId = 0;
		//string attackAniName = null;
		public override void EnterState ()
		{
			base.EnterState ();
			attackId = 0;
		}

	

		public override IEnumerator RunLogic (){
			var physic = GetAttr ().GetComponent<PhysicComponent> ();

			var targetPlayer = ObjectManager.objectManager.GetMyPlayer();
			GetAttr().GetComponent<CommonAI>().SetTargetPlayer(targetPlayer);
			while (!quit) {
				if(CheckEvent()) {
					yield break;
				}
				float passTime = 0;
				aiCharacter.SetRun();
				//Face To Target
				while(!quit) {
					Vector3 dir = targetPlayer.transform.position-GetAttr().transform.position;
					dir.y = 0;
					var rotation = Quaternion.LookRotation(dir);
					GetAttr().transform.rotation = Quaternion.Slerp(GetAttr().transform.rotation, rotation, Mathf.Min(1, Time.deltaTime*FastRotateSpeed));
					break;
				}

				var waitTime = Random.Range(1, 1.5f);
				//Move Around Player
				aiCharacter.SetRun();
				while(passTime < waitTime) {
					if(CheckEvent()) {
						yield break;
					}
					if(CheckFlee()) {
						aiCharacter.ChangeState(AIStateEnum.FLEE);
						yield break;
					}
					Vector3 dir = targetPlayer.transform.position-GetAttr().transform.position;
					dir.y = 0;
					var rotation = Quaternion.LookRotation(dir);
					GetAttr().transform.rotation = Quaternion.Slerp(GetAttr().transform.rotation, rotation, Time.deltaTime*FastRotateSpeed);
					
					var right = GetAttr().transform.TransformDirection(Vector3.right);
					var back = GetAttr().transform.TransformDirection(Vector3.back);
					if(dir.magnitude > GetAttr().AttackRange*1.2f) {
						back = Vector3.zero;
					}
					//GetController().SimpleMove(right*WalkSpeed+back*WalkSpeed);
					physic.MoveSpeed(right*WalkSpeed+back*WalkSpeed);
					passTime += Time.deltaTime;
					yield return null;
				}

				//Rush to Player Follow
				aiCharacter.SetRun();
				while(!quit) {
					if(CheckEvent()){
						yield break;
					}
					
					if(CheckFlee()) {
						aiCharacter.ChangeState(AIStateEnum.FLEE);
						yield break;
					}
					
					Vector3 dir = targetPlayer.transform.position-GetAttr().transform.position;
					dir.y = 0;
					if(dir.magnitude < GetAttr().AttackRange*0.8f) {
						break;
					}
					var rotation = Quaternion.LookRotation(dir);
					
					GetAttr().transform.rotation = Quaternion.Slerp(GetAttr().transform.rotation, rotation, Time.deltaTime*FastRotateSpeed);
					var forward = GetAttr().transform.TransformDirection(Vector3.forward);
					
					physic.MoveSpeed(forward*RunSpeed);

					yield return null;
				}

				yield return GetAttr().StartCoroutine(DoAttack(targetPlayer));

			}
		}
		//
		IEnumerator DoAttack(GameObject targetPlayer) {

			while (!quit) {
				Vector3 dist = targetPlayer.transform.position-GetAttr().transform.position;
				if(dist.magnitude > GetAttr().AttackRange) {
					break;
				}

				//一段攻击释放一个技能对象
				SetAni (GetAttackAniName (), 1, WrapMode.Once);
				//TODO:boss普通技能 暂时先用默认技能 后续添加随机技能
				GetAttr().GetComponent<SkillInfoComponent>().SetDefaultActive();
				var activeSkill = GetAttr().GetComponent<SkillInfoComponent>().GetActiveSkill();
				//var skillStateMachine = 
                SkillLogic.CreateSkillStateMachine (GetAttr().gameObject, activeSkill.skillData, GetAttr().transform.position, targetPlayer);

				//Attack Player DoHit
				while (!quit&& GetAttr().animation.isPlaying) {
					if (CheckEvent ()) {
						yield break;
					}
				
					//自动向目标旋转
					Vector3 dir = targetPlayer.transform.position - GetAttr ().transform.position;
					dir.y = 0;
					var rotation = Quaternion.LookRotation (dir);
					GetAttr ().transform.rotation = Quaternion.Slerp (GetAttr ().transform.rotation, rotation, Time.deltaTime * FastRotateSpeed);
					yield return null;
				}
				yield return null;
			}
		}

		//三段技能
		string GetAttackAniName ()
		{
			var name = string.Format ("attack{0}", attackId);
			attackId++;
			attackId %= 3;
			return name;
		}

	}
}
