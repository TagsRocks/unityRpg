using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ChuMeng;


public static class NpcDialogInterface  {
    static IEnumerator ShowList(NpcDialog dia, IList<string> text, System.Action cb) {
        bool next = false;
        dia.ShowNext = delegate() {
            next = true;
        };

        foreach(var t in text) {
            //dia.ShowText(string.Format(t, ObjectManager.objectManager.GetMyName())); 
            dia.ShowText(t);
            while(!next) {
                yield return new WaitForSeconds(0.1f);
            }
            next = false;
        }
        WindowMng.windowMng.PopView();
        if(cb != null) {
            cb();
        }
    }

    /// <summary>
    /// Npc Dialog List Text 
    /// </summary>
    /// <param name="text">Text.</param>
    /// <param name="cb">Cb.</param>
    public static void ShowTextList(IList<string> text, System.Action cb) {
        var npcDialog = WindowMng.windowMng.PushView("UI/NpcDialog", false);
        var dia = npcDialog.GetComponent<NpcDialog>();
        dia.StartCoroutine(ShowList(dia, text, cb));
    }

}
