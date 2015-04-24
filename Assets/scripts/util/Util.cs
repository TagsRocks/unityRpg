
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
using Google.ProtocolBuffers.Descriptors;
using Google.ProtocolBuffers;
using System;
using System.Collections.Generic;

namespace ChuMeng
{
	using pb = global::Google.ProtocolBuffers;
	public enum GameLayer
	{
		PlayerCamera = 12,
		UICamera = 5,
		Npc = 13,
        Light = 11,
	}

	public class GameTag
	{
		public const string Player = "Player";
		public const string Enemy = "Enemy";
	}

	public enum UIDepth
	{
		MainUI = 1,
		Window = 10,

	}

	public class Icon
	{
		public int iconId = -1;
		public string iconName = "";

		public Icon (int id, string name)
		{
			iconId = id;
			iconName = name;
		}

		public Icon ()
		{
		}
	}

	public delegate void VoidDelegate (GameObject g);

	public delegate void ItemDelegate (ItemData id);

	public delegate void IntDelegate (int num);

	public delegate void StringDelegate (string arg);

	public delegate void BoolDelegate (bool arg) ;

	public partial class Util
	{
		static AstarPath astarPath = null;

		public static IEnumerator WaitForAnimation (Animation a)
		{
			while (a.isPlaying) {
				yield return null;
			}
		}

		public static IEnumerator SetBurn (GameObject go)
		{
			Transform mesh = go.transform.Find ("obj");
			if (mesh == null) {
				foreach (Transform t in go.transform) {
					if (t.GetComponent<SkinnedMeshRenderer> () != null) {
						mesh = t;
						break;
					}
				}
				if (mesh == null) {
					Debug.Log ("Util::SetBurn Not Find Obj or Skinned Mesh");
				}
			}

			Material[] mat = mesh.renderer.materials;

			var shaderRes = Resources.Load<ShaderResource> ("levelPublic/ShaderResource");
			for (int i=0; i < mesh.renderer.materials.Length; i++) {
				mat [i].shader = Shader.Find ("Custom/newBurnShader");

				mat [i].SetTexture ("_cloud", shaderRes.cloudImg);
				mat [i].SetFloat ("_timeLerp", 0);
			}
			//Material mat = mesh.renderer.material;
			//mat.SetFloat("_timeLerp", 0);
			float passTime = 0;
			while (passTime < 1) {
				for (int i =0; i < mat.Length; i++) {
					mat [i].SetFloat ("_timeLerp", passTime);
					passTime += Time.deltaTime;
				}
				yield return null;
			}

		}
		/*
		 * If Monster Dead clear Monster Burn Material
		 */ 
		public static void ClearMaterial (GameObject go)
		{
			Transform mesh = go.transform.Find ("obj");
			if (mesh == null) {
				foreach (Transform t in go.transform) {
					if (t.GetComponent<SkinnedMeshRenderer> () != null) {
						mesh = t;
						break;
					}
				}
				if (mesh == null) {
					Debug.Log ("Util::ClearMaterial Not Find Obj or Skinned Mesh");
				}
			}

			Material[] mat = mesh.renderer.materials;
			for (int i=0; i < mat.Length; i++) {
				UnityEngine.Object.Destroy (mat [i]);
			}
		}
		public static List<Transform> FindAllChild(Transform t, string name) {
			List<Transform> list = new List<Transform> ();
			foreach (Transform c in t) {
				if(c.name == name) {
					list.Add(c);
				}
			}
			return list;
		}
		//not include root
		public static Transform FindChildRecursive (Transform t, string name)
		{
			if (t.name == name) {
				return t;
			}

			Transform r = t.Find (name);
			if (r != null)
				return r;
			foreach (Transform c in t) {
				r = FindChildRecursive (c, name);
				if (r != null)
					return r;
			}
			return null;
		}

		public static T FindChildrecursive<T> (Transform t) where T : MonoBehaviour
		{
			if (t.GetComponent<T> () != null) {
				return t.GetComponent<T> ();
			}
			T r = null;
			foreach (Transform c in t) {
				r = FindChildrecursive<T> (c);
				if (r != null) {
					return r;
				}
			}
			return null;
		}

		public static T FindType<T> (GameObject g) where T : Component
		{
			Transform tall = g.transform;
			foreach (Transform t in tall) {
				if (t.GetComponent<T> () != null) {
					return t.GetComponent<T> ();
				}
			}
			return null;
		}


