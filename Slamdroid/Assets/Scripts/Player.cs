using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private int money;

    private GameObject cam;
    //private bool canAccel = true;

	private Vector2 accel;
	public float friction = 10f;

    //Starting boost
    private float zOffset;
    private float boostTimer = 0;
    private float boostFactor = 0;
    private const float MAX_ACCEL = 10000f;
    private GameObject boostLine;
	private GameObject boostBar;
    
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

    //Gets and sets the money the player has.
    //other classes can only decrement this value.
    public int Money
    {
        get { return money; }
        set
        {
            if(value < money)
            {
                money = value;
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
    }
	
	// Update is called once per frame
	void Update () {
        //print("Player State: " + playerState + "\n");
        switch (playState)
        {
            case PlayerState.Ready:
                boostTimer += Time.deltaTime;
                boostFactor = -Mathf.Cos(boostTimer * 5);

                Vector3 barPosition = 1.5f * Vector2.up * boostFactor;
                barPosition.z = zOffset;

                boostLine.transform.localPosition = barPosition;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    accel += Vector2.right * Time.deltaTime * MAX_ACCEL * (1 - Mathf.Abs(boostFactor));
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
                break;
        }

        //Update camera position
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, -35);
    }

	public void Reset(){
		transform.position = Vector2.zero;
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
}
