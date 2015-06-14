using UnityEngine;
using System.Collections;

/*
 * Test Chat And Attribute Panel
 */ 
namespace ChuMeng {
	public class ChatButton : MonoBehaviour {
		public GameObject chatButton;
		public GameObject chatPanel;

		public GameObject attr;
		public GameObject debugPanel;
		void Awake() {
			if (chatButton) {
				UIEventListener.Get (chatButton).onClick = OnChat;
				chatPanel.SetActive (false);
			}

			UIEventListener.Get (attr).onClick = OnAttr;
			debugPanel.SetActive (false);
		}
		void OnAttr(GameObject g) {
			debugPanel.SetActive (true);
			StartCoroutine (ShowAttr ());
		}

		IEnumerator ShowAttr() {

			//var player = ObjectManager.objectManager.GetMyPlayer ();
			yield return null;
			/*
			if (player != null) {
				var charInfo = player.GetComponent<CharacterInfo> ();
				while (!charInfo.InitPropertyOver) {
					yield return null;
				}
				string st = "";
				foreach (System.Collections.Generic.KeyValuePair<CharAttribute.CharAttributeEnum, int> kv in charInfo.propertyValue) {
					st += kv.Key.ToString() + " " + kv.Value.ToString () + "\n";
				}
				//debugPanel.GetComponent<DebugPanel> ().label.text = st;
				debugPanel.GetComponent<DebugPanel> ().SetText(st);
			}
			*/
		}

		void OnChat(GameObject g) {
			chatPanel.SetActive (true);
		}
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}

}