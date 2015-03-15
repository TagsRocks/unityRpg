using UnityEngine;
using System.Collections;

namespace ChuMeng {
	public class MonsterCombat : CombatState {
		float FastRotateSpeed = 10;
		float WalkSpeed = 3;
		float RunSpeed = 5;
		
		public override void EnterState() {
			base.EnterState();
			SetAttrState (CharacterState.Attacking);	
			WalkSpeed = GetAttr ().WalkSpeed;
			RunSpeed = WalkSpeed*2;
		}


		IEnumerator CastSkill(GameObject targetPlayer) {
			GetAttr ().GetComponent<SkillInfoComponent> ().SetRandomActive ();
			var activeSkill = GetAttr().GetComponent<SkillInfoComponent>().GetActiveSkill();
			var skillStateMachine = SkillLogic.CreateSkillStateMachine (GetAttr().gameObject, activeSkill.skillData, GetAttr().transform.position, targetPlayer);

			//SetAni("attack", 1, WrapMode.Once);

			Log.AI ("Skill SetAni "+activeSkill.skillData.AnimationName);

			var realAttackTime = activeSkill.skillData.AttackAniTime;
			var rate = GetAttr().animation[activeSkill.skillData.AnimationName].length/realAttackTime;
			SetAni (activeSkill.skillData.AnimationName, rate, WrapMode.Once);

			while(GetAttr().animation.isPlaying && !quit) {
				if(CheckEvent()) {
					yield break;
				}

				//自动向目标旋转
				Vector3 dir = targetPlayer.transform.position - GetAttr ().transform.position;
				dir.y = 0;
				var rotation = Quaternion.LookRotation (dir);
				GetAttr ().transform.rotation = Quaternion.Slerp (GetAttr ().transform.rotation, rotation, Time.deltaTime * FastRotateSpeed);
				yield return null;
			}

		}

		public override IEnumerator RunLogic ()
		{
			var physic = GetAttr ().GetComponent<PhysicComponent> ();

			var targetPlayer = ObjectManager.objectManager.GetMyPlayer();
			GetAttr().GetComponent<CommonAI>().SetTargetPlayer(targetPlayer);
			while(!quit) {
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
					
					//GetController().SimpleMove(forward*RunSpeed);
					physic.MoveSpeed(forward*RunSpeed);
					yield return null;
				}

				yield return GetAttr().StartCoroutine(CastSkill(targetPlayer));

			}
		}

	}
}
