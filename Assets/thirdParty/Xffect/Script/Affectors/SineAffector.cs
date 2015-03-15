//----------------------------------------------
//            Xffect Editor
// Copyright © 2012- Shallway Studio
// http://shallway.net
//----------------------------------------------

using UnityEngine;
using System.Collections;
using Xft;

namespace Xft
{
    ///沿着本地的forward 方向 sine震动 时间频率和空间频率  振幅
	/// 震动是变化 X 坐标
    public class SineAffector : Affector
    {
        protected float Magnitude;
        protected float SineFreqTime = 1;
        protected float SineFreqDist = 1;

        public SineAffector(float mag, float sineFreqTime, float sineFreqDist, EffectNode node)
            : base(node, AFFECTORTYPE.SineAffector)
        {
            Magnitude = mag;
            SineFreqTime = sineFreqTime;
            SineFreqDist = sineFreqDist;
        }

        public override void Update(float deltaTime)
        {
			Node.IsSine = true;
			Node.SineMagnitude = Magnitude;
			Node.SineFreqTime = SineFreqTime;
			Node.SineFreqDist = SineFreqDist;

            /*
            float strength = 0f;

            if(IsSine) {
                strength = Magnitude;
                float t = Node.GetElapsedTime();
				float st = Mathf.Sin(2*Mathf.PI*SineFreq*t);
				strength = st*strength;

            }else if (MType == MAGTYPE.Fixed)
                strength = Magnitude;
            else
                strength = MagCurve.Evaluate(Node.GetElapsedTime());

   
            if (GType == GAFTTYPE.Planar)
            {
                Vector3 syncDir = Node.Owner.ClientTransform.rotation * Dir;
                if (IsAccelerate)
                    Node.Velocity += syncDir * strength * deltaTime;
                else
                    Node.Position += syncDir * strength * deltaTime;
            }
            else if (GType == GAFTTYPE.Spherical)
            {
                Vector3 dir;
                dir = GravityObj.position - Node.GetOriginalPos();
                if (IsAccelerate)
                    Node.Velocity += dir * strength * deltaTime;
                else
                {
                    Node.Position += dir.normalized * strength * deltaTime;
                }  
            }
            */
        }
    }
}
