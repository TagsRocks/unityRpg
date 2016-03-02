using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class AddMP : IEffect 
    {
        public override void Init(Affix af, GameObject o)
        {
            base.Init(af, o);
            type = Affix.EffectType.AddMP;
            BackgroundSound.Instance.PlayEffect("healdrink");
        }

        IEnumerator Add(){
            var period = 2.0f;
            var num = 20;
            var goneTime = 0.0f;
            var npcAttr = obj.GetComponent<NpcAttribute>();
            while(!IsDie) {
                if(goneTime > period) {
                    npcAttr.ChangeMP(num);
                    goneTime -= period;
                }
                goneTime += Time.deltaTime;
                yield return null;
            }
            Log.Sys("AddBUff over");
        }

        public override void OnActive()
        {
            base.OnActive();
            var buff = obj.GetComponent<BuffComponent>();
            buff.StartCoroutine(Add());
        }
    }

}