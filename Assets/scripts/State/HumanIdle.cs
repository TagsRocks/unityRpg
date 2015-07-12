using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class HumanIdle : IdleState
	{
        bool first= true;
		public override void EnterState ()
		{
			Log.AI ("Enter Idle State");
			base.EnterState ();
			SetAttrState (CharacterState.Idle);
			aiCharacter.SetIdle ();
            if(first){
                first = false;
                GetAttr().StartCoroutine(CheckFall());
            }
		}

        IEnumerator CheckFall(){
            Vector3 originPos = GetAttr().OriginPos;
            List<Vector3> samplePos = new List<Vector3>(){originPos};
            while(true){
                var lastOne = samplePos[0];
                Log.Sys("lastPos nowPos "+lastOne+" now "+GetAttr().transform.position);
                if(GetAttr().transform.position.y < (lastOne.y-2)){
                    GetAttr().transform.position = lastOne;    
                }else {
                    var pos = GetAttr().transform.position;
                    samplePos.Add(pos);
                    if(samplePos.Count > 4){
                        samplePos.RemoveAt(0);
                    }
                }
                yield return new WaitForSeconds(1);
            }
        }



		public override IEnumerator RunLogic ()
		{
			var playerMove = GetAttr ().GetComponent<MoveController> ();
			var vcontroller = playerMove.vcontroller;

			while (!quit) {
				if(CheckEvent()) {
					yield break;
				}


				float v = 0;
				float h = 0;
				if (vcontroller != null) {
					h = vcontroller.inputVector.x;//CameraRight 
					v = vcontroller.inputVector.y;//CameraForward
				}


				bool isMoving = Mathf.Abs (h) > 0.1 || Mathf.Abs (v) > 0.1;
				if(isMoving) {
					aiCharacter.ChangeState(AIStateEnum.MOVE);
				}

				yield return null;
			}
		}
	}
}