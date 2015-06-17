
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


namespace ChuMeng {
	public enum CharacterState
	{
		Idle,
		Running,
		Attacking,
		Around,
		Stunned,
		Dead,
		Birth,
		CastSkill,
		Story,
		Patrol,

		Flee,
	};

	/// <summary>
	/// 其它组件访问对象的数据 都通过 NpcAttribute 进行
	/// </summary>
	public class NpcAttribute : MonoBehaviour {
		public CharacterState _characterState = CharacterState.Idle;
		public int OwnerId = -1;

		public void SetOwnerId(int ownerId) {
			OwnerId = ownerId;
		}
		public GameObject GetOwner() {
			return ObjectManager.objectManager.GetLocalPlayer (OwnerId);
		}

		public float FastRotateSpeed {
			get {
				return 10;
			}
		}
		public float WalkSpeed{
			get {
				return ObjUnitData.MoveSpeed;
			}
		}

		//[NpcAttributeAtt()]
		public float ApproachDistance {
			get {
				if(_ObjUnitData != null) {
					return _ObjUnitData.ApproachDistance;
				}
				Debug.LogError("not init ObjData "+gameObject);
				return 0;
			}
		}
		public int HP {
			get {
				return GetComponent<CharacterInfo>().GetProp(CharAttribute.CharAttributeEnum.HP);
			}
			set {
				GetComponent<CharacterInfo>().SetProp(CharAttribute.CharAttributeEnum.HP, value);
			}
		}

		public int HP_Max {
			get {
				return GetComponent<CharacterInfo>().GetProp(CharAttribute.CharAttributeEnum.HP_MAX);
			}
			set {
				GetComponent<CharacterInfo>().SetProp(CharAttribute.CharAttributeEnum.HP_MAX, value);
			}
		}

		public int MP {
			get {
				return GetComponent<CharacterInfo>().GetProp(CharAttribute.CharAttributeEnum.MP);
			}
			set {
				GetComponent<CharacterInfo>().SetProp(CharAttribute.CharAttributeEnum.MP, value);
			}
		}

		public int MP_Max {
			get {
				return GetComponent<CharacterInfo>().GetProp(CharAttribute.CharAttributeEnum.MP_MAX);
			}
			set {
				GetComponent<CharacterInfo>().SetProp(CharAttribute.CharAttributeEnum.MP_MAX, value);
			}
		}


		//TODO::调整人物属性采用当前游戏的数据设定
	

		public int Exp {
			get {
				return GetComponent<CharacterInfo>().GetProp(CharAttribute.CharAttributeEnum.EXP);
			}
		
			private set {
				GetComponent<CharacterInfo>().SetProp(CharAttribute.CharAttributeEnum.EXP, value);
			}

		}

		//TODO: 技能点应该属于Skill系统
		//public int AttributePoint = 0;


		bool _isDead = false;

		public delegate void SetDead(GameObject g);
		public SetDead SetDeadDelegate;

		//玩家升级后设置等级
		//int _Level = 1;
		public int Level {
			get {
				return GetComponent<CharacterInfo>().GetProp(CharAttribute.CharAttributeEnum.LEVEL);
			}
			set {
				GetComponent<CharacterInfo>().SetProp(CharAttribute.CharAttributeEnum.LEVEL, value);
				SetLevel();
			}
		}

		[HideInInspector]
		public bool IsDead {
			get{
				return _isDead;
			}
			set {
				_isDead = true;
				if(SetDeadDelegate != null) {
					SetDeadDelegate(gameObject);
				}

				if(ObjectManager.objectManager != null) {
					if(ObjectManager.objectManager.killEvent != null) {
						ObjectManager.objectManager.killEvent(gameObject);
					}
				}

				//DropTreasure();
			}
		}

		int _Damage {
			get {
				return _ObjUnitData.Damage;
			}
		}

		public int Damage {
			get {
				return GetAllDamage();
			}
		}

		//int _PoisonDefense = 0;
		public int PoisonDefense {
			get {
				return GetWaterDefense();
			}
		}


		int _Armor{
			get {
				return _ObjUnitData.Armor;
			}
		}
		public int Armor {
			get {
				return GetAllArmor();
			}
		}

		UnitData _ObjUnitData;
		public UnitData ObjUnitData {
			set {
				_ObjUnitData = value;
				if(_ObjUnitData.TextureReplace.Length > 0) {
					SetTexture(_ObjUnitData.TextureReplace);
				}
			}
			get  {
				return _ObjUnitData;
			}
		}

		NpcEquipment npcEquipment;

