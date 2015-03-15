using UnityEngine;
using System.Collections;

namespace ChuMeng
{
	public class PlayerWindow : MonoBehaviour
	{
		public static PlayerWindow playerWindow;
		void Awake() {
			playerWindow = this;
			DontDestroyOnLoad (gameObject);
		}
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	}

}