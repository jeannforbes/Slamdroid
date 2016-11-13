using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	private GameObject Player;
    private float recoilTime = 0;

    private Vector2 startLocation;
    private Quaternion startRotation;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");

        startLocation = gameObject.transform.localPosition;
        startRotation = gameObject.transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
        if (!this.GetComponent<Collider2D>().enabled)
        {
            //this.gameObject.transform.Rotate(0, 0, -14.0f * Time.deltaTime);
            if(recoilTime > 0)
            {
                recoilTime -= Time.deltaTime;
                this.gameObject.transform.Translate(new Vector2(8.0f * Time.deltaTime, 5.0f * Time.deltaTime));
            }
            else
            {
                recoilTime = 0;
            }
        }
        if (transform.position.y < -40)
        {
            //GameObject.Destroy(gameObject);
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.enabled = false;
        }
	}

    //Handles the collision between the player and obstacle.
	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject == Player) {
            other.gameObject.GetComponent<Rigidbody2D>().velocity *= 0.9f;
            this.GetComponent<Collider2D>().enabled = false;
			this.GetComponent<Rigidbody2D>().Sleep();
            //this.gameObject.transform.Translate(new Vector2(100.0f * Time.deltaTime, 30.0f * Time.deltaTime));
            //this.gameObject.transform.Rotate(0,0,-500.0f*Time.deltaTime);
            Player.GetComponent<Player>().Cans++;
            recoilTime = 20.0f;
		}
	}

    //Resets obstacle object for a new run.
    public void Reset()
    {
        gameObject.transform.localPosition = startLocation;
        gameObject.transform.localRotation = startRotation;

        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<Rigidbody2D>().WakeUp();
        recoilTime = 0;
    }
}
