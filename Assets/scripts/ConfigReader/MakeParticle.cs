using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;

#if UNITY_EDITOR
using UnityEditor;
using Xft;
#endif

public class MakeParticle : MonoBehaviour
{
    public string configFile;
    [ButtonCallFunc()]
    public bool
        Make;

    public void MakeMethod()
    {
        var md = Resources.LoadAssetAtPath("Assets/Config/" + configFile + ".json", typeof(TextAsset)) as TextAsset;
        var jarr = SimpleJSON.JSON.Parse(md.text).AsArray;
        var xffectObj = DoCreateXffectObject();
        xffectObj.name = configFile.Split(char.Parse(".")) [0];

        foreach (SimpleJSON.JSONNode n in jarr)
        {
            var layer = DoCreateLayer(xffectObj);

            var jobj = GetProp(n);
            var name = jobj ["NAME"].Value;
            layer.name = name;
            var effectLayer = layer.GetComponent<EffectLayer>();
            var tex = jobj ["TEXTURE"].Value;
            var renderStyle = jobj ["RENDER STYLE"].Value;
            Shader useShader;
            if (renderStyle == "Additive")
            {
                useShader = Shader.Find("Mobile/Particles/Additive");
            } else
            {
                useShader = Shader.Find("Mobile/Particles/Alpha Blended");
            }

            var texPath = tex.Replace("media", "Assets").Replace(".dds", ".mat");
            var texName = texPath.Replace(".mat", ".png");
            Debug.Log("load Material " + texPath + "  " + texName);

            var mat = Resources.LoadAssetAtPath(texPath, typeof(Material)) as Material;
            Debug.Log("load Material " + mat);

            if (mat == null)
            {
                mat = new Material(useShader);
                mat.SetTexture("_MainTex", Resources.LoadAssetAtPath(texName, typeof(Texture)) as Texture);
                AssetDatabase.CreateAsset(mat, texPath);
                AssetDatabase.ImportAsset(texPath);
            }

            effectLayer.Material = mat;

            SetProp(effectLayer, jobj);
            //Modifier
            var child = GetChildren(n);
            var childArr = child ["children"].AsArray;
            foreach (JSONNode c in childArr)
            {
                var modData = c ["children"] [0].AsObject;
                var modType = modData ["DESCRIPTOR"].Value;
                if (modType == "Color")
                {
                    var overTime = modData ["COLOR OVER TIME"].Value;
                    var param = ConvertToFloat(overTime.Split(','));
                    SetColor(effectLayer, param);
                } else if (modType == "Emitter")
                {
                    var forever = modData ["NO EXPIRATION"].AsBool;
                    var rate = modData ["EMIT RATE"].Value;
                    var rateArr = ConvertToFloat(rate.Split(','));
                    
                    Debug.Log("Emit Config " + modData.ToString());
                    SetEmit(effectLayer, forever, rateArr, modData);

                } else if (modType == "Texture Rotate")
                {

                    Debug.Log("Rotate Config " + modData.ToString());
                    SetRotate(effectLayer, modData);
                } else if (modType == "Scale")
                {
                    SetScale(effectLayer, modData);
                } else if (modType == "Texture Animation")
                {
                    SetAni(effectLayer, modData);
                }
            }

        }
    }

    void SetProp(EffectLayer effectLayer, JSONClass modData)
    {
        var fw = modData ["FRAMES WIDE"].AsInt;
        var fh = modData ["FRAMES HIGH"].AsInt;
        if (fw > 0)
        {
            Debug.Log("Set UV");
            effectLayer.UVType = 1;
            effectLayer.Cols = fw;
            effectLayer.Rows = fh;
            
        }

        var renderType = modData ["RENDER TYPE"].Value;
        if (renderType == "Billboard Up" || renderType == "Billboard Up Camera" || renderType == "Billboard Forward")
        {
            effectLayer.RenderType = 0;
            effectLayer.SpriteType = (int)Xft.STYPE.BILLBOARD_UP;
        } else
        {
        }

        var isLight = modData ["IS LIGHT"].AsBool;
        if (isLight)
        {
            effectLayer.gameObject.layer = (int)ChuMeng.GameLayer.Light;
        }
    }

