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
            other.gameObject.GetComponent<Player>().Accel -= Vector2.right * Time.deltaTime * 1000;
            this.gameObject.transform.Translate(new Vector2(20.0f * Time.deltaTime, 20.0f * Time.deltaTime));
            this.gameObject.transform.Rotate(0,0,15.0f*Time.deltaTime);
            this.GetComponent<Collider2D>().enabled = false;
		}
	}


}
