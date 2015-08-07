using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public partial class Map2 : CScene
    {
        IEnumerator ShowChapter1StartDialog()
        {
            var npcDialog = WindowMng.windowMng.PushView("UI/NpcDialog", false);
            var dia = npcDialog.GetComponent<NpcDialog>();
            dia.ShowNext = delegate()
            {
                GameInterface_Player.SetGameState(GameBool.chapter1Start, true);
                WindowMng.windowMng.PopView();
            };
            
            string[] text = new string[]{
                @"孩子你父母有重要的事情要做，他们嘱托我照顾你，等你有了力量，就可以去找他们了。你父母走之前留给你一些东西，现在你可以先去村子里转转，一会再过来找我。",
            };
            dia.ShowText(text [0]);
            yield return null;
        }
        
        void ShowCunZhangNormalWord()
        {
            string[] text = new string[]{
                @"{0}，你再耐心等一会，我正在施法。", 
            };
            var npcDialog = WindowMng.windowMng.PushView("UI/NpcDialog", false);
            var dia = npcDialog.GetComponent<NpcDialog>();
            dia.ShowText(string.Format(text [0], ObjectManager.objectManager.GetMyName()));
        }

        bool CheckCondition(List<string> con)
        {
            foreach (var c in con)
            {
                if (!GameInterface_Player.GetGameState(GameBool.chapter1Start))
                {
                    return false;
                }
            }
            return true;
        }

        IEnumerator  CunZhang3()
        {
            string[] text = new string[]{
                "{0},你回来了\n我刚才施法打开了通往试炼之境的密道，你可以去那里，找到你父母留给你的东西。", 
                "巨牙子爷爷，试炼之境是什么地方？",
                "那里是村子的地下的一个秘境，据说是上古通天神魔用无边法力所构建的一个空间，里面居住着一些魔神的子子孙孙，到那后你要小心。",
                "父母留给我什么宝物呢？",
                "这个我也不清楚，应该是拥有通灵之力的神物，只有这种神物，才不会被魔气所侵袭。\n你可以叫上至若和东湖帮你，试炼之境虽不是万分险恶，但那些魔物也不是好惹之徒，你们还是要万分小心的。",
                "恩，多谢巨牙子爷爷。",
            };
            var npcDialog = WindowMng.windowMng.PushView("UI/NpcDialog", false);
            var dia = npcDialog.GetComponent<NpcDialog>();
            bool next = false;
            dia.ShowNext = delegate() {
                next = true;
            };
            
            foreach(var t in text) {
                dia.ShowText(string.Format(t, ObjectManager.objectManager.GetMyName())); 
                while(!next) {
                    yield return new WaitForSeconds(0.1f);
                }
                next = false;
            }
            
            GameInterface_Player.SetIntState(GameBool.cunZhangState, 1);
            WindowMng.windowMng.PopView();
        }

        void CunZhang4()
        {
            string[] text = new string[]{
                @"{0},早去早回呀。", 
            };
            var npcDialog = WindowMng.windowMng.PushView("UI/NpcDialog", false);
            var dia = npcDialog.GetComponent<NpcDialog>();
            dia.ShowText(string.Format(text [0], ObjectManager.objectManager.GetMyName()));
        }
        void CunZhang5(){
            string[] text = new string[]{
                "孩子你终于安全回来了！后面故事待续", 
            };
            NpcDialogInterface.ShowTextList(text, null);
        }
        void TalkToCunZhang()
        {
            Log.GUI("TalkTOCunZhange");
            //未曾开始对话
            if (!GameInterface_Player.GetGameState(GameBool.chapter1Start))
            {
                StartCoroutine(ShowChapter1StartDialog());
            } else
            {
                var step = GameInterface_Player.GetIntState(GameBool.cunZhangState);
                if (step == 0)
                {
                    var c = new List<string>() {
                        GameBool.chapter1Start,
                        GameBool.zhiruo1,
                        GameBool.donghu1,
                        GameBool.qinqing1,
                        GameBool.wanshan1,
                    };
                    if (CheckCondition(c))
                    {
                        StartCoroutine(CunZhang3());
                    } else
                    {
                        ShowCunZhangNormalWord();
                    }
                }else if(step == 1){
                    CunZhang4();
                }else {
                    CunZhang5();
                }
            }
        }
    }

}