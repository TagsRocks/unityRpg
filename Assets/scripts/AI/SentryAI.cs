using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	public class SentryAI : AIBase
	{
		void Awake() {
			attribute = GetComponent<NpcAttribute>();
			ai = new MonsterCharacter ();
			ai.attribute = attribute;
			ai.AddState (new SentryIdle ());
			ai.AddState(new SentrySkill());
			ai.AddState (new SentryDead ());
		}

		void Start() {

			ai.ChangeState (AIStateEnum.IDLE);
		}
	}
}
