using UnityEngine;
using System.Collections;

namespace MyLib
{
    public class Map7 : CScene
    {
        public override bool IsNet
        {
            get
            {
                return true;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(InitUI());
        }

        IEnumerator InitUI()
        {
            while (WorldManager.worldManager.station != WorldManager.WorldStation.Enter)
            {
                yield return null;
            }
            var uiRoot = WindowMng.windowMng.GetMainUI();
            //Resources.Load<GameObject>("UI/CardSelect")
            //var lt = NGUITools.AddChild(uiRoot, );
            WindowMng.windowMng.AddChild(uiRoot, Resources.Load<GameObject>("UI/CardSelect"));
            //VirtualJoystickRegion.VJR.gameObject.SetActive(false);
            VirtualJoystickRegion.VJR.Hide();

            var lr = Util.FindChildRecursive(uiRoot.transform, "LowRight_Panel");
            lr.gameObject.SetActive(false);
        }


    }
}
