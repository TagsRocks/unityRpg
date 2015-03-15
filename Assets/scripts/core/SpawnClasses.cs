
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/

/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class SpawnClasses : MonoBehaviour
	{
		public enum SpawnType {
			None,
			Weapon,
			Potion,
		}

		[System.Serializable]
		public class SpawnData
		{
			public SpawnType spawnType;
			public ItemData Item;
			public UnitData Monster;
			public SpawnClasses MonsterClass;
			public CreepAI Creep;
			public int Weight = -1;
			public int MinCount = 1;
			public int MaxCount = 1;
		}
		public List<SpawnData> Resources;

		public CreepAI GetRandomCreep ()
		{
			return Resources [0].Creep;
		}

		public SpawnData GetRandomMonster ()
		{
			var totalWeight = 0;
			foreach (var m in Resources) {
				totalWeight += m.Weight;
			}
			var curWeight = 0;
			var rw = Random.Range (0, totalWeight);
			foreach (var m in Resources) {
				curWeight += m.Weight;
				if (rw < curWeight) {
					return m;
				}
			}
			return Resources [Resources.Count - 1];
		}

		public ItemData GetRandomProps ()
		{
			return Resources [0].Item;
		}
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	}
}
