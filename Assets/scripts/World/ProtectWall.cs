using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    /// <summary>
    /// 进入一个Room之后开启保护墙，防止玩家倒退回上一个Room
    /// </summary>
    public class ProtectWall : MonoBehaviour
    {
        Transform doorOpenBeam;
        Transform collider;
        List<Transform> beams;
        void Awake(){
            doorOpenBeam = transform.FindChild("doorOpenBeam");
            doorOpenBeam.gameObject.SetActive (false);
            collider = transform.FindChild("collider");
            beams = Util.FindAllChild (transform, "enterBeam_entrance");
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 玩家进入新的区域显示区域保护门
        /// </summary>
        public void ShowWall(){
            gameObject.SetActive(true);
        }

    }
}
