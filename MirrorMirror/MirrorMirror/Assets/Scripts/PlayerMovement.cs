using UnityEngine;
using System.Collections;

public enum MoveState{ wallLeft, wallRight, jumpLeft, jumpRight, start, dead };

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (CircleCollider2D))]
public class PlayerMovement : MonoBehaviour {

	public float speed = 1000f;
	public float jumpStrength = 1000f;
	public MoveState moveState = MoveState.start;
	public int score = 0;

	private Rigidbody2D rbody;
	private Canvas canvas;

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody2D> ();
		canvas = FindObjectOfType<Canvas> ();
	}
	
	// Update is called once per frame
	void Update () {

		//Update score
		if (score < rbody.position.y) {
			score = (int)rbody.position.y;
			canvas.transform.GetChild (1).GetComponent<UnityEngine.UI.Text> ().text =  ""+score;
		}

		//Jumping from right to left
		if ((Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow)) 
			&& (moveState == MoveState.wallRight || moveState == MoveState.start)) {
			rbody.gravityScale = 0f;
			rbody.AddForce (Vector2.up * jumpStrength);
			moveState = MoveState.jumpLeft;
		}
        //Jumping from left to right
        else if ((Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)) 
			&& (moveState == MoveState.wallLeft || moveState == MoveState.start)) {
			rbody.gravityScale = 0f;
			rbody.AddForce (Vector2.up * jumpStrength);
			moveState = MoveState.jumpRight;
		}
        //Jumping from right to right
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            && (moveState == MoveState.wallRight))
        {
            rbody.gravityScale = 0f;
            rbody.AddForce((new Vector2(-1f, 1f)) * jumpStrength);
            moveState = MoveState.jumpRight;
        }
        //Jumping from left to left
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            && (moveState == MoveState.wallLeft)){
            rbody.gravityScale = 0f;
            rbody.AddForce( ( new Vector2(1f,1f) ) * jumpStrength);
            moveState = MoveState.jumpLeft;
        }
        //Player is dead
        else if ( Input.anyKeyDown && moveState == MoveState.dead) {
			Reset ();
		}

		//handle current movement state
		switch (moveState) {
		case MoveState.jumpLeft:
			rbody.AddForce( new Vector2(-speed, 0) );
			break;
		case MoveState.jumpRight:
			rbody.AddForce( new Vector2(speed, 0) );
			break;
		case MoveState.wallLeft:

			break;
		case MoveState.wallRight:

			break;
		case MoveState.dead:
			canvas.GetComponentInChildren<UnityEngine.UI.Text>().enabled = true;
			break;
		default:
			break;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		switch (moveState) {
		case MoveState.jumpLeft:
			rbody.velocity = Vector2.zero;
			rbody.gravityScale = 1f;
			moveState = MoveState.wallLeft;
			break;
		case MoveState.jumpRight:
			rbody.velocity = Vector2.zero;
			rbody.gravityScale = 1f;
			moveState = MoveState.wallRight;
			break;
		default:
			break;
		}
		Debug.Log (moveState);
	}

	void Reset(){
		canvas.GetComponentInChildren<UnityEngine.UI.Text>().enabled = false;
		GameObject camera = GameObject.FindWithTag ("MainCamera");
		camera.transform.position = new Vector3 (0, 0, camera.transform.position.z);
		rbody.position = Vector2.zero;
		moveState = MoveState.start;
	}
}
