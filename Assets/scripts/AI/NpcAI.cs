
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/

/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
namespace ChuMeng {
	public class NpcAI : AIBase {
		MyAnimationEvent myAnimationEvent;


		//CharacterController controller;
		float heading;
		Vector3 targetRotation;

		//CommonAI commonAI;
		
		void Awake() {
			//commonAI = GetComponent<CommonAI> ();
			attribute = GetComponent<NpcAttribute>();

			myAnimationEvent = GetComponent<MyAnimationEvent>();
			//controller = GetComponent<CharacterController>();
			heading = Random.Range(0, 360);
			transform.eulerAngles = new Vector3(0, heading, 0);

			//NGUITools.AddMissingComponent<ShadowComponent> (gameObject);
			//GetComponent<ShadowComponent> ().CreateShadowPlane ();



			ai = new MonsterCharacter ();
			ai.attribute = attribute;
			ai.AddState (new MonsterIdle ());
			ai.AddState (new MonsterCombat ());
			ai.AddState (new MonsterDead ());
			ai.AddState (new MonsterFlee ());
			ai.AddState (new MonsterKnockBack ());
		}
		void Start() {
			myAnimationEvent.RegFlee ();
			myAnimationEvent.RegKnockBack ();
			ai.ChangeState (AIStateEnum.IDLE);
		}

		protected override void OnDestroy() {
			if (attribute.IsDead) {
				Util.ClearMaterial (gameObject);
			}
		}

	}
}
