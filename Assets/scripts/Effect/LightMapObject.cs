using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class LightMapObject : MonoBehaviour
    {
        void Awake() {
        }
        void Update() {
            this.renderer.material.SetVector("_LightMapScaleAndOffset", this.renderer.lightmapTilingOffset);
        }
    }
}