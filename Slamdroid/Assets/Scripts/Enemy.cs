using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

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
		if (other.CompareTag ("Player")) {
			Player.GetComponent<Rigidbody2D>().AddForce(playerScript.Vel * -20);
		}
	}

}
