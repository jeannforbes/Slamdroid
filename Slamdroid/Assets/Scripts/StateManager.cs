using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {
    public enum GameState
    {
        MainMenu,Play,Pause,Instructions,Upgrade
    }

    private GameObject player;
    private Player playerScript;

    private GameState state;
    private Rect menuRect;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();

        changeState(GameState.MainMenu);
        menuRect = new Rect(0.0f, 0.0f, 150f, 100f);
    }
	
	// Update is called once per frame
	void Update () {
        switch (state)
        {
            case GameState.MainMenu:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    changeState(GameState.Play);
                }
                break;
            case GameState.Play:
                if (Input.GetKeyDown(KeyCode.P) && playerScript.PlayState != Player.PlayerState.Running)
                {
                    changeState(GameState.Pause);
                }
                break;
            case GameState.Pause:
                if (Input.GetKeyDown(KeyCode.P))
                {
                    changeState(GameState.Play);
                }
                break;
        }
    }

    void OnGUI()
    {
        switch (state)
        {
            case GameState.MainMenu:
                GUI.TextArea(menuRect, "Slamdroid\n\n(Press space to play)");
                break;
            case GameState.Pause:
                GUI.TextArea(menuRect, "The game is paused.\n(Press P to unpause)");
                break;
        }
    }

    public void changeState(GameState newState)
    {
        if (newState == GameState.Play)
        {
            playerScript.enabled = true;
            player.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            playerScript.enabled = false;
            if (newState != GameState.Pause)
            {
                player.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        state = newState;
    }
}
