using UnityEngine;
using System.Collections;

public class RoboMovement : MonoBehaviour {

	public float speed = 10f;
	public float bulletSpeed = 100f;
	public int bulletRate = 2;
	public GameObject bullet;
	private bool right = true;

	void Start()
	{
		InvokeRepeating("fire", bulletRate,bulletRate);
	}

	void Update () {
		if(right)
		{
			gameObject.transform.position = new Vector2 (gameObject.transform.position.x + speed * Time.deltaTime,gameObject.transform.position.y);
		}
		else if(!right)
		{
			gameObject.transform.position = new Vector2 (gameObject.transform.position.x - speed * Time.deltaTime,gameObject.transform.position.y);
		}
	}

	void OnTriggerEnter(Collider block){
		if(block.tag == "startTrigger")
		{
			right = true;
		}
		if(block.tag == "endTrigger")
		{
			right = false;
		}
	}

	void fire()
	{
		GameObject clone;

		clone = Instantiate (bullet,transform.position , Quaternion.identity) as GameObject;
		if (right)
		{
			clone.rigidbody.AddForce(bulletSpeed,0,0);
		}
		else if(!right)
		{
			clone.rigidbody.AddForce(-bulletSpeed,0,0);
		}
	}
}
