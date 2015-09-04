using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	public class GraphInit : MonoBehaviour
	{
        public Texture lightMap;
        //public Vector3 camPos = Vector3.zero;

        public Vector3 ambient = Vector3.one;
        public Texture lightMask;
        public float lightCoff;
		
        void InitAll() {
            var lc = Resources.Load<GameObject>("LightCamera").camera;
            var lightCamera = lc.GetComponent<LightCamera>();
            //New Shader lightMapxxx need these Set
            //var lc = GameObject.FindGameObjectWithTag("LightCamera").camera;
            
            var camSize = lc.orthographicSize;
            Shader.SetGlobalTexture("_LightMap", lightMap);
            Shader.SetGlobalVector("_CamPos", lightCamera.CamPos);
            Shader.SetGlobalFloat("_CameraSize", camSize);
            
            Shader.SetGlobalVector("_AmbientCol", ambient);
            Shader.SetGlobalTexture("_LightMask", lightMask);
            Shader.SetGlobalFloat("_LightCoff", lightCoff);
            
            
            
            Shader.SetGlobalColor ("_OverlayColor", new Color(68/255.0f, 227/255.0f, 237/255.0f, 0.5f));
            Shader.SetGlobalColor ("_ShadowColor", new Color (28/255.0f, 25/255.0f, 25/255.0f, 1));
            Shader.SetGlobalVector ("_LightDir", new Vector3 (-1, -1, -1));
            Shader.SetGlobalVector ("_HighLightDir", new Vector3 (-1, -1, -1));
            Shader.SetGlobalColor ("_LightDiffuseColor", new Color (223/255.0f, 248/255.0f, 255/255.0f, 1));
            
            Shader.SetGlobalColor ("_GhostColor", new Color(68/255.0f, 227/255.0f, 68/255.0f, 0.5f));
            
            var res = Screen.currentResolution;
            Log.GUI ("Screen Attribute resolution "+res.width + " "+res.height+" "+res.refreshRate);
            Log.GUI ("Screen Attribute dpi "+Screen.dpi);
            Log.GUI ("Screen Attribute height "+Screen.height);
            Log.GUI ("Screen Attribute width "+Screen.width); 
        }
        // Use this for initialization
		void Start ()
		{
            InitAll();
		}
	
        public Color testAmbient;
        [ButtonCallFunc()]
        public bool InitAmbient;
        public void InitAmbientMethod() {
            Shader.SetGlobalVector("_AmbientCol", testAmbient);
            Shader.SetGlobalFloat("_LightCoff", lightCoff);
        }

        [ButtonCallFunc()]
        public bool InitNow;
        public void InitNowMethod() {
            InitAll();
        }

	}
}
