using UnityEngine;
using System.Collections;


[RequireComponent(typeof(MovementSystem))]
public class PlayerNetwork : MonoBehaviour {
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
    MovementSystem moveSystem;
    PlayerInput playerinput;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	

    

}
