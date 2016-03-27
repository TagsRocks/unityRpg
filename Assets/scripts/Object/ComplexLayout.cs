using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyLib
{
    [System.Serializable]
    public class LayoutConfig {
        public GameObject g;
        public Vector3 pos;
    }
    /// <summary>
    /// 参考SpawnTrigger 运行时加载相关组件资源而不是将组件资源嵌入到GameObject中 
    /// 
    /// 使用方法：
    ///   1：配置工程中的prefab 到parts中
    ///   2：点击UpdateModel
    ///   3：调整model位置
    ///   4：点击CollectPos
    ///   5：点击RemoveModel
    ///   6：将其保存为一个Prefab到工程中
    /// </summary>
    public class ComplexLayout : MonoBehaviour
    {
        public List<LayoutConfig> parts = new List<LayoutConfig>();

        [ButtonCallFunc()]
        public bool UpdateModel;
        public void UpdateModelMethod() {
            ClearChildren();
            AddChildren();
        }

        [ButtonCallFunc()]
        public bool CollectPos;
        public void CollectPosMethod() {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                parts[i].pos = child.localPosition;
            } 
        }

        [ButtonCallFunc()]
        public bool Remove;
        public void RemoveMethod() {
            ClearChildren();
        }

        void ClearChildren() {
            for (int i = 0; i < transform.childCount;)
            {
                GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        void AddChildren() {
            foreach(var g in parts) {
                var nb = GameObject.Instantiate(g.g) as GameObject;
                var oldRot = nb.transform.localRotation;
                nb.transform.parent = transform;
                Util.InitGameObject(nb);
                nb.transform.localRotation = oldRot;
                nb.transform.localPosition = g.pos;
            }
        }

        /// <summary>
        /// 游戏中调用 
        /// </summary>
        void Awake() {
            UpdateModelMethod();
        }
    }

}