    void SetRandomScaleCurve(EffectLayer effectLayer, float[] scale)
    {
        int count = (scale.Length - 1) / 3;
        float x = 0;
        float value = 0;
        float max = 1;

        for (int i = 1; i < scale.Length; i++)
        {
            var mod = (i-1)%3;
            if (mod == 0)
            {
                x = scale [i];
            } else if(mod == 1)
            {
                value = scale [i];
                if (Mathf.Abs(value) > max)
                {
                    max = Mathf.Abs(value);
                }
            }
        }

        effectLayer.MaxScaleCalue = max;
        var ks = new Keyframe[count];
        int c = 0;
        for (int i = 1; i < scale.Length; i++)
        {
            var mod = (i-1)%3;
            if (mod == 0)
            {
                x = scale [i];
            } else if(mod == 1)
            {
                value = scale [i];
                Debug.Log("Add Scale Node " + value / max);
                ks [c] = new Keyframe(x, value / max);
                c++;

            }
        }
        effectLayer.ScaleXCurveNew = new AnimationCurve(ks);
        effectLayer.ScaleWrapMode = WRAP_TYPE.LOOP;
    }

    void SetScale(EffectLayer effectLayer, JSONClass modData)
    {
        var fix = modData ["FIXED"].AsBool;
        if (fix)
        {
            effectLayer.ScaleType = RSTYPE.CURVE01;
            effectLayer.UseSameScaleCurve = true;

            var scale = ConvertToFloat(modData ["X"].Value);
            int scaType = (int)scale [0];
            if (scaType == 4)
            {
                SetRandomScaleCurve(effectLayer, scale);
            } else
            {
                //scale[0]  2  curve  3 straight
                int count = (scale.Length - 1) / 2;
                float x = 0;
                float value = 0;
                float max = 1;
                for (int i = 1; i < scale.Length; i++)
                {
                    if ((i - 1) % 2 == 0)
                    {
                        x = scale [i];
                    } else
                    {
                        value = scale [i];
                        if (Mathf.Abs(value) > max)
                        {
                            max = Mathf.Abs(value);
                        }
                    }
                }
                effectLayer.MaxScaleCalue = max;
                var ks = new Keyframe[count];
                int c = 0;
                for (int i = 1; i < scale.Length; i++)
                {
                    if ((i - 1) % 2 == 0)
                    {
                        x = scale [i];
                    } else
                    {
                        value = scale [i];
                        Debug.Log("Add Scale Node " + value / max);
                        ks [c] = new Keyframe(x, value / max);
                        c++;
                        /*
                    var ret = effectLayer.ScaleXCurveNew.AddKey(new Keyframe(x, value/max)); 
                    if(ret == -1) {
                        
                    }
                    if(i == 2) {

                        var key = effectLayer.ScaleXCurveNew[0];
                        key.time = x;
                        key.value = value/max;

                    }else if(i == scale.Length-1) {
                        var key = effectLayer.ScaleXCurveNew[effectLayer.ScaleXCurveNew.length-1];
                        key.time = x;
                        key.value = value/max;

                    }else {

                    }
                    */
                    }
                }
                effectLayer.ScaleXCurveNew = new AnimationCurve(ks);
                effectLayer.ScaleWrapMode = WRAP_TYPE.LOOP;
            }
        }
    }