		//从root中找到 和copyPart 同名的骨骼 组成一个骨骼组，赋值到新拷贝的部分的骨骼
		public static void SetBones (GameObject newPart, GameObject copyPart, GameObject root)
		{
			var render = newPart.GetComponent<SkinnedMeshRenderer> ();
			var copyRender = copyPart.GetComponent<SkinnedMeshRenderer> ();
			var myBones = new Transform[copyRender.bones.Length];
			for (var i = 0; i < copyRender.bones.Length; i++) {
				myBones [i] = Util.FindChildRecursive (root.transform, copyRender.bones [i].name);
			}
			render.bones = myBones;
			render.rootBone = Util.FindChildRecursive (root.transform, copyRender.rootBone.name);
		}

		public static int GetGoldDrop (int level)
		{
			var gd = Resources.Load<GameObject> ("graphics/stat/golddrop").GetComponent<GraphData> ();
			return Mathf.RoundToInt (gd.GetData (level));
		}

		public static string GetString (string key)
		{
			//return GameObject.FindObjectOfType<ItemToolTipFormat> ().GetString (key);
			//var it = Resources.Load<GameObject>("levelPublic/ItemToolTipFormat").GetComponent<ItemToolTipFormat>();
			//return it.GetString (key);
			var db = GMDataBaseSystem.database.GetJsonDatabase (GMDataBaseSystem.DBName.StrDictionary);
			return db.SearchForKey (key);
		}

		public static void ShowMsg (string st)
		{
			//var tips = NGUITools.AddMissingComponent<TipsPanel>(GameObject.FindGameObjectWithTag("UIRoot"));
			//tips.SetContent(st);
			//tips.ShowTips();

		}


