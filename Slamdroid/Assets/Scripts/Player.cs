using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private GameObject cam;
	private bool canAccel = true;
	private Vector2 accel;
	public float friction = 10f;

	public Vector2 Accel{
		get{ return accel; }
		set{ accel = value; }
	}

	void Start(){
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		//Player input
		if (canAccel && Input.GetKeyDown (KeyCode.Space)) {
			accel += Vector2.right * Time.deltaTime * 2000;
			//canAccel = false;
		}

		GetComponent<Rigidbody2D>().velocity += accel;
		accel = Vector2.zero;

		//Update camera position
		cam.transform.position = new Vector3(transform.position.x, transform.position.y, -35);
	}

	public void Reset(){
		transform.position = Vector2.zero;
		canAccel = true;
	}
}
