
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

namespace ChuMeng
{
	public class DungeonData : MonoBehaviour
	{
		public string Name = "Main";
		public string ParentDungeon;
		public int MonsterLvlMult = 1;
		public int Floors = 1;
		public SpawnClasses MonsterSpawnClass;
		public float MonsterPerMeterMin = 0.0175f;
		public float MonsterPerMeterMax = 0.0175f;
		public SpawnClasses ChampionSpawnClass;
		public float ChampionsMin = 2f;
		public float ChampionsMax = 2f;
		public SpawnClasses PropsSpawnClass;
		public float PropsPerMeterMin = 0.003f;
		public float PropsPerMeterMax = 0.003f;
		public SpawnClasses NpcSpawnClass;

		//http://docs.runicgames.com/wiki/Dungeons
		//Environment rat snake frogs 
		public SpawnClasses CreepSpawnClass;
		public float CreepsPerMeterMin = 0.003f;
		public float CreepsPerMeterMax = 0.003f;
		public int MonsterLvlMin = 1;
		public int MonsterLvlMax = 1;

		//Kill X monster task random quest
		public SpawnClasses QuestMonsterSpawnClass;
		//Collect X item random quest
		public SpawnClasses QuestItemSpawnClass;
		//Kill X Champion
		public SpawnClasses QuestChampionClass;
		public RuleSet RuleSet;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
		/*
		 * Init Rat Or Snake 
		 */
		public void InitCreep (AstarPath PathInfo)
		{
			if (PathInfo == null) {
				return ;
			}

			if (PathInfo.astarData.gridGraph != null) {
				int width = PathInfo.astarData.gridGraph.width;
				int height = PathInfo.astarData.gridGraph.depth;

				Pathfinding.GridGraph gridGraph = PathInfo.astarData.gridGraph;
				float rate = CreepsPerMeterMin;
				for (int i = 0; i < width; i++) {
					for (int j = 0; j < height; j++) {
						var w = gridGraph.nodes [j * width + i].Walkable;
						var node = gridGraph.nodes [j * width + i];

						if (w) {
							if (Random.Range (0.0f, 1.0f) < rate) {
								var creep = CreepSpawnClass.GetRandomCreep ();
								var mg = Instantiate (creep.gameObject) as GameObject;
								mg.transform.position = (Vector3)node.position + new Vector3 (0, 0.1f, 0);
							}
						}
					}
				}
			}
		}

		/*
		 * Initial Detructive  box
		 */ 
		public void InitProps (AstarPath PathInfo)
		{
			if (PathInfo == null) {
				return;
			}
			if (PathInfo.astarData.gridGraph != null) {
				int width = PathInfo.astarData.gridGraph.width;
				int height = PathInfo.astarData.gridGraph.depth;
		
				Pathfinding.GridGraph gridGraph = PathInfo.astarData.gridGraph;
				float rate = PropsPerMeterMin;
				for (int i = 0; i < width; i++) {
					for (int j = 0; j < height; j++) {
						var w = gridGraph.nodes [j * width + i].Walkable;
						var node = gridGraph.nodes [j * width + i];
						if (w) {
							if (Random.Range (0.0f, 1.0f) < rate) {
								var bar = PropsSpawnClass.GetRandomProps ();
								var mg = Instantiate (Resources.Load<GameObject> (bar.ModelName)) as GameObject;
								mg.transform.position = (Vector3)node.position + new Vector3 (0, 0.1f, 0);
								mg.GetComponent<ItemDataRef> ().ItemData = bar;
								mg.tag = "Barrel";
							}
						}
					}		
				}
			}
		}
	}

}