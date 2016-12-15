using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    //Assets in the scene
	public GameObject spikePrefab;
	public List<GameObject> fallingPrefabs;
	public List<GameObject> fgTiles;
	public List<GameObject> bgTiles;

	private GameObject player;
	private GameObject[] walls;

    private PlayerMovement playerScript;

    //Height calculations
    public float currentHeight;
    public float updateHeight;
	public float currentTileHeight;
    
    // GUI Fields
    public GUIStyle menuStyle;
    public GUIStyle textStyle;
    public GUIStyle leftArrow;
    public GUIStyle rightArrow;
    public GUIStyle notificationStyle;


    public float lineHeight = 25f;// The height of a single line of text used to calculate line spacing.
    public float keyImageWidth = 50f;
    public float keyImageHeight = 50f;
    public float popupWidth = 250f;
    public float popupHeight = 250f;
    public float popupHeightOffset = 150f;
    public float padding = 5f;//padding between the menu rect and sub-element
    public float guiWidthPercent = 0.2f;// How wide the menu is as a percentage of the screen width
    private float guiWidth;

    private Rect popupRect;
    private Rect lArrowRect;
    private Rect rArrowRect;
    private Rect infoRect;
    private Rect scoreRect;

    private string infoString;

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
    private int numInfoLines = 3;
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
    private int numInfoLines = 3;
#else
    private int numInfoLines = 1;
#endif

    public Vector2 gravityDirection = Vector3.down;

    // Use this for initialization
    void Start () {
		currentHeight = 0f;
		currentTileHeight = -10f;
		player = GameObject.FindGameObjectWithTag ("Player");
		walls = GameObject.FindGameObjectsWithTag ("Wall");

        playerScript = player.GetComponent<PlayerMovement>();
        //GUI rectangles
        guiWidth = Screen.width * guiWidthPercent;

        popupRect = new Rect((Screen.width - popupWidth) / 2, (Screen.height - popupHeight) / 2 - popupHeightOffset, popupWidth, popupHeight);
        lArrowRect = new Rect((Screen.width - (3 * keyImageWidth)) / 2, (Screen.height - popupHeight) / 2 - popupHeightOffset + padding, keyImageWidth, keyImageHeight);
        rArrowRect = new Rect((Screen.width + (keyImageWidth)) / 2, (Screen.height - popupHeight) / 2 - popupHeightOffset + padding, keyImageWidth, keyImageHeight);
        infoRect = new Rect((Screen.width - popupWidth) / 2 + padding, (Screen.height - popupHeight) / 2 - popupHeightOffset + keyImageHeight + 2 * padding, popupWidth - 2 * padding, numInfoLines * (padding + lineHeight) - padding);

        scoreRect = new Rect(padding, padding, guiWidth - (2 * padding), lineHeight*2);

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
        infoString = "Use A and D or arrow keys to jump.\n\nTip: Jump against the same wall to gain height! \n";
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        infoString = "(Not written)";
#endif
        gravityDirection.Normalize();
    }

    // Update is called once per frame
    void Update () {
		MoveWalls ();
		GenerateLevel ();
        UpdateGravity();
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
        //Set the new current height
        currentHeight = player.transform.position.y;

        if (player.transform.position.y + 20f > currentTileHeight) {
			currentTileHeight += 5;
			GenerateWallTiles ();
			GenerateBGTiles ();
		}
		if (currentHeight - 20f > updateHeight) {
			GenerateSpike ();

			//Will something fall?
			if ( Random.Range (0,10) < 5 ) SpawnFallingObject();

			//Set the new current height
			updateHeight = currentHeight;
		}
	}

    /// <summary>
    /// 
    /// </summary>
    void UpdateGravity()
    {
        player.GetComponent<Rigidbody2D>().gravityScale = -1*gravityDirection.y;
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

    //Runs the GUI for the scene.
    void OnGUI()
    {
        GUI.TextArea(scoreRect, "Current Height - " + (currentHeight>0?(int)currentHeight:0) +"\nHighest - " + (int)playerScript.score, menuStyle);
        if (playerScript.moveState == MoveState.tutorialPopup)
        {

            //GUI.BeginGroup(popupRect);
            GUI.Box(popupRect, "", menuStyle);
            GUI.Box(lArrowRect, "", leftArrow);
            GUI.Box(rArrowRect, "", rightArrow);
            GUI.TextArea(infoRect, infoString, textStyle);
            //GUI.EndGroup();
        }

        if(playerScript.moveState == MoveState.dead)
        {
            GUI.Box(popupRect, "Press any key to restart.", notificationStyle);
        }
    }
}