		//TODO:攻击距离由当前激活的技能决定 而不是 由 角色属性决定
		//TODO:简化 人物的攻击距离 分成远程和进展两种攻击距离即可，进展不能变成远程，远程也不能变成近战, 攻击距离主要对近战有效
		//TODO: 火炬之光里面 攻击距离是由武器决定的
		public float AttackRange {
			get {
				if(_ObjUnitData != null) {
					return _ObjUnitData.AttackRange;
				}
				Debug.LogError("not init ObjData "+gameObject);
				return 0;
			}
		}
		public float ReachRange {
			get {
				return 2;
			}
		}
		public float PatrolRange {
			get {
				return 5;
			}
		}



		//根据配置文件初始化属性
		//TODO: 初始化其它玩家的属性 PlayerOther  PlayerSelf Monster Boss
		void InitData() {
			Log.Important ("Initial Object HP "+gameObject.name);
			var characterInfo = GetComponent<CharacterInfo>();
			if(ObjUnitData != null && characterInfo != null) {
				var view = GetComponent<KBEngine.KBNetworkView>(); 


				Log.Important("Player View State "+gameObject.name+" "+view.IsPlayer+" "+view.IsMine);
				HP_Max = _ObjUnitData.HP;
				HP = HP_Max;
				MP_Max = _ObjUnitData.MP;
				MP = MP_Max;
				Log.Important("Init Obj Data  "+gameObject.name+" "+HP+" "+_ObjUnitData.HP);
				ChangeHP(0);
				ChangeMP(0);
				/*
				//自己玩家的属性 通过CharacterInfo 来初始化
				if(view.IsPlayer && view.IsMine) {
				
					//TODO:其它玩家属性  从网络同步数据
				}else if(view.IsPlayer && !view.IsMine) {
					HP_Max = _ObjUnitData.HP;
					HP = HP_Max;
					MP_Max = _ObjUnitData.MP;
					MP = MP_Max;
					Debug.Log("NpcAttribute::InitData Use Animation In ");
					gameObject.tag = "Player";

				}else {
					Log.Important("Init Local Monster Data "+gameObject.name);
					//普通单人副本怪物 从本地初始化属性
					HP_Max = _ObjUnitData.HP;
					HP = HP_Max;

					ChangeHP(0);
					Log.Important("Init MonsterData "+HP);
				}
				*/

				//CharacterController ch = GetComponent<CharacterController>();
			}
		}

		//玩家升级后设置等级 调整对应UnitData
		//TODO: 单人副本中调整属性  多人副本中网络同步属性 城市中网络同步属性  属性调整都是通过 CharacterInfo 来做的
		void SetLevel() {
			_ObjUnitData =  Util.GetUnitData (_ObjUnitData.GetIsPlayer(), _ObjUnitData.ID, Level);
		}

		void Awake() {
			npcEquipment = GetComponent<NpcEquipment>();
		}

	

		public void SetObjUnitData(UnitData ud) {
			ObjUnitData = ud;
			InitData ();
		}

		/*
		 * Player Equipment PoisonDefense
		 * Monster Define in UnitData
		 */ 
		int GetWaterDefense() {
			int d = 0;
			if (npcEquipment != null) {
				d += npcEquipment.GetPoisonDefense();
			}
			return d;
		}

		/*
		 * BaseWeapon Damage
		 * Fire Element Damage  Ice Element Electric
		 */ 
		int GetAllDamage() {
			int d = _Damage;
			if(npcEquipment != null) {
				d += npcEquipment.GetDamage();
			}
			Debug.Log("Damage is what  "+d);
			return d;
		}



		int GetAllArmor() {
			int a = _Armor;
			if (npcEquipment != null) {
				a += npcEquipment.GetArmor ();
			}
			a += GetComponent<BuffComponent> ().GetArmor();
			return a;
		}

		void Start () {
		}
		
		// Update is called once per frame
		//TODO: 更新技能状态
		void Update () {
			/*
			foreach (SkillFullInfo sk in skills) {
				sk.Update();
			}
			*/
		}
		public int GetLocalId() {
			return GetComponent<KBEngine.KBNetworkView> ().GetLocalId ();
		}
		public void ChangeHP(int c) {
			HP += c;
			HP = Mathf.Min(Mathf.Max(0, HP), HP_Max);
			var rate = HP * 1.0f / HP_Max * 1.0f;


			var evt = new MyEvent (MyEvent.EventType.UnitHPPercent);
			evt.localID = GetLocalId ();

			evt.floatArg = rate;
			MyEventSystem.myEventSystem.PushEvent (evt);

			Log.Important ("Init GameObject HP "+gameObject.name);

			var evt1 = new MyEvent (MyEvent.EventType.UnitHP);
			evt1.localID = GetLocalId ();
			evt1.intArg = HP;
			evt1.intArg1 = HP_Max;
			MyEventSystem.myEventSystem.PushLocalEvent(evt1.localID, evt1);

            if(GetLocalId() == ObjectManager.objectManager.GetMyLocalId())
            {
                MyEventSystem.myEventSystem.PushEvent(MyEvent.EventType.UpdateMainUI);
            }
		}

