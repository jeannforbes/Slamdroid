using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	private GameObject Player;
    private float recoilTime = 0;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (!this.GetComponent<Collider2D>().enabled)
        {
            this.gameObject.transform.Rotate(0, 0, -14.0f * Time.deltaTime);
            if(recoilTime > 0)
            {
                recoilTime -= Time.deltaTime;
                this.gameObject.transform.Translate(new Vector2(8.0f * Time.deltaTime, 5.0f * Time.deltaTime));
            }
        }
        if (transform.position.y < -40)
        {
            GameObject.Destroy(gameObject);
        }
	}

    //Handles the collision between the player and obstacle.
	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject == Player) {
            other.gameObject.GetComponent<Player>().Accel -= Vector2.right * Time.deltaTime * 1000;
            this.GetComponent<Collider2D>().enabled = false;
            //this.gameObject.transform.Translate(new Vector2(100.0f * Time.deltaTime, 30.0f * Time.deltaTime));
            //this.gameObject.transform.Rotate(0,0,-500.0f*Time.deltaTime);
            recoilTime = 20.0f;
		}
	}


}
