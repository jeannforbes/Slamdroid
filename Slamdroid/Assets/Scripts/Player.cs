using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private GameObject cam;
    private bool canAccel = true;

	private Vector2 accel;
	public float friction = 10f;

    //Starting boost
    private float boostTimer = 0;
    private float boostFactor = 0;
    private const float MAX_ACCEL = 10000f;
    public GameObject boostLine;
    public GameObject boostBar;

    public Vector2 Accel{
		get{ return accel; }
		set{ accel = value; }
	}

	void Start(){
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		GetComponent<Rigidbody2D> ().freezeRotation = true;
	}
	
	// Update is called once per frame
	void Update () {
        //Player input
        /*if (canAccel && Input.GetKeyDown (KeyCode.Space)) {
			accel += Vector2.right * Time.deltaTime * 2000;
			//canAccel = false;
		}*/
        if (canAccel)
        {
            boostTimer += Time.deltaTime;
            boostFactor = -Mathf.Cos(boostTimer*5);

            Vector3 barPosition = 1.5f * Vector2.up * boostFactor;
            barPosition.z = -0.25f;

            boostLine.transform.localPosition = barPosition;

            if(Input.GetKeyDown(KeyCode.Space))
            {
                accel += Vector2.right * Time.deltaTime * MAX_ACCEL * (1 - Mathf.Abs(boostFactor));
                canAccel = false;

                boostBar.GetComponent<SpriteRenderer>().enabled = false;
                boostLine.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        GetComponent<Rigidbody2D>().velocity += accel;
		accel = Vector2.zero;

        /*We need some way to deal with the player hitting 0 velocity.
         * 
        if (!canAccel && GetComponent<Rigidbody2D>().velocity == Vector2.zero)
        {
            Reset();
        }
        */

        //Update camera position
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, -35);


	}

	public void Reset(){
		transform.position = Vector2.zero;
		canAccel = true;
        boostTimer = 0;

        boostBar.GetComponent<SpriteRenderer>().enabled = true;
        boostLine.GetComponent<SpriteRenderer>().enabled = true;
    }
}
