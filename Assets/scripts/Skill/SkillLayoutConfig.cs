using UnityEngine;
using System.Collections;
namespace ChuMeng {
	public class SkillLayoutConfig : MonoBehaviour {
		public Vector3 Position;
		public GameObject particle;
        public float delayTime;
		//TODO:暂时不支持直接绑定到骨骼上面
		public string boneName="";

	}
}
