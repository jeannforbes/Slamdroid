using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	private GameObject player;
	private PlayerMovement playerScript;
	private Rigidbody2D playerBody;
    private GameManager gameManager;
    
    private float roll;
    private Quaternion rollQuat;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		playerScript = player.GetComponent<PlayerMovement> ();
		playerBody = player.GetComponent<Rigidbody2D> ();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

    // Update is called once per frame
    void Update()
    {
        if (playerBody.position.y > this.transform.position.y)
        {
            this.transform.position = new Vector3(this.transform.position.x, playerBody.position.y, this.transform.position.z);
        }

        roll = Vector2.Angle(Vector2.down, gameManager.gravityDirection);
        if(gameManager.gravityDirection.x > 0)
        {
            roll *= -1;
        }
        rollQuat = Quaternion.AngleAxis(roll, Vector3.forward);
        gameObject.transform.rotation = rollQuat;
    }
}
