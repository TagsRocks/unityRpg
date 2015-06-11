using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class CopyCell : IUserInterface
    {
        public void SetTitle(string tit){
            GetLabel("Name").text = tit;
        }
        public void SetBtnCb(UIEventListener.VoidDelegate cb){
            SetCallback("button", cb);
        }
    }

}