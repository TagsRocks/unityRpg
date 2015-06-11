using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//一个场景中的数据，包括LevelInit AStar 以及 BattleManager 以及Forever怪物出生点等信息
namespace ChuMeng
{
	public class CScene
	{
		public bool IsCity {
			get {
				return def.isCity;
			}
		}
		public DungeonConfigData def;
		public CScene(DungeonConfigData sceneDef) {
			def = sceneDef;
		}

		//地图大小
		public int GetSizeX() {
			return 0;
		}
		public int GetSizeZ() {
			return 0;
		}

		//初始化静态资源
		public void Init() {
			//创建网格Astar和LevelInit 等对象

			//加载环境音效


		}

		//
		public void EnterScene() {
			//预先加载Npc和怪物资源

			//播放背景音乐
		}

		//
		public void LeaveScene() {
			//销毁环境音效和背景音乐

			//销毁场景网络对象 其它玩家 服务器Npc
			var keys = ObjectManager.objectManager.Actors.ToArray ();

			foreach (var k in keys) {
				ObjectManager.objectManager.DestroyObject(k.Key);
			}

			//销毁本地怪物 本地玩家对象
			ObjectManager.objectManager.DestroyMySelf ();

		}

		public void Tick() {
		}
	}
}
