using UnityEngine;
using System.Collections;
using MyLib;

public class MoveAndShoot : MonoBehaviour
{
    NpcAttribute attr; 
    PhysicComponent physic;
    IEnumerator Start() {
        attr = GetComponent<NpcAttribute>();
        physic = GetComponent<PhysicComponent>();

        yield return null;
        var playerMove = GetComponent<MoveController> ();
        var vcontroller = playerMove.vcontroller;
        var camRight = playerMove.camRight;
        var camForward = playerMove.camForward;



        var eh = NGUITools.AddMissingComponent<EvtHandler>(this.gameObject);
        eh.AddEvent(MyEvent.EventType.EnterShoot, (evt)=>{
        });

        eh.AddEvent(MyEvent.EventType.ExitShoot, (evt)=>{
        });

        eh.AddEvent(MyEvent.EventType.CancelShoot, (evt)=>{
        });

        eh.AddEvent(MyEvent.EventType.ShootDir, (evt)=>{
            if(!attr.IsDead) {
                var dir = new Vector3(evt.vec2.x, 0, evt.vec2.y);
                physic.TurnTo(dir);
            }
        });
    }
  
	
   
}
