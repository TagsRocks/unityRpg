using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	//0.8s 击退怪物 buff
	public class KnockBack : IEffect
	{
		//float UpSpeed = 1;
		//float KnockBackTime = 0.7f;
		//float KnockBackSpeed = 22;
		//float Gravity = 20;
		//float StopKnockTime = 0.1f;

		public override void Init (Affix af, GameObject o)
		{
			base.Init (af, o);
			type = Affix.EffectType.KnockBack;
		}


		public override void OnActive ()
		{
			Log.AI ("KnockBack Buff Active");
			obj.GetComponent<MyAnimationEvent> ().KnockBackWho (attacker);
		}

	}

}