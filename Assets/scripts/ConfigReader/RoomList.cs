using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class RoomList : MonoBehaviour {
    public List<GameObject> roomPieces = new List<GameObject>();
    public void AddRoom(GameObject g){
        roomPieces.Add(g);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
