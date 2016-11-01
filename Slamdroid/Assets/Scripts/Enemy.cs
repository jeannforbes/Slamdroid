using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float PassSpeed = 20f;

	private GameObject Player;
	private Player playerScript;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
		playerScript = Player.GetComponent ("Player") as Player;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject == Player) {
			if (other.attachedRigidbody.velocity.x >= PassSpeed)
				Destroy (this.gameObject);
			else {
				other.attachedRigidbody.velocity = Vector2.zero;
				playerScript.Reset();
			}
		}
	}

}
