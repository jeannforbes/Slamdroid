using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			var playerVelocity = coll.gameObject.GetComponent<Rigidbody2D>().velocity;
			coll.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2(playerVelocity.x * -2, playerVelocity.y);
		}
		
	}
}
