using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class LightMapInit : MonoBehaviour
    {
        public Texture unityLightMap;
        public float lightMapScale;

        public static LightMapInit Instance;

        void Awake()
        {
            Instance = this;
        }
        void Start() {
            InitLightMapMethod();
        }

        [ButtonCallFunc()] public bool InitLightMap;

        public void InitLightMapMethod()
        {
            Shader.SetGlobalTexture("_UnityLightMap", unityLightMap);
           // Shader.SetGlobalFloat("_LightMapScale", lightMapScale);
        }
	
    }

}