		public void ChangeMP(int c) {
			MP += c;
			MP = Mathf.Min (Mathf.Max(0, MP), MP_Max);
			var rate = MP * 1.0f / MP_Max * 1.0f;

			var evt = new MyEvent (MyEvent.EventType.UnitMPPercent);
			evt.localID = GetLocalId();

			evt.floatArg = rate;
			MyEventSystem.myEventSystem.PushEvent (evt);

			var evt1 = new MyEvent (MyEvent.EventType.UnitMP);
			evt1.localID = GetLocalId();
			evt1.intArg = MP;
			evt1.intArg1 = MP_Max;
			MyEventSystem.myEventSystem.PushEvent (evt1);

            if(GetLocalId() == ObjectManager.objectManager.GetMyLocalId())
            {
                MyEventSystem.myEventSystem.PushEvent(MyEvent.EventType.UpdateMainUI);
            }
		}

		/*
		 * Damage Type 
		 */ 
		public void DoHurt(int v, bool isCritical, SkillData.DamageType dt = SkillData.DamageType.Physic) {
			Debug.Log ("NpcAttribute::DoHurt Name:"+gameObject.name+" hurtValue:"+v+" Armor:"+Armor+" DamageType "+dt);
			if (dt == SkillData.DamageType.Physic) {
				int hurt = v - Armor;
				Log.Important("Get Hurt is "+hurt);
				if (hurt > 0) {
					if(!isCritical) {
						PopupTextManager.popTextManager.ShowRedText("-"+hurt.ToString(), transform);
					}else {
						PopupTextManager.popTextManager.ShowPurpleText("-"+hurt.ToString(), transform);
					}
					ChangeHP (-hurt);
				}else {
					Log.Important("Armor too big for player "+Armor);
				}
			} else if(dt == SkillData.DamageType.Water){
				var d = GetWaterDefense();
				int hurt = (int)(v*(1- d/100.0f));
				if(hurt > 0) {
					ChangeHP(-hurt);
				}
			}
		}

		//calculate Hurt event in stunned
		public bool CheckDead() {
			return (HP <= 0);
		}

		//精英怪或者怪物变种 需要替换纹理
		void SetTexture(string tex) {
			/*
			Transform obj = transform.Find("obj");
			if (obj == null) {
				obj = transform.Find("obj0");
			}
			*/
			var skins = gameObject.GetComponentInChildren<SkinnedMeshRenderer> ();
			skins.renderer.material.mainTexture = Resources.Load<Texture>(tex);

		}


		//TODO: 单人副本中需要判断是否升级以及升级相关处理
		public void ChangeExp(int e) {
			Exp += e;
			var maxExp = _ObjUnitData.MaxExp;

			if (Exp >= maxExp) {
				LevelUp ();
			}

			var evt = new MyEvent (MyEvent.EventType.UpdatePlayerData);
			evt.localID = GetLocalId ();
			MyEventSystem.myEventSystem.PushEvent (evt);
		}


		//TODO:玩家升级的逻辑处理  技能点
		void LevelUp() {
			//Modify Hp Mp
			Level += 1;
			Exp = 0;
			//ChangeExp (0);

			Util.ShowLevelUp (Level);
			var par = Instantiate(Resources.Load<GameObject>("particles/events/levelUp")) as GameObject;
			NGUITools.AddMissingComponent<RemoveSelf> (par);
			par.transform.parent = ObjectManager.objectManager.transform;
			par.transform.position = transform.position;
		}



