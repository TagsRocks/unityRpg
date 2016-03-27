using UnityEngine;
using System.Collections;

namespace MyLib
{
    public class LightSourceCamera : MonoBehaviour
    {
        void Awake() {
            this.camera.depthTextureMode = DepthTextureMode.Depth;
        }
       
    }
}
