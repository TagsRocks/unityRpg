using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng {
	public class IEffect {
		protected GameObject obj;
		protected Affix affix;
		protected Affix.EffectType type;
		public bool IsDie = false;
		public GameObject attacker;
		protected float passTime = 0;
		GameObject unitTheme;

        /// <summary>
        /// 初始化Buff
        /// </summary>
        /// <param name="af">Af.</param>
        /// <param name="o">O.</param>
		public virtual void Init(Affix af, GameObject o) {
			affix = af;
			obj = o;
			if (affix.UnitTheme != null) {
				var par = GameObject.Instantiate(affix.UnitTheme) as GameObject;
				par.transform.parent = obj.transform;
				par.transform.localPosition = Vector3.zero;
				par.transform.localRotation = Quaternion.identity;
				par.transform.localScale = Vector3.one;

				unitTheme = par;
			}
		}
        /// <summary>
        /// 激活Buff
        /// </summary>
		public virtual void OnActive() {
		}
        /// <summary>
        /// Buff状态更新 
        /// </summary>
		public virtual void OnUpdate() {
			passTime += Time.deltaTime;
			if (passTime >= affix.Duration) {
				IsDie = true;
			}
		}
		public virtual void OnDie(){
			if (unitTheme != null) {
				GameObject.Destroy(unitTheme);
			}
		}

		public virtual int GetArmor ()
		{
			return 0;
		}
		protected BuffComponent GetBuffCom() {
			return obj.GetComponent<BuffComponent> ();
		}
	}
}
