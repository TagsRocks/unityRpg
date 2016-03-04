#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class TestShader : MonoBehaviour
    {
        [ButtonCallFunc()]public bool SetOther;

        public void SetOtherMethod()
        {
            var me = ObjectManager.objectManager.GetMyAttr();
            me.SetTeamHideShader();
        }

    }
}

#endif