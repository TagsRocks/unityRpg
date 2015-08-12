using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public class NotifyData
    {
        public string text;
        public float time;
        public System.Action<GameObject> cb;
    }

    public class NotifyUIManager : MonoBehaviour
    {
        List<NotifyData> nd = new List<NotifyData>();
        public static NotifyUIManager Instance;

        void Awake()
        {
            Instance = this;
        }

        public void AddNotify(string text, float time, System.Action<GameObject> cb)
        {
            nd.Add(new NotifyData(){text=text, time=time, cb = cb});
            if(nd.Count >= 3) {
                nd.RemoveAt(0);
            }
        }
        // Use this for initialization
        void Start()
        {
            StartCoroutine(CheckNotify());
        }

        IEnumerator CheckNotify()
        {
            while (true)
            {
                if (nd.Count > 0 && WindowMng.windowMng.GetUIRoot() != null)
                {
                    if (NotifyUI.Instance == null || NotifyUI.Instance.activeSelf == false)
                    {
                        yield return new WaitForSeconds(0.1f);
                        var not = nd [0];
                        nd.RemoveAt(0);
                        var g = WindowMng.windowMng.PushTopNotify("UI/NotifyLog");
                        if (g != null)
                        {
                            g.GetComponent<NotifyUI>().SetText(not.text);
                            g.GetComponent<NotifyUI>().SetDurationTime(not.time);
                        }
                        if (not.cb != null)
                        {
                            not.cb(g);
                        }
                    }else {
                        NotifyUI.Instance.GetComponent<NotifyUI>().ShortTime();
                    }
                }
                yield return new WaitForSeconds(1);
            }
        }

    
    }

}