using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (CircleCollider2D))]
public class Spike : MonoBehaviour {

	private GameObject player;
	private PlayerMovement playerScript;
	private Rigidbody2D playerBody;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		playerScript = player.GetComponent<PlayerMovement> ();
		playerBody = player.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject == player && playerScript.moveState != MoveState.dead) {
			playerScript.moveState = MoveState.dead;
			playerBody.velocity = Vector2.zero;
			playerBody.gravityScale = 3f;
			playerBody.AddForce( Vector2.up * 700f );
		}
	}
}
