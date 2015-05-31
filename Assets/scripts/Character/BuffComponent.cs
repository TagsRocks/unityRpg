using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ChuMeng {
	public class BuffComponent : MonoBehaviour {
		List<IEffect> effectList = new List<IEffect>();
		
		void Update () {
			foreach(IEffect ef in effectList) {
				ef.OnUpdate();
			}
			for (int i = 0; i < effectList.Count; ) {
				var ef = effectList[i];
				if(ef.IsDie) {
					ef.OnDie();
					effectList.RemoveAt(i);
				}else {
					i++;
				}
			}
		}

		public void AddBuff (Affix affix, GameObject attacker = null)
		{
			if (affix != null) {
				Log.Sys ("AddBuff is "+gameObject.name+" " + affix.effectType);

				var eft = BuffManager.buffManager.GetBuffInstance (affix.effectType);
				var buff = (IEffect)Activator.CreateInstance (eft);
				buff.Init (affix, gameObject);
				buff.attacker = attacker;

				effectList.Add (buff);
				buff.OnActive ();
			}
		}

		public int GetArmor ()
		{
			int addArmor = 0;
			foreach (IEffect ef in effectList) {
				addArmor += ef.GetArmor();
			}
			return addArmor;
		}

		void OnDisable() {
			foreach(IEffect ef in effectList) {
				ef.OnDie();
			}
		}
	}

}