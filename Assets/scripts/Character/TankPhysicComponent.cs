using UnityEngine;
using System.Collections;

namespace MyLib
{
    //移动和旋转接口
    public class TankPhysicComponent : MonoBehaviour
    {
        Rigidbody rigid;
        Vector3 moveValue = Vector3.zero;
        private Vector3 mvDir;
        private bool rot = false;

        NpcAttribute attribute;
        public float maxVelocityChange = 3.0f;
        public float maxRotateChange = 3.0f;
        private float gravity = 20;

        private GameObject tower;
        private bool grounded = false;

        void Start()
        {
            rigid = GetComponent<Rigidbody>();
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            rigid.useGravity = false;
            attribute = GetComponent<NpcAttribute>();
            tower = Util.FindChildRecursive(transform, "tower").gameObject;
        }

        public void MoveSpeed(Vector3 moveSpeed)
        {
            moveValue = moveSpeed * attribute.GetMoveSpeedCoff();
        }

        public void TurnTo(Vector3 moveDirection)
        {
            /*
            var y1 = transform.eulerAngles.y;
            var y2 = Quaternion.LookRotation (moveDirection).eulerAngles.y;
            var y3 = Mathf.LerpAngle(y1, y2, attribute.GetMoveSpeedCoff());
            transform.rotation = Quaternion.Euler(new Vector3(0, y3, 0));
            */
            mvDir = moveDirection;
            rot = true;
        }

        //旋转炮台 射击时候 或者安静的时候自动归位
        public void TurnTower(Vector3 moveDirection)
        {
            var y = Quaternion.LookRotation(moveDirection).eulerAngles.y;
            Log.Sys("TowerRotate: " + y);
            tower.transform.rotation = Quaternion.Euler(new Vector3(0, y, 0));
        }

        void OnCollisionStay()
        {
            grounded = true;
        }

        void FixedUpdate()
        {
            if (grounded)
            {
                var targetVelocity = moveValue;
                var oldVelocity = rigid.velocity;
                var velocityDiff = targetVelocity - oldVelocity;
                velocityDiff.y = 0;
                velocityDiff.x = Mathf.Clamp(velocityDiff.x, -maxVelocityChange, maxVelocityChange);
                velocityDiff.z = Mathf.Clamp(velocityDiff.z, -maxVelocityChange, maxVelocityChange);
                rigidbody.AddForce(velocityDiff, ForceMode.VelocityChange);
                moveValue = new Vector3(0, 0, 0);
            }

            rigidbody.AddForce(new Vector3(0, -gravity * rigidbody.mass, 0));
            grounded = false;


            if (rot)
            {
                var forwardDir = mvDir;
                var curDir = transform.forward;
                curDir.y = 0;
                forwardDir.y = 0;

                var diffDir = Quaternion.FromToRotation(curDir, forwardDir);
                var diffY = diffDir.eulerAngles.y;
                Log.Sys("diffYIs: " + diffY);
                if (diffY > 180)
                {
                    diffY = diffY - 360;
                }
                if (diffY < -180)
                {
                    diffY = 360 + diffY;
                }

                var rate = 1.0f;
                /*
                var abs = Mathf.Abs(diffY);
                if(abs < 20) {
                    rate = abs/20.0f;
                }
                */

                var dy = Mathf.Clamp(diffY, -maxRotateChange, maxRotateChange);
                var curSpeed = rigid.angularVelocity;
                var diffVelocity = dy-curSpeed.y;
                Log.Sys("DirY: " + dy + " diffY: " + diffY);

                rigid.AddTorque(Vector3.up * diffVelocity, ForceMode.VelocityChange);
                rot = false;
            }
        }
        #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (ClientApp.Instance.testAI)
            {
                Gizmos.color = Color.red;
                var st = transform.position + new Vector3(0, 2, 0);
                var ed = st + mvDir * 4;
                Gizmos.DrawLine(st, ed);
                //Gizmos.DrawSphere(st, 4);
            }
        }
        #endif
    }


}