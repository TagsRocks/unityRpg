using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public partial class Map2 : CScene 
    {
        void TalkToZhiRuo() {
            var step = GameInterface_Player.GetIntState(GameBool.cunZhangState);
            if(step == 0) {
                if(!GameInterface_Player.GetGameState(GameBool.zhiruo1)) {
                    StartCoroutine(ZhiRuo1());
                }else {
                    ZhiRuo2();
                }
            }else if(step == 1){
                var c = new List<string>() {
                    GameBool.zhiruo3,
                };
                if(CheckCondition(c)) {
                    StartCoroutine(ZhiRuo4());
                }else {
                    StartCoroutine(ZhiRuo3());    
                }
            }
        }
        IEnumerator  ZhiRuo3() {
            string[] text = new string[]{
                "至若要和我一起去，找宝贝么？", 
                "好的哥哥。",
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
            
            GameInterface_Player.SetGameState(GameBool.zhiruo3, true);
            WindowMng.windowMng.PopView();
        }
        IEnumerator ZhiRuo4() {
            string[] text = new string[]{
                "哥哥我准备好，咱们快出发吧，不要被我阿婆发现了。", 
            };
            var npcDialog = WindowMng.windowMng.PushView("UI/NpcDialog", false);
            var dia = npcDialog.GetComponent<NpcDialog>();
            dia.ShowText(string.Format(text [0], ObjectManager.objectManager.GetMyName()));
            yield return null;
        }

        IEnumerator ZhiRuo1() {
            string[] text = new string[]{
                @"{0}哥哥,叔叔和婶婶应该不久就会回来，你别难过哦。", 
                @"至若，等我变厉害了，我就出村去找他们。",
                @"{0}哥哥，我也想出村去看看，听村长爷爷说，村外有很多神奇的东西，还有好多好多人呢。",
                @"恩没问题,到时候我一定带你一起出去",
                @"哥哥，你真好。",
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
            
            GameInterface_Player.SetGameState(GameBool.zhiruo1, true);
            WindowMng.windowMng.PopView();
        }
        void ZhiRuo2() {
            string[] text = new string[]{
                @"{0}哥哥,你去看看东湖哥哥吧，他好像在等你。", 
            };
            var npcDialog = WindowMng.windowMng.PushView("UI/NpcDialog", false);
            var dia = npcDialog.GetComponent<NpcDialog>();
            dia.ShowText(string.Format(text[0], ObjectManager.objectManager.GetMyName())); 

        }

    
    }

}