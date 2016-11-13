using UnityEngine;
using System.Collections;

public enum Direction{ Right, Left, Up, Down };

[RequireComponent (typeof (BoxCollider2D))]

public class Booster : MonoBehaviour {

	public Direction direction = Direction.Right;
	public float boostAmount = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<Rigidbody2D> ()) {
			if(direction == Direction.Right) other.GetComponent<Rigidbody2D> ().AddForce (Vector2.right * boostAmount);
			if(direction == Direction.Left) other.GetComponent<Rigidbody2D> ().AddForce (Vector2.left * boostAmount);
			if(direction == Direction.Up) other.GetComponent<Rigidbody2D> ().AddForce (Vector2.up * boostAmount);
			if(direction == Direction.Down) other.GetComponent<Rigidbody2D> ().AddForce (Vector2.down * boostAmount);
		}
	}
}
