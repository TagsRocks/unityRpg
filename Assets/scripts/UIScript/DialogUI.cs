using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class DialogUI : IUserInterface
    {
        void Awake()
        {
            SetCallback("Ok", delegate(GameObject g){
                act(true);
                Hide(null);
           });
            SetCallback("Cancel", delegate (GameObject g){
                act(false);
                Hide(null);
            });
        }

        BoolDelegate act;

        public void SetCb(BoolDelegate action)
        {
            act = action;
        }

    }

}