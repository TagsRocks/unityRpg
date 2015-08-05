using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class Map101 : CScene
    {
        void  Start()
        {
            var step = GameInterface_Player.GetIntState(GameBool.cunZhangState);
            Log.Sys("Current Step is "+step);
            if(step == 1) {
                StartCoroutine(CreateZhiRuoAndDongHu());
            }
        }

        IEnumerator CreateZhiRuoAndDongHu() {
            GameObject  myplayer = null;
            while(myplayer == null) {
                yield return new WaitForSeconds(1);
                myplayer = ObjectManager.objectManager.GetMyPlayer();
            }
            var myp = new GameObject("TempNpcPos");
            yield return new WaitForSeconds(1);
            myp.transform.position = myplayer.transform.position+ new Vector3(Random.Range(-3.0f, 3.0f), 0.2f, Random.Range(-3.0f, 3.0f)); 
            ObjectManager.objectManager.CreateNpc(Util.GetNpcData(20008), myp);
            yield return new WaitForSeconds(1);
            ObjectManager.objectManager.CreateNpc(Util.GetNpcData(20009), myp);

            string[] text = new string[]{
                "东湖：这里真的好黑呀...",
                "至若：{0}哥哥,我好怕",
                "别怕至若，我会保护你的，跟紧我。",
                "大家和我一起向前走吧",
            };
            var c = 0;
            foreach(var t in text) {
                text[c] = string.Format(t, ObjectManager.objectManager.GetMyName());
                c++;
            }
            NpcDialogInterface.ShowTextList(text, null);
        }
    
    }
}
