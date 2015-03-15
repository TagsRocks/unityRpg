using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace ChuMeng
{
	public class BuffManager : MonoBehaviour
	{
		public static BuffManager buffManager;
		void Awake() {
			DontDestroyOnLoad (gameObject);
			buffManager = this;
		}

		public Type GetBuffInstance(Affix.EffectType type) {
			return Type.GetType ("ChuMeng."+type.ToString());
		}

		public IEffect CreateBuffInstance(Affix affix) {
			var eft = GetBuffInstance (affix.effectType);
			var buff = (IEffect)Activator.CreateInstance (eft);
			return buff;
		}
	}

}