using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    /// <summary>
    ///主城场景控制类配置 
    /// 配置所有Npc的Interactive事件的处理机制
    /// </summary>
    public class Map2 : CScene 
    {
        public override void Init()
        {
        }

        public override void EnterScene()
        {

        }

        public override void LeaveScene()
        {
            base.LeaveScene();
        }

        public override void ManagerInitOver()
        {
        }


        IEnumerator ShowChapter1StartDialog() {
            var npcDialog = WindowMng.windowMng.PushView("UI/NpcDialog", false);
            var dia = npcDialog.GetComponent<NpcDialog>();
            dia.ShowNext = delegate() {
                GameInterface_Player.SetGameState(GameBool.chapter1Start, true);
                WindowMng.windowMng.PopView();
            };

            string[] text = new string[]{
                @"孩子你父母有重要的事情要做，他们嘱托我照顾你，等你有了力量，就可以去找他们了。你父母走之前留给你一些东西，现在你可以先去村子里转转，一会再过来找我。",
            };
            dia.ShowText(text[0]);
            yield return null;
        }

        void ShowCunZhangNormalWord() {
            string[] text = new string[]{
                @"{0}，你再耐心等一会，我正在施法。", 
            };
            var npcDialog = WindowMng.windowMng.PushView("UI/NpcDialog", false);
            var dia = npcDialog.GetComponent<NpcDialog>();
            dia.ShowText(string.Format(text[0], ObjectManager.objectManager.GetMyName()));
        }

        void TalkToCunZhang() {
            //未曾开始对话
            if(!GameInterface_Player.GetGameState(GameBool.chapter1Start)) {
                StartCoroutine(ShowChapter1StartDialog());
            }else {
                ShowCunZhangNormalWord();
            }
        }
        //Wait For All Npc Init Over
        //Then Set Npc TalkHandler
        IEnumerator Start(){
            yield return new WaitForSeconds(1f);
            var cunZhang = NpcManager.Instance.GetNpc("巨牙子");
            cunZhang.TalkToMe = TalkToCunZhang;

            var miePo = NpcManager.Instance.GetNpc("灭婆");
            miePo.TalkToMe = TalkToMiePo;

            var zhiRuo = NpcManager.Instance.GetNpc("至若");
            zhiRuo.TalkToMe = TalkToZhiRuo;

            var donghu  = NpcManager.Instance.GetNpc("东湖");
            donghu.TalkToMe = TalkToDongHu;

            var qinqing = NpcManager.Instance.GetNpc("秦情");
            qinqing.TalkToMe = TalkToQinQing;

            var wanshan = NpcManager.Instance.GetNpc("万山");
            wanshan.TalkToMe = TalkToWanShan;

            var aniu  = NpcManager.Instance.GetNpc("阿牛");
            aniu.TalkToMe = TalkToANiu;
        }

        void TalkToMiePo() {
            string[] text = new string[]{
                @"离我们家至若远点,你这不详之子", 
            };
            var npcDialog = WindowMng.windowMng.PushView("UI/NpcDialog", false);
            var dia = npcDialog.GetComponent<NpcDialog>();
            dia.ShowText(text[0]); 
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

        void TalkToZhiRuo() {
            if(!GameInterface_Player.GetGameState(GameBool.zhiruo1)) {
                StartCoroutine(ZhiRuo1());
            }else {
                ZhiRuo2();
            }
        }

        IEnumerator  DongHu1() {
            string[] text = new string[]{
                @"{0}，叔叔婶婶都是极厉害的人，他们一定是去干惊天大事情去了，真想有一天像他们一样呀。", 
                @"东湖，你想和我一起出去闯荡么？",
                @"听说外面世界很大，可是我还是喜欢呆在村子里，每天看见至若妹妹，我就很开心了。",
                @"至若妹妹说她也想出去呢",
                @"是吗，如果你们都走了我也要一起",
                @"好的一言为定，到时候我们一起闯荡天下。"
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
            
            GameInterface_Player.SetGameState(GameBool.donghu1, true);
            WindowMng.windowMng.PopView(); 
        }

        void DongHu2() {
            string[] text = new string[]{
                @"{0}，刚才村长那里发出一声巨响，似乎发生了什么事情，咱们快过去看看吧。", 
            };
            var npcDialog = WindowMng.windowMng.PushView("UI/NpcDialog", false);
            var dia = npcDialog.GetComponent<NpcDialog>();
            dia.ShowText(string.Format(text[0], ObjectManager.objectManager.GetMyName()));  
        }

        void TalkToDongHu() {
            if(!GameInterface_Player.GetGameState(GameBool.donghu1)) {
                StartCoroutine(DongHu1());
            }else {
                DongHu2();
            }
        }

        IEnumerator  QinQing1() {
            string[] text = new string[]{
                @"秦情姐姐，你好美呀", 
                @"臭小孩一边去，又想从我这里偸药么？听说你想去外面闯荡。",
                @"恩，我想去找我父母。",
                @"外面世界很凶险，我送你几瓶药吧。",
                @"谢谢姐姐。",
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
            
            GameInterface_Player.SetGameState(GameBool.qinqing1, true);
            WindowMng.windowMng.PopView();
        }

        void QinQing2() {
            string[] text = new string[]{
                @"{0}，外面很危险，你一定要多加小心。", 
            };
            var npcDialog = WindowMng.windowMng.PushView("UI/NpcDialog", false);
            var dia = npcDialog.GetComponent<NpcDialog>();
            dia.ShowText(string.Format(text[0], ObjectManager.objectManager.GetMyName())); 
        }

        void TalkToQinQing() {
            if(!GameInterface_Player.GetGameState(GameBool.qinqing1)) {
                StartCoroutine(QinQing1());
            }else {
                QinQing2();
            }
        }

        IEnumerator WanShan1() {
            string[] text = new string[]{
                @"{0}，我在村外找到的好玩的玩意，只要十个金币，你有钱了就可以到我这里耍耍。",
                @"万山叔，等我有钱了，再来找你。",
                @"等等，你能帮我带件礼物给秦情么，但别说是我送的，如果你答应的话，我可以送你件礼物。",
                @"恩，好的。",
                @"......,嘿嘿。",
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
            
            GameInterface_Player.SetGameState(GameBool.wanshan1, true);
            WindowMng.windowMng.PopView(); 
        }

        void WanShan2() {
            string[] text = new string[]{
                @"{0},要不要赌上一把。", 
            };
            var npcDialog = WindowMng.windowMng.PushView("UI/NpcDialog", false);
            var dia = npcDialog.GetComponent<NpcDialog>();
            dia.ShowText(string.Format(text[0], ObjectManager.objectManager.GetMyName())); 
        }

        void TalkToWanShan() {
            if(!GameInterface_Player.GetGameState(GameBool.wanshan1)) {
                StartCoroutine(WanShan1());
            }else {
                WanShan2();
            }
        }

        void TalkToANiu() {
            string[] text = new string[]{
                @"我叫阿牛，我爸爸离开村子了，好久没回来了。", 
            };
            var npcDialog = WindowMng.windowMng.PushView("UI/NpcDialog", false);
            var dia = npcDialog.GetComponent<NpcDialog>();
            dia.ShowText(string.Format(text[0], ObjectManager.objectManager.GetMyName())); 
        }
    }

}