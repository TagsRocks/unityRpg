using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

//一个场景中的数据，包括LevelInit AStar 以及 BattleManager 以及Forever怪物出生点等信息
namespace ChuMeng
{
	public class CScene : MonoBehaviour
	{
		public bool IsCity {
			get {
				return def.isCity;
			}
		}
		public DungeonConfigData def;
        void Awake() {
            GameObject.DontDestroyOnLoad(gameObject);
        }
        public static CScene CreateScene(DungeonConfigData sceneDef){
            var tp = Type.GetType ("ChuMeng.Map" +sceneDef.id);
            var g = new GameObject("CScene");

            var t = typeof(NGUITools);
            var m = t.GetMethod ("AddMissingComponent");
            var geMethod = m.MakeGenericMethod (tp);
            geMethod.Invoke (null, new object[]{g});// as AIBase;

            var sc = g.GetComponent<CScene>();
            sc.def = sceneDef;
            return sc;
        }

		//初始化静态资源
		public virtual void Init() {
			//创建网格Astar和LevelInit 等对象

			//加载环境音效
		}

		//
		public virtual  void EnterScene() {
			//预先加载Npc和怪物资源

			//播放背景音乐
		}

        /// <summary>
        /// 战斗管理器等初始化结束 
        /// </summary>
        public virtual void ManagerInitOver() {
        }

		/// <summary>
        /// 销毁场景元素
        /// </summary>
		public virtual  void LeaveScene() {
			//销毁环境音效和背景音乐

			//销毁场景网络对象 其它玩家 服务器Npc
			var keys = ObjectManager.objectManager.Actors.ToArray ();

			foreach (var k in keys) {
				ObjectManager.objectManager.DestroyObject(k.Key);
			}

			//销毁本地怪物 本地玩家对象
			ObjectManager.objectManager.DestroyMySelf ();

		}
	}
}
