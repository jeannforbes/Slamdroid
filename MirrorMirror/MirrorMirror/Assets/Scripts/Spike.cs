using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (CircleCollider2D))]
public class Spike : Obstacle {

    public Spike() : base()
    {
        
    }

	// Use this for initialization
	void Start (){
        //print("Spike start");
        this.Init();
	}
	
	// Update is called once per frame
	void Update () {
        this.Move();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        this.Collision(other);
    }

    protected override void CollisionResponse()
    {
        GameObject.Destroy(this.gameObject);
    }

    /*public override void Collision(Collider2D other)
    {
        print("Spike collision!");
        base.Collision(other);
    }*/
}
