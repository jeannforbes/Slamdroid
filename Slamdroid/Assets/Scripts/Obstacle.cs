using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	private GameObject Player;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject == Player) {
			this.GetComponent<Collider2D>().enabled = false;
		}
	}


}
