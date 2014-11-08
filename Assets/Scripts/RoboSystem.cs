using UnityEngine;
using System.Collections;

public class RoboSystem : MonoBehaviour {
	
	public Collider startTrigger;
	public Collider endTrigger;
	public GameObject robot;

	void Start () {
		Instantiate (startTrigger, new Vector2((gameObject.transform.position.x - gameObject.transform.localScale.x/2),gameObject.transform.position.y), Quaternion.identity);
		Instantiate (endTrigger, new Vector2((gameObject.transform.position.x + gameObject.transform.localScale.x/2),gameObject.transform.position.y), Quaternion.identity);
		Instantiate (robot, new Vector2(gameObject.transform.position.x,gameObject.transform.position.y), Quaternion.identity);
	}
}
