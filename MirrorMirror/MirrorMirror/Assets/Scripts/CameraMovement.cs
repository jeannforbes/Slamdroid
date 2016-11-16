using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

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
		if (playerBody.position.y > this.transform.position.y) {
			this.transform.position = new Vector3 (this.transform.position.x, playerBody.position.y, this.transform.position.z);
		} else if (playerBody.position.y+7 < this.transform.position.y) {
			playerScript.moveState = MoveState.dead;
		}
	}
}
