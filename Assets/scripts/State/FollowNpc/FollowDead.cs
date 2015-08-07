using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class FollowDead : DeadState 
    {
        float KnockFlyTime = 0.3f;
        float FlySpeed = 6;
        float StopKnockTime = 0.2f;
        public override void EnterState ()
        {
            base.EnterState ();
            var rd1 = Random.Range(1, 3);
            BackgroundSound.Instance.PlayEffect("burrowerdeath"+rd1);
        }


        public override IEnumerator RunLogic ()
        {
            var deathBlood = Resources.Load<GameObject> ("particles/swordhit");
            GameObject g = GameObject.Instantiate (deathBlood) as GameObject;
            g.transform.parent = SaveGame.saveGame.EffectMainNode.transform;
            g.transform.position = GetAttr().transform.position;
            if ( CheckAni("transform")) {
                GetAttr().animation.CrossFade ("transform");
            }
            yield return new WaitForSeconds(2);
        }

    }
}
