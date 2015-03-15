
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
using System.Collections.Generic;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(ShadowComponent))]

//[RequireComponent(typeof())]
public class CreepAI : MonoBehaviour {
	public enum CreepState {
		Idle,
		Running,
	}

	public float RunSpeed = 1;
	public float directionChangeInterval = 1;

	CharacterController controller;
	CreepState creepState;
	float heading;
	Vector3 targetRotation;

	Vector3 targetPos;
	Vector3 initPos;
	float radius;

	Seeker seeker;
	bool findPath = false;
	Pathfinding.Path path;

	void Awake() {
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
		creepState = CreepState.Idle;
		heading = Random.Range(0, 360);
		radius = 5;
		//controller.detectCollisions = false;
		animation.Play("idle");
		//GetComponent<ShadowComponent>().CreateShadowPlane();
	}

	void OnEnable() {
		seeker.pathCallback += OnPathComplete;
	}

	void OnPathComplete(Pathfinding.Path _p) {
		Debug.Log("path is "+_p);
		path = _p;
		findPath = true;

	}
	// Use this for initialization
	void Start () {
		initPos = transform.position;
		StartCoroutine(MoveAround());
	}
	IEnumerator WaitForFindPath() {
		while(!findPath)
			yield return null;
	}

	IEnumerator MoveAround() {
		while(true) {
			if(creepState == CreepState.Idle) {
				Vector2 xz = Random.insideUnitCircle;
				float r = Random.Range(0.0f, radius);
				Vector3 tarPos = initPos+new Vector3(xz.x*r, 0, xz.y*r);
				
				Debug.Log("target Pos "+tarPos);

				seeker.StartPath(transform.position, tarPos);

				yield return StartCoroutine(WaitForFindPath());

				findPath = false;
				List<Vector3> vPath = path.vectorPath;
				if (vPath.Count == 1) {
					vPath.Insert (0, transform.position);
				}
				animation.CrossFade("run");
				animation["run"].wrapMode = WrapMode.Loop;
				animation["run"].speed = 2;
				for(int i =1; i < vPath.Count; i++) {

					while(true) {
						Vector3 dir = vPath[i]-transform.position;
						dir.y = 0;
						var rotation = Quaternion.LookRotation(dir);
						transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime*10);
						controller.SimpleMove(transform.forward*RunSpeed);
						if(dir.magnitude < 0.1f) {
							break;
						}
						yield return null;
					}

					yield return null;
				}

				animation.CrossFade("fidget");
				animation["fidget"].wrapMode = WrapMode.Loop;
				yield return new WaitForSeconds(Random.Range(1, 2));
				animation.CrossFade("idle");
				animation["idle"].wrapMode = WrapMode.Loop;
				yield return new WaitForSeconds(Random.Range(1, 2));
			}
			yield return null;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
