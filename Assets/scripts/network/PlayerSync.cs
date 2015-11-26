
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

using System;
using KBEngine;

namespace ChuMeng
{
	public class PlayerSync : KBEngine.MonoBehaviour
	{
		/*
		 * Write Message Send To Server
		 * PlayerManagerment  PhotonView Manager 
		 */ 
		int lastGridX = -1;
		int lastGridZ = -1;
		public void OnPhotonSerializeView (Packet packet)
		{
            return;
			if (photonView.IsMine) {
				if(AstarPath.active != null && AstarPath.active.graphs.Length > 0) {
					CGPlayerMove.Builder mv = CGPlayerMove.CreateBuilder ();
					var vxz = Util.CoordToGrid(transform.position.x, transform.position.z);
					//MapY Offset Height
					mv.X = Convert.ToInt32( vxz.x);
					mv.Y = Util.IntYOffset(transform.position.y);
					mv.Z = Convert.ToInt32(vxz.y);
					if(mv.X == lastGridX && mv.Z == lastGridZ) {
						return;
					}
					if(Util.CheckMovable((int)mv.X, (int)mv.Z)) {
						lastGridX = mv.X;
						lastGridZ = mv.Z;
						packet.protoBody = mv.BuildPartial ();
						return;
					}else {
						Log.Sys("Can't Move Grid "+mv.X+" "+mv.Z+" "+gameObject.name);
					}
				}

				packet.protoBody = null;

			} else {
				//Debug.Log("Not Mine Push Move Command");
                Log.AI("PushMoveCommand");
				var ms = packet.protoBody as MotionSprite;
				var vxz = Util.GridToCoord(ms.X, ms.Z);

				var mvTarget = new Vector3(vxz.x, 0, vxz.y);

				var cmd = new ObjectCommand();
				cmd.targetPos = mvTarget;
				cmd.commandID = ObjectCommand.ENUM_OBJECT_COMMAND.OC_MOVE;
				GetComponent<LogicCommand>().PushCommand(cmd);

			}

		}

		public void SetPosition(ViewPlayer ms) {
            return;
            Log.Sys("PlayerSync::SetPosition init other player "+ms);
			Vector2 vxz = Util.GridToCoord (ms.X, ms.Z);
			float y = ObjectManager.objectManager.GetSceneHeight (ms.X, ms.Z);
			transform.position = new Vector3(vxz.x, y, vxz.y);
			transform.rotation = Quaternion.Euler (new Vector3(0, ms.Dir, 0));
		}

        public void NetworkMove(AvatarInfo info) {
            var mvTarget = new Vector3(info.X/100.0f, info.Y/100.0f+0.2f, info.Z/100.0f);
            var cmd = new ObjectCommand();
            cmd.targetPos = mvTarget;
            cmd.commandID = ObjectCommand.ENUM_OBJECT_COMMAND.OC_MOVE;
            GetComponent<LogicCommand>().PushCommand(cmd);
        }

        public void SetPosition(AvatarInfo info) {
            Vector3 vxz = new Vector3(info.X/100.0f, info.Y/100.0f+0.2f, info.Z/100.0f);
            Log.Sys("SetPosition: "+info+" vxz "+vxz+" n "+gameObject.name);
            transform.position = new Vector3(vxz.x, vxz.y, vxz.y);
            transform.rotation = Quaternion.Euler (new Vector3(0, 0, 0));
            StartCoroutine(SetPos(vxz));
        }
        /// <summary>
        /// 稳定一下初始化位置 
        /// </summary>
        /// <returns>The position.</returns>
        /// <param name="p">P.</param>
        IEnumerator SetPos(Vector3 p) {
            var c = 0;
            while(c <= 3) {
                transform.position = p;
                c++;
                yield return null;
            }
        }
	
	}

}