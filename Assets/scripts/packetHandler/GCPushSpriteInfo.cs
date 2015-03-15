
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

namespace PacketHandler {
	public class GCPushSpireInfo : IPacketHandler {
		public override void HandlePacket(KBEngine.Packet packet) {
			if (packet.responseFlag == 0) {
				var data = packet.protoBody as ChuMeng.GCPushSpireInfo;
				foreach(ChuMeng.ViewPlayer vp in data.ViewPlayersList) {
					CreatePlayer(vp);
				}
				foreach(ChuMeng.MotionSprite ms in data.MotionsList) {
					MovePlayer(ms);
				}

				foreach(ChuMeng.HideSprite hp in data.HidesList) {
					HidePlayer(hp);
				}
			} else {
			}
		}

		/*
		 * Other Player Model
		 * JobModel
		 * EquipmentModel
		 * MyUnitID is what?
		 * 
		 * Where To Put These Message Handler Global Function Or In Global Object
		 * 
		 * Lua State Contain A System Function And Data
		 */

		void CreatePlayer(ChuMeng.ViewPlayer vp) {
			ChuMeng.ObjectManager.objectManager.CreatePlayer (vp);


		}

		//HeartBeat Move
		void MovePlayer(ChuMeng.MotionSprite ms) {
			var view = ChuMeng.ObjectManager.objectManager.GetPhotonView (ms.UnitId.Id);
			if(view != null) {
				ChuMeng.ObjectManager.objectManager.RunViewUpdate (view, ms);
			}
		}

		/*
		 * MyPlayer And MyPhotonView  owner == -1   internalID = 1 2 3
		 * 
		 * Clear Player And ClearPhotonView
		 */ 
		void HidePlayer(ChuMeng.HideSprite hp) {
			ChuMeng.ObjectManager.objectManager.ClearPlayer (hp);
		}
	}

}
