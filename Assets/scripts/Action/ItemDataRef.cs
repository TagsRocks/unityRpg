
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

namespace ChuMeng
{
	public class ItemDataRef : MonoBehaviour
	{
		public ItemData itemData;
		public GameObject Particle;
		public bool IsOnGround = false;
		//ShadowComponent shadow;
		GameObject player;
		
        //BackPack backpack;
		//Gold Number
		private int num;

		void Awake ()
		{
			//player = GameObject.FindGameObjectWithTag ("Player");
            player = ObjectManager.objectManager.GetMyPlayer();
			//shadow = GetComponent<ShadowComponent> ();
			/*
			if (GetComponent<ShadowComponent> ()) {
				GetComponent<ShadowComponent> ().CreateShadowPlane ();
			}
			*/
			//backpack = GameObject.Find ("backpackController");

			//backpack = GameObject.FindObjectOfType<BackPack> ();

		}
		// Use this for initialization
		void Start ()
		{
			if (itemData != null && itemData.UnitType == ItemData.UnitTypeEnum.QUESTITEM) {
				var c = NGUITools.AddMissingComponent<SphereCollider>(gameObject);
				c.radius = 2;
				c.isTrigger = true;
			}
			else if (itemData != null && IsOnGround) {
				var c = gameObject.AddComponent<CharacterController>();
				c.center = new Vector3(0, 0.2f, 0);
				c.radius = 0.1f;
				c.height = 0.5f;
				Physics.IgnoreCollision(c, player.GetComponent<CharacterController>());
				StartCoroutine (PickTreasure ());

			}
		}

		/*
		 * TODO:拾取任务物品
		 */
		void OnTriggerEnter(Collider col) {

			/*
			 * if (col.tag == "Player") {
				if(ItemData != null && ItemData.UnitType == ItemData.UnitTypeEnum.QUESTITEM) {
					GameObject.Destroy (gameObject);
					if (backpack != null) {
						if(ItemData.UnitType == ItemData.UnitTypeEnum.GOLD) {
							backpack.PutGold(Value);
						}else {
							backpack.PutItemInBackpack(ItemData);
						}
						if(Particle != null) {
							GameObject.Destroy(Particle);
						}
						IsOnGround = false;
					}
				}
			}
			*/
		}

		//TODO:拾取掉落物品
		IEnumerator PickTreasure ()
		{
            Log.Sys("PickThisTreasure "+itemData.ItemName);
			Vector2 dir = Random.insideUnitCircle;
			float curFlySpeed = 0;
			Vector3 upSpeed = new Vector3(0, 5, 0);
			Vector3 moveDirection = new Vector3 (dir.x, 0, dir.y);
			var controller = GetComponent<CharacterController> ();
			float Gravity = 5.0f;

			float passTime = 0;
			float rotSpeed = 360;
			while (passTime < 2f) {
				curFlySpeed = Mathf.Lerp(curFlySpeed, 2, 5*Time.deltaTime);
				var movement = moveDirection * curFlySpeed + upSpeed;
				movement *= Time.deltaTime;
				controller.Move(movement);
				upSpeed.y -= Gravity*Time.deltaTime;
				
				passTime += Time.deltaTime;
				transform.localRotation = Quaternion.Euler(rotSpeed*Time.deltaTime, 0, 0)*transform.localRotation;
				yield return null;
			}

			yield return new WaitForSeconds (1);

			float flySpeed = 20;
			float UpSpeed = 10;
			upSpeed = new Vector3 (0, UpSpeed, 0);
			while (true) {
				moveDirection = player.transform.position - transform.position;
				if(moveDirection.magnitude < 0.3f) {
					break;
				}

				var nor = moveDirection.normalized;


				var movement = nor*flySpeed+upSpeed;
				movement *= Time.deltaTime;
				controller.Move(movement);
				upSpeed.y -= Gravity*Time.deltaTime;

				passTime += Time.deltaTime;
				yield return null;
			}
            BackgroundSound.Instance.PlayEffect("pickup");
			GameObject.Destroy (gameObject);
            GameInterface_Backpack.PickItem(itemData, num);
            IsOnGround = false;

            /*
			if (backpack != null) {
				if(ItemData.UnitType == ItemData.UnitTypeEnum.GOLD) {
					backpack.GetComponent<BackPack>().PutGold(Value);
				}else {
					backpack.GetComponent<BackPack> ().PutItemInBackpack(ItemData);
				}
				if(Particle != null) {
					GameObject.Destroy(Particle);
				}
				IsOnGround = false;
			}
			*/
		}

		// Update is called once per frame
		void Update ()
		{
            /*
			if (player != null && shadow != null) {
				var dis = (player.transform.position - transform.position).magnitude;
				if (dis < 4) {
					shadow.AdjustLightPos (player.transform.position + new Vector3 (0, 3, 0));
				}
			}
            */
		}

		IEnumerator WaitRemove ()
		{
			yield return new WaitForSeconds (0.3f);
			GameObject.Destroy (gameObject);
		}

		public void Break ()
		{
			var breakable = Resources.Load<GameObject> ("particles/barrelbreak");
			breakable.transform.position = transform.position + new Vector3 (0, 0.1f, 0);
			Instantiate (breakable);
			StartCoroutine (WaitRemove ());
		}

		public static GameObject MakeDropItem(ItemData itemData, Vector3 pos, int num) {
			var g = Instantiate(Resources.Load<GameObject>(itemData.DropMesh)) as GameObject;
			var com = NGUITools.AddMissingComponent<ItemDataRef>(g);
			com.itemData = itemData;
			g.transform.position = pos;
			
			var par = Instantiate(Resources.Load<GameObject>("particles/drops/generic_item")) as GameObject;
			par.transform.parent = g.transform;
			par.transform.localPosition = Vector3.zero;
			com.Particle = par;
			com.IsOnGround = true;
            com.num = num;
            BackgroundSound.Instance.PlayEffect("dropgold");
			return g;
		}
	}

}
