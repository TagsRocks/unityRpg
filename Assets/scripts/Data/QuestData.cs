
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

public class QuestData : MonoBehaviour {
	public string QuestDisplayName = "Quest1";
	public string Dungeon = "main";
	public bool Canabandon = true;
	public bool ShowQuestComplete = true;

	public string UnitName = "vasman";

	[Multiline()]
	public string Intro;
	[Multiline()]
	public string Return;
	[Multiline()]
	public string Complete;
	[Multiline()]
	public string Details;

	
	public string AcquireItem;
	public int SpecificFloor = 0;
	public int MinCount = 1;
	public int MaxCount = 1;
	public bool RemoveFromInventory = true;


	//Random Gem Reward
	public int RewardGold;
	public int RewardXP;
	public string TreasureReward;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