		//TODO: 掉落物品机制重新设计 掉落物品和掉落黄金 
		public List<float> GetDropTreasure() {
            return _ObjUnitData.GetRandomDrop();

			/*
			if (_ObjUnitData != null) {
				UnitData.Treasure treasure = _ObjUnitData.GetRandomDrop();
				if(treasure != null) {
					if(treasure.TreasureType == UnitData.TreasureType.Armor) {
						Debug.Log("Drop Treasure is what "+treasure.itemData.DropMesh);
						var g = Instantiate(Resources.Load<GameObject>(treasure.itemData.DropMesh)) as GameObject;
						var com = NGUITools.AddMissingComponent<ItemDataRef>(g);
						com.ItemData = treasure.itemData;
						g.transform.position = transform.position;

						//g.transform.localScale = new Vector3(2, 2, 2);

						var par = Instantiate(Resources.Load<GameObject>("particles/drops/generic_item")) as GameObject;
						//par.transform.position = transform.position;
						par.transform.parent = g.transform;
						par.transform.localPosition = Vector3.zero;
						com.Particle = par;
						com.IsOnGround = true;
					}
					//if(treasure.TreasureType == UnitData.TreasureType.Potion) {
					else {
						var g = Instantiate(Resources.Load<GameObject>(treasure.itemData.ModelName)) as GameObject;
						var com = NGUITools.AddMissingComponent<ItemDataRef>(g);
						com.ItemData = treasure.itemData;
						g.transform.position = transform.position;

						//g.transform.localScale = new Vector3(2, 2, 2);

						var par = Instantiate(Resources.Load<GameObject>("particles/drops/generic_item")) as GameObject;
						//par.transform.position = transform.position;
						par.transform.parent = g.transform;
						par.transform.localPosition = Vector3.zero;
						com.Particle = par;
						com.IsOnGround = true;
					}
				}

			}

			//Drop Gold 
			var rd = Random.Range (0, 100);
			if (rd < 25) {

			}
			*/
		}


		//TODO:单人副本中获得一个怪物的随机技能
		public SkillData GetRandomSkill() {
			/*
			if (skills.Count == 0) {
				return null;
			}

			int sk = Random.Range(0, skills.Count);
			SkillFullInfo si = skills[sk];
			int ch = Random.Range(0, 100);
			Debug.Log ("random skill is "+ch+" "+si.skillData.Chance);
			if(ch < si.skillData.Chance && si.CoolDownTime == 0 && si.skillData.castType == SkillData.CastType.Always) {
				si.CoolDownTime = si.skillData.CooldownMS/1000.0f;
				return si.skillData;
			}
			*/

			return null;
		}

		//获得终极技能
		//TODO: 死亡时释放的技能
		public SkillData GetDeadSkill() {
			/*
			foreach (SkillFullInfo sin in skills) {
				if(sin.skillData.castType == SkillData.CastType.Struck ) {
					int ch = Random.Range(0, 100);
					if(ch < sin.skillData.Chance) {
						return sin.skillData;
					}
				}
			}
			*/
			return null;
		}

		IEnumerator AddHpProgress(float duration, float totalAdd) {
			float addRate = totalAdd / duration;
			float goneTime = 0;
			int count = 0;
			int tc = Mathf.RoundToInt(duration / 0.1f);
			while (count < tc) {
				if(goneTime > 0.1f) {
					HP += Mathf.RoundToInt(addRate*0.1f);
					HP = Mathf.Min(HP_Max, HP);
					ChangeHP(0);
					goneTime -= 0.1f;
				}
				goneTime += Time.deltaTime;
				count++;
				yield return null;
			}
		}

		//TODO: 吃个药瓶
		public void AddHp(float duration, float totalAdd) {
			StartCoroutine (AddHpProgress (duration, totalAdd));
		}

		IEnumerator AddMpProgress(float duration, float totalAdd) {
			float addRate = totalAdd / duration;
			float goneTime = 0;
			int count = 0;
			int tc = Mathf.RoundToInt (duration/0.1f);
			while(count < tc) {
				if(goneTime > 0.1f) {
					MP += Mathf.RoundToInt(addRate*0.1f);
					MP = Mathf.Min(MP_Max, MP);
					ChangeMP(0);
					goneTime -= 0.1f;
				}
				goneTime += Time.deltaTime;
				count++;
				yield return null;
			}
		}

		public void AddMp(float duration, float totalAdd) {
			StartCoroutine (AddMpProgress (duration, totalAdd));
		}



		public void ShowDead() {
			IsDead = true;
			_characterState = CharacterState.Dead;
			var sdata = GetDeadSkill ();
			if (sdata != null) {
				StartCoroutine (GetComponent<CommonAI>().ShowDeadSkill (sdata));
			}
			
			if (ObjectManager.objectManager != null && ObjectManager.objectManager.myPlayer != null) {
				Physics.IgnoreCollision (GetComponent<CharacterController>(), ObjectManager.objectManager.GetMyPlayer ().GetComponent<CharacterController> ());
			}
		}

		public bool CheckAni(string name) {
			return animation.GetClip (name) != null; 
		}
	}

}