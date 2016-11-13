using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private int cans;
    private Vector2 startPosition;

    private GameObject cam;
    //private bool canAccel = true;

	private Vector2 accel;
	public float friction = 10f;

    //Starting boost
    private float zOffset;
    private float boostTimer = 0;
    private float boostFactor = 0;
    private float maxAccel = 3000f;
    private float barSpeed = 6f;
    private GameObject boostLine;
	private GameObject boostBar;

    private GameObject gameManager;
    
    //player's state
    public enum PlayerState
    {
        Ready, Running, Stopped
    }
    private PlayerState playState;

    //Gets and sets the acceleration vector.
    public Vector2 Accel{
		get{ return accel; }
		set{ accel = value; }
	}

    //Gets the player state.
    public PlayerState PlayState
    {
        get { return playState; }
    }

    //Gets and sets the cans(money) the player has.
    //other classes can only decrement this value.
    public int Cans
    {
        get { return cans; }
        set
        {
            if(value >= 0)
            {
                cans = value;
            }
        }
    }

	void Start(){
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		boostBar = GameObject.FindGameObjectWithTag ("BoostBar");
		boostLine = boostBar.transform.FindChild ("BoostLine").gameObject;
		GetComponent<Rigidbody2D> ().freezeRotation = true;

        playState = PlayerState.Stopped;

        zOffset = boostLine.transform.localPosition.z;

        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        startPosition = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        //print("Player State: " + playerState + "\n");
        switch (playState)
        {
            case PlayerState.Ready:
                boostTimer += Time.deltaTime;
                boostFactor = -Mathf.Cos(boostTimer * barSpeed);

                Vector3 barPosition = 1.5f * Vector2.up * boostFactor;
                barPosition.z = zOffset;

                boostLine.transform.localPosition = barPosition;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    accel += Vector2.right * Time.deltaTime * maxAccel * (1 - Mathf.Abs(boostFactor));
                    //canAccel = false;
                    playState = PlayerState.Running;

                    boostBar.GetComponent<SpriteRenderer>().enabled = false;
                    boostLine.GetComponent<SpriteRenderer>().enabled = false;
                }
                break;
            case PlayerState.Running:
                GetComponent<Rigidbody2D>().velocity += accel;
                accel = Vector2.zero;

                //We need some way to deal with the player hitting 0 velocity.

                if (GetComponent<Rigidbody2D>().velocity == Vector2.zero)
                {
                    playState = PlayerState.Stopped;
                }
                break;
            case PlayerState.Stopped:
                Reset();
                gameManager.GetComponent<StateManager>().worldReset();
                break;
        }

        //Update camera position
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, -35);
    }

    //Resets player object for a new run.
	public void Reset(){
        //canAccel = true;
        playState = PlayerState.Ready;
        boostTimer = 0;

        boostBar.GetComponent<SpriteRenderer>().enabled = true;
        boostLine.GetComponent<SpriteRenderer>().enabled = true;
    }

    void OnEnable()
    {
        playState = PlayerState.Ready;
    }

    //Increases the rate at which a character is boosted at the start.
    public void increaseBoost()
    {
        print("Speed boosted!");
        maxAccel += 1500;
    }

    //Decreases the speed of the boost bar.
    public void decreaseBarSpeed()
    {
        barSpeed -= 1.5f;
    }
}
