using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject spikePrefab;
	public List<GameObject> fallingPrefabs;
	public List<GameObject> fgTiles;
	public List<GameObject> bgTiles;

	private GameObject player;
	private GameObject[] walls;

	public float currentHeight;
	public float currentTileHeight;

	// Use this for initialization
	void Start () {
		currentHeight = 0f;
		currentTileHeight = -10f;
		player = GameObject.FindGameObjectWithTag ("Player");
		walls = GameObject.FindGameObjectsWithTag ("Wall");
	}
	
	// Update is called once per frame
	void Update () {
		MoveWalls ();
		GenerateLevel ();
	}

	void MoveWalls(){
		//Don't move if the player's dead
		if (player.GetComponent<PlayerMovement> ().moveState == MoveState.dead)
			return;
		//Move the walls along with the player's current height
		for (int i=0; i<walls.Length; i++) {
			walls[i].transform.position = new Vector2(walls[i].transform.position.x, player.transform.position.y);
		}
	}

	/// <summary>
	/// Generates the level.
	/// </summary>
	void GenerateLevel(){
		if (player.transform.position.y + 20f > currentTileHeight) {
			currentTileHeight += 5;
			GenerateWallTiles ();
			GenerateBGTiles ();
		}
		if (player.transform.position.y - 20f > currentHeight) {
			GenerateSpike ();

			//Will something fall?
			if ( Random.Range (0,10) < 5 ) SpawnFallingObject();

			//Set the new current height
			currentHeight = player.transform.position.y;
		}
	}
	void GenerateWallTiles(){
		GameObject leftTile, rightTile;
		leftTile = Instantiate (fgTiles [(int)Random.Range (0f, fgTiles.Count)]);
		leftTile.transform.position = new Vector2 (-8f, currentTileHeight);

		rightTile = Instantiate (fgTiles [(int)Random.Range (0f, fgTiles.Count)]);
		rightTile.transform.position = new Vector2 (8f, currentTileHeight);
	}

	void GenerateBGTiles(){
		GameObject bgTile;
		for (int i=0; i<4; i++) {
			bgTile = Instantiate (bgTiles [(int)Random.Range (0f, bgTiles.Count)]);
			bgTile.transform.position = new Vector3 (-8f + i*5, currentTileHeight, 4f);
		}
	}


	/// <summary>
	/// Generates randomly placed spikes as the player is climbing.
	/// </summary>
	void GenerateSpike(){
		GameObject spike;
		spike = Instantiate (spikePrefab);
		spike.transform.position = new Vector2(-5f, player.transform.position.y + 10f + Random.Range (0,5));
		spike = Instantiate (spikePrefab);
		spike.transform.position = new Vector2(5f, player.transform.position.y + 20f + Random.Range (0,5));
	}

	/// <summary>
	/// Spawns a falling object for the player to avoid.
	/// </summary>
	void SpawnFallingObject(){
		int randomNum = (int)Random.Range (0, fallingPrefabs.Count - 1);
		Debug.Log (randomNum);
		GameObject fallingObject = (GameObject)Instantiate (fallingPrefabs[randomNum]);
		fallingObject.transform.position = new Vector2 (0f, player.transform.position.y + 20f);
		fallingObject.GetComponent<Rigidbody2D> ().angularVelocity = 300f;
	}
}
