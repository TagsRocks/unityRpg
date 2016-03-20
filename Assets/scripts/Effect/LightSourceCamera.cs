using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class LightSourceCamera : MonoBehaviour
    {
        void Awake() {
            this.camera.depthTextureMode = DepthTextureMode.Depth;
        }
       
    }
}
