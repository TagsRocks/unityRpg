﻿using UnityEngine;
using System.Collections;

namespace ChuMeng
{
    public class Bomb : MonoBehaviour
    {
        public Vector3 OffsetPos;
        public SkillData skillData;
        public SkillLayoutRunner runner;
        public BombData bombData;
        public GameObject attacker;
        const float BombTime = 4;

        float velocity;
        Vector3 initPos;
        GameObject activeParticle;
        private bool isDie = false;

        //抛物线子弹需要添加一个小的CharacterController碰撞器 用于运动
        void Awake()
        {
            var c = gameObject.AddComponent<CharacterController>();
            c.center = new Vector3(0, 0.2f, 0);
            c.radius = 0.1f;
            c.height = 0.5f;
        }

        void Start()
        {
            velocity = bombData.Velocity;
            initPos = transform.position;

            var playerForward = Quaternion.Euler(new Vector3(0, 0 + attacker.transform.rotation.eulerAngles.y, 0));
            if (bombData.ReleaseParticle != null)
            {
                GameObject par = Instantiate(bombData.ReleaseParticle) as GameObject;
                NGUITools.AddMissingComponent<RemoveSelf>(par);

                par.transform.parent = ObjectManager.objectManager.transform;
                par.transform.localPosition = attacker.transform.localPosition + playerForward * OffsetPos;
                par.transform.localRotation = playerForward;
            }

            if (bombData.ActiveParticle != null)
            {
                GameObject par = Instantiate(bombData.ActiveParticle) as GameObject;
                activeParticle = par;
                par.transform.parent = transform;
                par.transform.localPosition = Vector3.zero;
                par.transform.localRotation = Quaternion.identity;
            }
            StartCoroutine(ThrowPhysic());
        }

        IEnumerator ThrowPhysic()
        {
            var upSpeed = bombData.UpSpeed;
            var moveDirection = transform.forward;
            var forwardSpeed = velocity;
            var controller = gameObject.GetComponent<CharacterController>();
            var gravity = bombData.Gravity;
            var passTime = 0.0f;
            while (passTime < BombTime)
            {
                var movement = moveDirection * forwardSpeed + upSpeed * Vector3.up;
                movement *= Time.deltaTime;
                controller.Move(movement);

                upSpeed = upSpeed - gravity * Time.deltaTime;
                passTime += Time.deltaTime;

                if ((controller.collisionFlags & CollisionFlags.Below) != 0)
                {
                    forwardSpeed -= bombData.Friction * Time.deltaTime;
                    forwardSpeed = Mathf.Max(0, forwardSpeed);
                }
                yield return null;
            }
            CreateHitParticle();
            GameObject.Destroy(gameObject);
            AOEDamage();
        }

        void CreateHitParticle()
        {
            if (bombData.HitParticle != null)
            {
                GameObject g = Instantiate(bombData.HitParticle) as GameObject;
                NGUITools.AddMissingComponent<RemoveSelf>(g);
                g.transform.position = transform.position;
                g.transform.parent = ObjectManager.objectManager.transform;

            }
        }

        void AOEDamage()
        {
            Collider[] col = Physics.OverlapSphere(transform.position, bombData.AOERadius, SkillDamageCaculate.GetDamageLayer());
            foreach (Collider c in col)
            {
                DoDamage(c);
            }
        }
        //炸弹所有人都攻击
        void DoDamage(Collider other)
        {
            var tarPos = other.transform.position+new Vector3(0, 1, 0);
            var mePos = transform.position+new Vector3(0, 0.1f, 0);
            var dir = tarPos - mePos;
            dir.Normalize();
            RaycastHit hitInfo;
            var hit  = Physics.Raycast(mePos, dir, out hitInfo, bombData.AOERadius, SkillDamageCaculate.GetBlockerLayer());
            Log.Sys("TestBlock: "+mePos+" dir "+dir+" tarpos "+tarPos+" radius "+bombData.AOERadius+" hit "+hit+" hitInfo "+hitInfo);

            //障碍物阻挡无法攻击目标
            if(hit) {
                var hitObj = hitInfo.collider.gameObject;
                Log.Sys("Block HitObjIs: "+hitObj+" other "+other.gameObject);
                if(hitObj != other.gameObject) {
                    Log.Sys("Bomb Block Hit: "+hitObj+" other "+other.gameObject);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(skillData.HitSound))
            {
                BackgroundSound.Instance.PlayEffect(skillData.HitSound);
            }


            var attr = other.GetComponent<NpcAttribute>();
            if(attr == null) {
                attr = other.GetComponentInParent<NpcAttribute>();
            }

            if (runner != null && attr != null)
            {
                runner.DoDamage(attr.gameObject);
            }
        }

    }
}
