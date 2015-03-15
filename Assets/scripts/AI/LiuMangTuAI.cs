using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	public class LiuMangTuAI : AIBase
	{
		void Awake() {
			attribute = GetComponent<NpcAttribute>();

			ai = new MonsterCharacter ();
			ai.attribute = attribute;
			ai.AddState (new MonsterIdle ());
			ai.AddState (new LiuMangTuCombat ());
			ai.AddState (new MonsterDead ());
			ai.AddState (new MonsterFlee ());
			ai.AddState (new MonsterKnockBack ());
		}
		void Start() {
			GetComponent<MyAnimationEvent>().RegKnockBack ();
			ai.ChangeState (AIStateEnum.IDLE);
		}

		void OnDestroy() {
			if (attribute.IsDead) {
				Util.ClearMaterial (gameObject);
			}
		}
	}

}