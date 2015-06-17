using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public class CopyList : IUserInterface
    {
        List<GameObject> levels;
        GameObject Cell;
        int curChapter = -1;
        List<LevelInfo> allLevels;

        void Awake() {
            regEvt = new System.Collections.Generic.List<MyEvent.EventType> () {
                MyEvent.EventType.OpenCopyUI,
                MyEvent.EventType.UpdateCopy,
            };
            RegEvent();

            SetCallback ("closeButton", Hide);
            levels = new List<GameObject>();
            Cell = GetName("Cell");
            Cell.SetActive(false);
        }

        protected override void OnEvent (MyEvent evt)
        {
            Log.GUI ("Update CopyController Update Gui ");
            UpdateFrame ();
        }
        void OnLevel(int levId){
            Log.GUI("OnLevelId "+levId);
            CopyController.copyController.SelectLevel (curChapter, allLevels[levId]);

            WorldManager.worldManager.WorldChangeScene(CopyController.copyController.SelectLevelInfo.levelLocal.id, false);
            Log.GUI("OnCopyLevel "+levId);
        }
        void UpdateFrame() {
            if (curChapter == -1) {
                curChapter = CopyController.copyController.GetCurrentChapter();
            }
            if (curChapter == -1) {
                return;
            }
            
            allLevels = CopyController.copyController.GetChapterLevel (curChapter);
            bool lastUnPass = true;
            Log.GUI ("Level Count "+levels.Count+ " "+allLevels.Count);
            for (int i = 0; i < allLevels.Count; i++) {
                //NewCell
                if(i >= levels.Count){
                    var nc = GameObject.Instantiate(Cell) as GameObject;
                    nc.transform.parent = Cell.transform.parent;
                    Util.InitGameObject(nc);
                    levels.Add(nc);
                }

                var lcell = levels[i];
                lcell.SetActive(true);
                if(allLevels[i].levelServer.IsPass) {
                    var cc = lcell.GetComponent<CopyCell>();
                    cc.SetTitle(allLevels[i].levelLocal.name);
                    int levelId = i;
                    cc.SetBtnCb(delegate(GameObject g){
                        OnLevel(levelId);
                    });
                }else {
                    if(lastUnPass){
                        lastUnPass = false;
                        var cc = lcell.GetComponent<CopyCell>();
                        cc.SetTitle(allLevels[i].levelLocal.name);
                        int levelId = i;
                        cc.SetBtnCb(delegate(GameObject g){
                            OnLevel(levelId);
                        });

                    }else {
                        lcell.SetActive(false);
                        break;
                    }
                }
            }
        }

        // Use this for initialization
        void Start()
        {
    
        }
    
        // Update is called once per frame
        void Update()
        {
    
        }
    }
}