    void SetRotate(EffectLayer effectLayer, JSONClass modData)
    {
        var rotValue = modData ["STARTING ROTATION"].Value;
        if (rotValue != "")
        {
            var rotate = ConvertToFloat(rotValue);

            if ((int)(rotate [0]) == 1)
            {
                Debug.Log("rate value " + rotate [0]);
                effectLayer.RandomOriRot = true;
                effectLayer.OriRotationMin = (int)rotate [1];
                effectLayer.OriRotationMax = (int)rotate [2];
            }
        }
        var speed = ConvertToFloat(modData ["ROTATION SPEED"].Value);
        if ((int)speed [0] == 1)
        {
            float min = speed [1];
            float max = speed [2];
            effectLayer.RotateType = RSTYPE.RANDOM;
            effectLayer.RotateSpeedMin = min;
            effectLayer.RotateSpeedMax = max;
        }
    }

    void SetAni(EffectLayer effectLayer, JSONClass modData)
    {
        var fnum = effectLayer.Cols * effectLayer.Rows;
        var speed = ConvertToFloat(modData ["ANIMATION SPEED"].Value);
        var uvTime = fnum * 1.0f / speed [1];
        effectLayer.UVTime = uvTime;

    }

    void SetEmit(EffectLayer effectLayer, bool forever, float[] rate, JSONClass modData)
    {
        int rateType = (int)rate [0];
        int rateMin = (int)rate [1];
        if (rateType == 0)
        {
            effectLayer.EmitRate = rateMin;
            effectLayer.MaxENodes = rateMin;
        } else
        {
            int rateMax = (int)rate [2];
            effectLayer.EmitRate = rateMax;
            effectLayer.MaxENodes = rateMax;
        }

        if (forever)
        {
            effectLayer.IsNodeLifeLoop = true;
            effectLayer.IsBurstEmit = true;
        } else
        {
            effectLayer.IsNodeLifeLoop = false;
            effectLayer.IsBurstEmit = false;
        }

        {
            var life = ConvertToFloat(modData ["PARTICLE LIFE"].Value);
            int lifeType = (int)life [0];
            if (lifeType == 1)
            {
                //effectLayer.IsNodeLifeLoop = false;
                //effectLayer.IsBurstEmit = false;
                effectLayer.NodeLifeMin = life [1];
                effectLayer.NodeLifeMax = life [2];
                effectLayer.EmitDuration = 1;
                effectLayer.EmitLoop = -1;
                effectLayer.EmitDelay = 0;

            } else if (lifeType == 0)
            {
                //effectLayer.IsNodeLifeLoop = false;
                //effectLayer.IsBurstEmit = false;
                effectLayer.NodeLifeMin = life [1];
                effectLayer.NodeLifeMax = life [1];
                effectLayer.EmitDuration = 1;
                effectLayer.EmitLoop = -1;
                effectLayer.EmitDelay = 0;

            }
        }



        var emitType = modData ["TYPE OF EMITTER"].Value;
        if (emitType == "Circle")
        {
            effectLayer.EmitType = 3;
            effectLayer.UseRandomCircle = true;
            effectLayer.CircleRadiusMin = ConvertToFloat(modData ["MIN RADIUS"].Value.Split(',')) [1];
            effectLayer.CircleRadiusMax = ConvertToFloat(modData ["MAX RADIUS"].Value.Split(',')) [1];
        } else if (emitType == "SphereSurface")
        {
            effectLayer.EmitType = 2;
            effectLayer.Radius = modData ["RADIUS"].AsFloat;
        }
            
        var scaleData = ConvertToFloat(modData ["SCALE ON LAUNCH"].Value.Split(','));
        var scaType = (int)scaleData [0];
        if (scaType == 1)
        {
            effectLayer.RandomOriScale = true;
            effectLayer.OriScaleXMin = scaleData [1];
            effectLayer.OriScaleXMax = scaleData [2];
            effectLayer.OriScaleYMin = scaleData [1];
            effectLayer.OriScaleYMax = scaleData [2];
        } else if (scaType == 0)
        {
            effectLayer.RandomOriScale = false;
            effectLayer.OriScaleXMin = effectLayer.OriScaleXMax = scaleData [1];
            effectLayer.OriScaleYMin = effectLayer.OriScaleYMax = scaleData [1];
        }

        var posx = modData ["POSITIONX"].AsFloat;
        var posy = modData ["POSITIONY"].AsFloat;
        var posz = modData ["POSITIONZ"].AsFloat;
        effectLayer.EmitPoint = new Vector3(posx, posy, posz);
        if (modData ["WIDTH"].Value != "")
        {
            var width = ConvertToFloat(modData ["WIDTH"].Value);
            if ((int)width [0] == 0)
            {
                effectLayer.SpriteWidth = width [1];
            }
        }

        var velocity = ConvertToFloat(modData ["VELOCITY"].Value);
        if ((int)velocity [0] == 0)
        {
            effectLayer.OriSpeed = velocity [1];
            effectLayer.OriVelocityAxis = Vector3.up;
        }
    }

