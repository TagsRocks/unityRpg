using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class StoryProgress : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            WindowMng.windowMng.PushView("UI/StoryDialog");
        }
    
        // Update is called once per frame
        void Update()
        {
    
        }
    }

}