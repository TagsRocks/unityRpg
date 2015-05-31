using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	/// <summary>
	/// 宠物的AI状态机构建
	/// </summary>

	public class PetAI : AIBase
	{
		
		void Awake() {
			attribute = GetComponent<NpcAttribute>();
			ai = new MonsterCharacter ();
			ai.attribute = attribute;
			ai.AddState (new PetIdle ());
            ai.AddState(new PetSkill());
			ai.AddState (new PetDead ());
		}
        void Start() {
            ai.ChangeState (AIStateEnum.IDLE);
        }
		
	}
}
