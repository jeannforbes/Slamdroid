using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Obstacle : MonoBehaviour
{

    private GameObject player;
    private PlayerMovement playerScript;
    private Rigidbody2D playerBody;

    public Obstacle()
    {
        
    }

    ////////Warning children do not call parent's events////////

    // Use this for initialization
    void Start()
    {
        this.Init();
    }

    // Update is called once per frame
    void Update()
    {
        this.Move();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        this.Collision(other);
    }

    //Initializes the obstacle
    protected virtual void Init()
    {
        //print("obstacle start");
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();
        playerBody = player.GetComponent<Rigidbody2D>();
    }

    //Handles the basic collisions
    public virtual void Collision(Collider2D other)
    {
        //print("Obstacle collision!");
        if (other.gameObject == player && playerScript.moveState != MoveState.dead)
        {
            playerScript.moveState = MoveState.dead;
            playerBody.velocity = Vector2.zero;
            playerBody.gravityScale = 3f;
			playerBody.AddForce( Vector2.up * 900f);
			playerBody.angularVelocity = 700f;
            this.CollisionResponse();
        }
    }
    
    //Determines what happens to the Obstacle after it is collided with.
    protected virtual void CollisionResponse()
    {

    }

    //Moves the obstacle
    protected virtual void Move()
    {

    }
}