    void SetColor(EffectLayer layer, float[] f)
    {
        layer.ColorChangeType = COLOR_CHANGE_TYPE.Gradient;
        layer.ColorParam = new ColorParameter();
        layer.ColorParam.Colors.Clear();
        layer.ColorAffectorEnable = true;
        layer.ColorGradualType = COLOR_GRADUAL_TYPE.LOOP;
        float time = 0;
        float r, g, b, a;
        r = g = b = a = 0;
        for (int i = 0; i < f.Length; i++)
        {
            switch (i % 5)
            {
                case 0:
                    time = f [i];
                    break;
                case 1:
                    r = f [i];
                    break;
                case 2:
                    g = f [i];
                    break;
                case 3:
                    b = f [i];
                    break;
                case 4:
                    a = f [i];
                    layer.ColorParam.AddColorKey(time, new Color(r, g, b, a));
                
                    break;

            }

        }
    }

    float[] ConvertToFloat(string s)
    {
        return ConvertToFloat(s.Split(','));
    }

    float[] ConvertToFloat(string[] s)
    {
        float[] f = new float[s.Length];
        int i = 0;
        foreach (string c in s)
        {
            f [i] = Convert.ToSingle(c);
            i++;
        }
        return f;
    }

    SimpleJSON.JSONClass GetProp(JSONNode obj)
    {
        return obj ["children"] [0].AsObject;
    }

    JSONClass GetChildren(JSONNode obj)
    {
        return obj ["children"] [1].AsObject;
    }

    static GameObject DoCreateXffectObject()
    {
        GameObject go = new GameObject("XffectObj");
        go.transform.localScale = Vector3.one;
        go.transform.rotation = Quaternion.identity;
        go.AddComponent<XffectComponent>();
        return go;
    }

    static GameObject DoCreateLayer(GameObject go)
    {
        GameObject layer = new GameObject("EffectLayer");
        EffectLayer efl = (EffectLayer)layer.AddComponent("EffectLayer");
        layer.transform.parent = go.transform;

        efl.transform.localPosition = Vector3.zero;
        //fixed 2012.6.25. default to effect layer object.
        efl.ClientTransform = efl.transform;
        efl.GravityObject = efl.transform;
        efl.BombObject = efl.transform;
        efl.TurbulenceObject = efl.transform;
        efl.AirObject = efl.transform;
        efl.VortexObj = efl.transform;
        efl.DirCenter = efl.transform;
        efl.DragObj = efl.transform;

        efl.Material = AssetDatabase.LoadAssetAtPath(GetXffectPath() + DefaultMatPath, typeof(Material)) as Material;
        return layer;
    }

    static public string GetXffectPath()
    {
        Shader temp = Shader.Find("Xffect/PP/radial_blur_mask");
        string assetPath = AssetDatabase.GetAssetPath(temp);
        int index = assetPath.LastIndexOf("Xffect");
        string basePath = assetPath.Substring(0, index + 7);
        
        return basePath;
    }

    // Use this for initialization
    void Start()
    {
    
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }

    public static string DefaultMatPath = "Examples/Materials/default.mat";
}
