using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Vector3 accel, vel;
	public float friction = 0.1f;

	public Vector3 Accel{
		get{ return accel; }
		set{ accel = value; }
	}

	public Vector3 Vel{
		get{ return vel; }
		set{ vel = value; }
	}

	// Use this for initialization
	void Start () {
		accel = new Vector3 ();
		vel = new Vector3 ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			accel += Vector3.right * Time.deltaTime * 1000;
		}
		if(vel.x > 0) vel.x -= friction;
		if (vel.x < 0) vel.x += friction;

		vel += accel;
		GetComponent<Rigidbody2D> ().AddForce (vel);
		accel = Vector3.zero;
	}
}
