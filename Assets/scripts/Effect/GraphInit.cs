using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	public class GraphInit : MonoBehaviour
	{

		// Use this for initialization
		void Start ()
		{
			Shader.SetGlobalColor ("_OverlayColor", new Color(68/255.0f, 227/255.0f, 237/255.0f, 0.5f));
			Shader.SetGlobalColor ("_ShadowColor", new Color (28/255.0f, 25/255.0f, 25/255.0f, 1));
			Shader.SetGlobalVector ("_LightDir", new Vector3 (-1, -1, -1));
			Shader.SetGlobalVector ("_HighLightDir", new Vector3 (-1, -1, -1));
			Shader.SetGlobalColor ("_LightDiffuseColor", new Color (223/255.0f, 248/255.0f, 255/255.0f, 1));

			var res = Screen.currentResolution;
			Log.GUI ("Screen Attribute resolution "+res.width + " "+res.height+" "+res.refreshRate);
			Log.GUI ("Screen Attribute dpi "+Screen.dpi);
			Log.GUI ("Screen Attribute height "+Screen.height);
			Log.GUI ("Screen Attribute width "+Screen.width);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	}
}
