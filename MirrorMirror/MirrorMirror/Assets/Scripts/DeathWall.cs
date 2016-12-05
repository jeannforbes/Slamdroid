using UnityEngine;
using System.Collections;

public class DeathWall : MonoBehaviour {

	public float speed = 1f;
	public float maxSpeed = 10f;

	protected PlayerMovement playerScript;
	protected Rigidbody2D playerBody;

	private StateManager stateManager;
	private GameObject player;
	private Vector2 startingPos;

	// Use this for initialization
	void Start () {
		stateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<StateManager> ();
		player = GameObject.FindWithTag("Player");
		playerScript = player.GetComponent<PlayerMovement>();
		playerBody = player.GetComponent<Rigidbody2D>();

		startingPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetComponent<PlayerMovement> ().moveState == MoveState.dead)
			this.GetComponent<Collider2D> ().enabled = false;

		if (speed < maxSpeed)
			speed += 0.001f;
		if (player.GetComponent<PlayerMovement> ().moveState == MoveState.start) {
			Reset ();
		} else if (stateManager.state == StateManager.GameState.Play) {
			this.transform.Translate (new Vector3 (0f, 0.05f * speed, 0f));
			if (this.transform.position.y < player.transform.position.y - 15f){
				this.transform.position = new Vector2(0f, player.transform.position.y - 15f);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject == player && playerScript.moveState != MoveState.start)
		{
			playerScript.moveState = MoveState.dead;
			playerBody.velocity = Vector2.zero;
			playerBody.gravityScale = 3f;
			playerBody.AddForce( Vector2.up * 900f);
			playerBody.angularVelocity = 700f;
		}
		
	}

	void Reset(){
		this.GetComponent<Collider2D> ().enabled = true;
		this.transform.position = startingPos;
	}
}
