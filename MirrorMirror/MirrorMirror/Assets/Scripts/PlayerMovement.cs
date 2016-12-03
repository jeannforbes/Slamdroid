using UnityEngine;
using System.Collections;

public enum MoveState{ wallLeft, wallRight, jumpLeft, jumpRight, start, dead };
public enum Direction { left,right}

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (BoxCollider2D))]
public class PlayerMovement : MonoBehaviour {

	public float speed = 1000f;
	public float jumpStrength = 1000f;
	public MoveState moveState = MoveState.start;
	public int score = 0;
    public int money = 0;
	public Sprite wallSprite;
	public Sprite jumpSprite;

	private Rigidbody2D rbody;
	private Canvas canvas;
	private Camera camera;
	private GameObject gameManager;

    private Vector2 touchPoint;

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody2D> ();
		canvas = FindObjectOfType<Canvas> ();
		camera = FindObjectOfType<Camera> ();
		gameManager = GameObject.FindGameObjectWithTag ("GameManager");
	}
	
	// Update is called once per frame
	void Update () {

		//Update score
		if (score < rbody.position.y) {
			score = (int)rbody.position.y;
			canvas.transform.GetChild (1).GetComponent<UnityEngine.UI.Text> ().text =  ""+score;
		}

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
        //Desktop controls

        //Left jump
        if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow)) {
            Jump(Direction.left);
		}
        //Right jump
        else if (Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)) {
            Jump(Direction.right);
		}
        //Player is dead
        if ( Input.anyKeyDown && moveState == MoveState.dead) {
			Reset ();
		}
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        //Mobile controls
        if (Input.touchCount > 0)
        {
            Touch nextTouch = Input.touches[0];
            int i = 1;
            //Loops through the list of touches for the first touch that began this frame.
            while(i < Input.touchCount && nextTouch.phase != TouchPhase.Began)
            {
                nextTouch = Input.touches[i];
                i++;
            }

            //If a touch began this turn check controls.
            if(nextTouch.phase == TouchPhase.Began)
            {
                touchPoint = nextTouch.position;

                //Player is dead
                if (moveState == MoveState.dead)
                {
                    Reset();
                }
                //Left jump
                else if (touchPoint.x < 0)
                {
                    Jump(Direction.left);
                }
                //Right jump
                else if (touchPoint.x > 0)
                {
                    Jump(Direction.right);
                }
            }
        }
#endif

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
			if(rbody.transform.localScale.x > 0){
				rbody.GetComponent<SpriteRenderer>().sprite = wallSprite;
				rbody.transform.localScale = new Vector3(-rbody.transform.localScale.x, rbody.transform.localScale.y, rbody.transform.localScale.z);
			}
			break;
		case MoveState.jumpRight:
			rbody.velocity = Vector2.zero;
			rbody.gravityScale = 1f;
			moveState = MoveState.wallRight;
			if(rbody.transform.localScale.x < 0){
				rbody.GetComponent<SpriteRenderer>().sprite = wallSprite;
				rbody.transform.localScale = new Vector3(-rbody.transform.localScale.x, rbody.transform.localScale.y, rbody.transform.localScale.z);
			}
			break;
		default:
			break;
		}
	}

	void OnTriggerStay2D(Collider2D other){
		switch (moveState) {
		case MoveState.wallLeft:
			rbody.GetComponent<SpriteRenderer>().sprite = wallSprite;
			//rbody.position = new Vector2(-4f, rbody.position.y);
			break;
		case MoveState.wallRight:
			rbody.GetComponent<SpriteRenderer>().sprite = wallSprite;
			//rbody.position = new Vector2(4f, rbody.position.y);
			break;
		default:
			break;
		}
	}

	void Reset(){
		canvas.GetComponentInChildren<UnityEngine.UI.Text>().enabled = false;
		camera.transform.position = new Vector3 (0, 0, camera.transform.position.z);
		rbody.position = Vector2.zero;
		rbody.velocity = Vector2.zero;
		rbody.rotation = 0f;
		rbody.angularVelocity = 0f;

		gameManager.GetComponent<GameManager> ().currentHeight = 0f;
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag ("Obstacle");
		for (int i=0; i<obstacles.Length; i++)
			Destroy (obstacles [i]);


		moveState = MoveState.start;
	}

    //Method takes the direction the player wants to move and executes the movement.
    //If the player is moving to the other wall they cross the intemediate space, otherwise they jump upwards in space.
    void Jump(Direction direction)
    {
        //moving to the left
        if (direction == Direction.left)
        {
			rbody.GetComponent<SpriteRenderer>().sprite = jumpSprite;
            //Jumping from right to left
            if (moveState == MoveState.wallRight || moveState == MoveState.start)
            {
                rbody.gravityScale = 0f;
				rbody.AddForce((new Vector2(1f * speed, 1f * jumpStrength)));
                moveState = MoveState.jumpLeft;
            }
            //Jumping from left to left
            else if (moveState == MoveState.wallLeft)
            {
                rbody.gravityScale = 0f;
				rbody.AddForce((new Vector2(1f * speed, 1f * jumpStrength)));
                moveState = MoveState.jumpLeft;
            }
        }
        //moving to the right
        else if (direction == Direction.right)
        {
			rbody.GetComponent<SpriteRenderer>().sprite = jumpSprite;
            //Jumping from right to right
            if (moveState == MoveState.wallRight)
            {
                rbody.gravityScale = 0f;
                rbody.AddForce((new Vector2(-1f, 1f)) * jumpStrength);
                moveState = MoveState.jumpRight;
            }
            //Jumping from left to right
            else if (moveState == MoveState.wallLeft || moveState == MoveState.start)
            {
                rbody.gravityScale = 0f;
                rbody.AddForce(Vector2.up * jumpStrength);
                moveState = MoveState.jumpRight;
            }
        }
    }
}
