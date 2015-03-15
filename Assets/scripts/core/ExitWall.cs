
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
	public class ExitWall : MonoBehaviour
	{
		Transform doorOpenBeam;
		Transform collider;
		List<Transform> beams;
		// Use this for initialization
		void Start ()
		{
			doorOpenBeam = transform.FindChild("doorOpenBeam");
			doorOpenBeam.gameObject.SetActive (false);
			collider = transform.FindChild("collider");
			beams = Util.FindAllChild (transform, "enterBeam_entrance");
		}
		public void ZoneClear() {
			collider.gameObject.SetActive (false);
			StartCoroutine (WaitShowDoorOpenEffect());
		}

		IEnumerator WaitShowDoorOpenEffect() {
			while (true) {
				var players = Physics.OverlapSphere(transform.position, 4, 1 << (int)GameLayer.Npc);
				foreach(Collider p in players) {
					if(p.tag == GameTag.Player) {
						StartCoroutine(ShowOpenEffect());
						yield break;
					}
				}
				yield return null;
			}

		}

		IEnumerator ShowOpenEffect() {
			doorOpenBeam.gameObject.SetActive(true);
			yield return new WaitForSeconds (0.4f);
			foreach (Transform t in beams) {
				t.gameObject.SetActive(false);
			}
		}

		public void CloseDoor() {
			collider.gameObject.SetActive (true);
			foreach (Transform t in beams) {
				t.gameObject.SetActive(true);
			}

		}

	}


}