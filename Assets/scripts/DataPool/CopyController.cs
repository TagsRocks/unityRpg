using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng {

	//副本信息关卡信息
	public class LevelInfo
	{
		public CopyInfo levelServer;
		public DungeonConfigData levelLocal;
		public int levelIndex; //关卡在章节中的编号
		public string LevelName {
			get {
				return levelLocal.name;
			}
		}
		public int CopyId {
			get {
				return levelLocal.id;
			}
		}
		public LevelInfo() {
		}

		public LevelInfo(DungeonConfigData d) {
			levelLocal = d;
		}
	}

	public class CopyController : MonoBehaviour{
		public bool InitYet = false;
		GCCopyInfo copyInfo;
		bool inInit = false;

		public int SelectChapter;
		public LevelInfo SelectLevelInfo;

		public static CopyController copyController;
		void Awake() {
			gameObject.name = "CopyController";
			copyController = this;
			DontDestroyOnLoad (gameObject);
		}

		IEnumerator Init() {
			CGCopyInfo.Builder cp = CGCopyInfo.CreateBuilder ();
			var packet = new KBEngine.PacketHolder ();
			yield return StartCoroutine (KBEngine.Bundle.sendSimple(this, cp, packet));
			copyInfo = packet.packet.protoBody as GCCopyInfo;
			InitYet = true;
			inInit = true;
			Log.Important ("Copy Controller Init Over "+copyInfo.ToString());
			MyEventSystem.myEventSystem.PushEvent (MyEvent.EventType.UpdateCopy);

		}

		public IEnumerator InitFromNetwork() {
			if (!InitYet && !inInit) {
				yield return StartCoroutine(Init());
			}
		}
		List<LevelInfo> GetAllChapterLevelInfo(int chapter) {
			List<LevelInfo> l = new List<LevelInfo> ();
			foreach (DungeonConfigData d in GameData.DungeonConfig) {
				if(d.Chapter == chapter) {
					var lv = new LevelInfo(d);
					l.Add(lv);
				}
			}
			return l;
		}
		//得到所有 可以组队进入的副本的列表
		public List<LevelInfo> GetAllCopy() {
			List<LevelInfo> l = new List<LevelInfo>();
			foreach (DungeonConfigData d in GameData.DungeonConfig) {
				var lv = new LevelInfo(d);
				l.Add(lv);
			}
			return l;
		}

		//得到某个副本或者城镇场景的数据
		public DungeonConfigData GetLevelInfo(int levId) {
			foreach (DungeonConfigData d in GameData.DungeonConfig) {
				if(d.id == levId) {
					return d;
				}
			}
			return null;
		}



		//获得某个的章节的level信息
		public List<LevelInfo> GetChapterLevel(int chapter) {
			Log.GUI ("GetChapterLevel "+chapter);
			List<LevelInfo> allLevels = null;
			//int c = 0;
			Log.GUI ("copyInfoLength "+copyInfo.CopyInfoCount);
			int curChapter = GetCurrentChapter ();
			int curLevel = GetCurrentLevel ();

            Log.Sys("curChapter chapter "+curChapter+" chapter "+chapter+" curLevel "+curLevel);
			if (curChapter > chapter) {
				allLevels = GetAllChapterLevelInfo(chapter);
				foreach(LevelInfo l in allLevels) {
					var cin = CopyInfo.CreateBuilder();
					cin.IsPass = true;
					l.levelServer = cin.BuildPartial();

				}
			} else if (curChapter < chapter) {
				allLevels = new List<LevelInfo>();
			} else {
				allLevels = GetAllChapterLevelInfo(chapter);
				foreach(LevelInfo l in allLevels) {
					if(l.CopyId <= curLevel) {
						var cin = CopyInfo.CreateBuilder();
						cin.IsPass = true;
						l.levelServer = cin.BuildPartial();
					}else {
						var cin = CopyInfo.CreateBuilder();
						cin.IsPass = false;
						l.levelServer = cin.BuildPartial();
					}
				}
			}

			return allLevels;
		}

		int GetLastChapter() {
			var last = GameData.DungeonConfig[GameData.DungeonConfig.Count-1];
			return last.Chapter;
		}

		//获得当前章节的id
		public int GetCurrentChapter() {
			int lastId = 0;
			foreach(CopyInfo c in copyInfo.CopyInfoList) {
				if(!c.IsPass) {
					var linfo1 = GetLevelInfo(c.Id);
					return linfo1.Chapter;
				}else {
					lastId = c.Id;
				}
			}

			lastId++;
			var linfo = GetLevelInfo(lastId);
			if(linfo == null) {
				return GetLastChapter();
			}
			return 1;
		}
	
		//获得当前开放的关卡
		int GetCurrentLevel() {
			int lastId = 0;
			foreach(CopyInfo c in copyInfo.CopyInfoList) {
				if(!c.IsPass) {
					return c.Id-1;
				}else {
					lastId = c.Id;
				}
			}
			lastId++;
			var linfo = GetLevelInfo (lastId);
			//All Level Open
			if (linfo == null) {
				return GameData.DungeonConfig[GameData.DungeonConfig.Count-1].id;
			}
			return lastId;
		}


		//选择特定的关卡
		public void SelectLevel(int chapter, LevelInfo lev) {
			SelectChapter = chapter;
			SelectLevelInfo = lev;
		}

		//进入当前选择的副本
		public void EnterLevel() {
		}


		public string ChapterName(int cha) {
			Log.GUI ("GetChapter Name "+cha);
			var chapter = GMDataBaseSystem.SearchIdStatic<ChapterConfigData> (GameData.ChapterConfig, cha);
			return chapter.name;
		}

		public int GetMaxChapterId() {
			var chapter = GameData.ChapterConfig [GameData.ChapterConfig.Count - 1];
			return chapter.id;
		}
	}
}