		// 将时间戳转为字符串 
		public static string GetTimer(string _time)         
		{ 
			DateTime dt = new DateTime (1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
			dt = dt.AddSeconds (long.Parse(_time)/1000).ToLocalTime();
			//dt:3/5/2015 4:50:09 PM
			string date = dt.ToShortDateString().ToString();   
			string time = dt.ToLongTimeString().ToString();  
			string[] date_arr = date.Split('/'); 
			string[] time_arr = time.Split(':');     
			//string result = date_arr[0]+"月"+date_arr[1]+"日"+" "+time_arr[0]+"时"+time_arr[1]+"分";     
			string result = date_arr [2] + "/" + date_arr [0] + "/" + date_arr [1];
			return result;        
		} 


		public static int JobType(Job jobtype)
		{
			int type = -1;
			switch (jobtype) 
			{
			case Job.NOVICE:
				type = 0;
				break;
			case Job.WARRIOR:
				type = 1;
				break;
			case Job.ARMOURER:
				type = 2;
				break;
			case Job.ALCHEMIST:
				type = 3;
				break;
			case Job.STALKER:
				type = 4;
				break;
			default:
				type = 0;
				break;
			}
			return type;
			
		}

		//职位 类型显示中文
		public static string GetJobName(int type)
		{
			string jobName = "";
			switch (type) 
			{
			case 0:
				jobName = "新手";
				break;
			case 1:
				jobName = "战士";
				break;
			case 2:
				jobName = "枪械师";
				break;
			case 3:
				jobName = "炼金术师";
				break;
			case 4:
				jobName = "潜杀者";
				break;
			default:
				jobName = "新手";
				break;
			}
			return jobName;
		}

		public static void ShowLevelUp (int lev)
		{
			var lp = WindowMng.windowMng.PushTopNotify ("UI/LevelUpPanel").GetComponent<LevelUpPanel> ();
			lp.ShowLevelUp (lev);
		}

		public class Pair
		{
			public int moduleId;
			public int messageId;

			public Pair (int a, int b)
			{
				moduleId = a;
				messageId = b;
			}
		}
		public static Pair GetMsgID (string name)
		{
			return SaveGame.saveGame.GetMsgID (name);
		}

		public static Pair GetMsgID (string moduleName, string name)
		{
			Debug.Log ("moduleName " + moduleName + " " + name);
			var mId = SaveGame.saveGame.msgNameIdMap [moduleName] ["id"].AsInt;
			var pId = SaveGame.saveGame.msgNameIdMap [moduleName] [name].AsInt;
			return new Pair (mId, pId);
		}

		/*
		 * Message Builder
		 */ 
		/*
		public static IMessage GetMsg(int moduleId, int messageId, ByteString buf) {
			var module = SaveGame.saveGame.getModuleName(moduleId);
			var msg = SaveGame.saveGame.getMethodName(module, messageId);
			Debug.LogWarning ("modulename "+module+" "+msg);

			//var imsg = MessageUtil.GetDefaultMessage (msg);
			//IMessage retMsg = imsg.WeakCreateBuilderForType ().WeakMergeFrom (buf).WeakBuild();
			var tp = Type.GetType ("ChuMeng."+msg);
			Debug.LogWarning ("Message is "+tp);
			var pro = tp.GetProperty ("DefaultInstance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
			IMessage im = (IMessage)pro.GetValue (null, null);

			var retMsg = ChuMeng.GCPushChat2Client.CreateBuilder ().MergeFrom (buf).BuildPartial ();
			var retMsg = im.WeakCreateBuilderForType ().WeakMergeFrom (buf).WeakBuild ();

			return retMsg;
		}
		*/

		public static string GetMsgName (int moduleId, int messageId)
		{
			var module = SaveGame.saveGame.getModuleName (moduleId);
			var msg = SaveGame.saveGame.getMethodName (module, messageId);
			return msg;
		}

		public  static IEnumerator tweenRun (UITweener tp)
		{
			bool fin = false;
			tp.SetOnFinished (delegate() {
				fin = true;
			});
			tp.ResetToBeginning ();
			tp.enabled = true;
			while (!fin) {
				yield return null;
			}
		}

		public static IEnumerator tweenReverse (TweenPosition tp)
		{
			bool fin = false;
			var f = tp.from;
			var t = tp.to;
			tp.from = t;
			tp.to = f;
			tp.SetOnFinished (delegate() {
				fin = true;
			});
			tp.ResetToBeginning ();
			tp.enabled = true;
			while (!fin) {
				yield return null;
			}
			tp.from = f;
			tp.to = t;
		}

		/*
		 * 0 width-1  center.x
		 * 0 height-1 center.z
		 * NodeSize 1 1
		 */ 
		static void InitAstarPath ()
		{
			astarPath = AstarPath.active;
		}

		public static Vector2 GridToCoord (int x, int z)
		{
			InitAstarPath ();
			var gridGraph = astarPath.graphs [0] as Pathfinding.GridGraph;
			var center = gridGraph.center;
			var width = gridGraph.width;
			var height = gridGraph.depth;

			var v = new Vector2 ();
			v.x = x + center.x - width / 2.0f;
			v.y = z + center.z - height / 2.0f;
			return v;
		}

		public static Vector2 CoordToGrid (float x, float z)
		{
			InitAstarPath ();
			var gridGraph = astarPath.graphs [0] as Pathfinding.GridGraph;
			var center = gridGraph.center;
			var width = gridGraph.width;
			var height = gridGraph.depth;

			var v = new Vector2 ();
			v.x = (float)Math.Round (x - center.x + width / 2.0f);
			v.y = (float)Mathf.Round (z - center.z + height / 2.0f);

			return v;
		}

		public static bool CheckMovable(int x, int z) {
			InitAstarPath ();
			var gridGraph = astarPath.graphs[0] as Pathfinding.GridGraph;
			var width = gridGraph.width;
			var height = gridGraph.depth;
			if (x < 0 || z < 0 || x >= width || z >= height) {
				return false;
			}
			var index = z * gridGraph.width + x;
			if(index >= 0 && index < gridGraph.nodes.Length) {
				var n = gridGraph.nodes[index];
				var walk = n.Flags&0x1;
				if(walk == 0) {
					return false;
				}
				return true;
			}
			return false;
		}

		public static int IntYOffset (float v)
		{
			return Convert.ToInt32 ((v + 1000) * 1000);
		}

		public static float FloatYOffset (int v)
		{
			return (v / 1000.0f) - 1000;
		}

		static Dictionary<int, ItemData> itemDataCache = new Dictionary<int, ItemData> ();
		//根据BaseID 以及装备类型获得ItemData
		public static ItemData GetItemData (int propsOrEquip, int baseId)
		{
			int key = propsOrEquip * 1000000 + baseId;
			if (itemDataCache.ContainsKey (key)) {
				return itemDataCache [key];
			}
			//var allItem = Resources.LoadAll<ItemData>("units/items");
			ItemData id;
			id = new ItemData (propsOrEquip, baseId);
			itemDataCache [key] = id;
			return id;
		}

		static Dictionary<int, UnitData> monsterData = new Dictionary<int, UnitData> ();

		public static UnitData GetUnitData (bool isPlayer, int mid, int level)
		{
			//玩家才需要level 怪物的level都是0， 因此mid为玩家的job的时候*10足够了
			int key = Convert.ToInt32 (isPlayer) * 1000000 + mid * 10 + level;
			if (monsterData.ContainsKey (key)) {
				return monsterData [key];
			}

			UnitData ud = new UnitData (isPlayer, mid, level);
			monsterData [key] = ud;
			return ud;
		}

		static Dictionary<int, SkillData> skillData = new Dictionary<int, SkillData> ();

		public static SkillData GetSkillData (int skillId, int level)
		{
			int key = skillId * 1000000 + level;
			if (skillData.ContainsKey (key)) {
				return skillData [key];
			}
			SkillData sd = new SkillData (skillId, level);
			skillData [key] = sd;
			return sd;
		}

		public static GameObject InstanPlayer (Job job)
		{
			switch (job) {
			case Job.WARRIOR:
				break;
			case Job.ARMOURER:
				break;
			case Job.ALCHEMIST:
				break;
			case Job.STALKER:
				break;
			default:
				Debug.Log ("InstanPlayer Error UnknowJob " + job);
				break;
			}
			return null;
		}

		public static void SetIcon (UISprite icon, int sheet, string iconName)
		{
			Log.Important ("Why Altas Lost?" + icon + " " + sheet + " " + iconName);
			var atlas = Resources.Load<UIAtlas> ("UI/itemicons/itemicons" + sheet);
			if (icon.atlas == atlas && icon.spriteName == iconName) {
				return;
			}
			icon.atlas = atlas;
			icon.spriteName = iconName;
		}

		public static T ParseEnum<T> (string value)
		{
			try {
				return (T)Enum.Parse (typeof(T), value, true);
			} catch {
				return default(T);
			}
		}

		public static void SetLayer (GameObject g, GameLayer l)
		{
			g.layer = (int)l;
			foreach (Transform t in g.transform) {
				SetLayer (t.gameObject, l);
			}
		}

		public static string GetMoney (int type)
		{
			if (type == 0) {
				return "非绑银";
			} else if (type == 1) {
				return "绑银";
			} else if (type == 2) {
				return "绑金币";
			} else if (type == 3) {
				return "绑定金票";
			}
			return " error " + type.ToString ();
		}

		public static Vector3 CoordToGrid (Vector3 pos)
		{
			var p = CoordToGrid (pos.x, pos.z);
			var h = IntYOffset (pos.y);
			return new Vector3 (p.x, h, p.y);
		}

		static void CollectUIPanel (GameObject g, List<UIPanel> all)
		{
			if (g.GetComponent<UIPanel> () != null) {
				all.Add (g.GetComponent<UIPanel> ());
			}
			foreach (Transform t in g.transform) {
				CollectUIPanel (t.gameObject, all);
			}
		}

		public static List<UIPanel> GetAllPanel (GameObject g)
		{
			var ret = new List<UIPanel> ();
			CollectUIPanel (g, ret);
			return ret;
		}

		public static string Diffcult (int d)
		{
			if (d == 0) {
				return "简单";
			} else if (d == 1) {
				return "普通";
			} else {
				return "困难";
			}
		}

		public static int GetDiff (string w)
		{
			if (w == "简单") {
				return 0;
			}
			if (w == "普通") {
				return 1;
			}
			return 2;
		}

		public static string GetJob (int job)
		{
			return "职业";
		}

		public static void CreateUI ()
		{
			UIPanel p = NGUITools.CreateUI (false, (int)GameLayer.UICamera);
			p.tag = "UIRoot";
			var root = p.GetComponent<UIRoot> ();
			root.scalingStyle = UIRoot.Scaling.ConstrainedOnMobiles;
			root.manualWidth = 1024;
			root.manualHeight = 768;
			root.fitWidth = true;
			root.fitHeight = true;

		}
	}


}