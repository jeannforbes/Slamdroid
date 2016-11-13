using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float PassSpeed = 20f;

	private GameObject Player;
	private Player playerScript;

    private Vector2 startLocation;

    // Use this for initialization
    void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
		playerScript = Player.GetComponent ("Player") as Player;

        startLocation = gameObject.transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject == Player) {
            if (other.attachedRigidbody.velocity.x >= PassSpeed)
            {
                print("Enemy defeated.");
                playerScript.Cans += 3;
                //Destroy(this.gameObject);
                this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                enabled = false;
            }
            else
            {
                other.attachedRigidbody.velocity = Vector2.zero;
                playerScript.Reset();
            }
		}
	}

    //Resets enemy object for a new run.
    public void Reset()
    {
        gameObject.transform.localPosition = startLocation;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
