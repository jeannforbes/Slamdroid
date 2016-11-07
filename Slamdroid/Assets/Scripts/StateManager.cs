using UnityEngine;
using System.Collections;


public delegate void effect();
struct UpgradeChoice
{
    public string name;
    public effect upgradeEffect;
    public int baseCost;
    public int costIncrement;
    public int numUpgrades;
    public int maxUpgrades;
}

public class StateManager : MonoBehaviour {
    public enum GameState
    {
        MainMenu,Play,Pause,Instructions,Upgrade
    }

    private GameObject player;
    private Player playerScript;

    private GameState state;
    private GameState lastState;

    //GUI
    private const int numInstructionLines = 1;
    private const int numUpgrades = 0;
    private const int maxButtons = 3;

    private const float lineHeight = 20f;
    private const float padding = 5f;

    private Rect menuRect;
    private Rect labelRect;
    private Rect[] buttonRects;
    private Rect instructionsRect;
    private Rect instructionButton;
    
    //Upgrades
    private UpgradeChoice[] upgrades = new UpgradeChoice[0];
    private int totalCost = 0;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();

        changeState(GameState.MainMenu);
        menuRect = new Rect(0.0f, 0.0f, 200f, Screen.height);
        labelRect = new Rect(padding, padding, menuRect.width-(2*padding), lineHeight);
        buttonRects = new Rect[maxButtons];

        for(int i = 0; i < buttonRects.Length; i++)
        {
            buttonRects[i] = new Rect(padding, padding+((i+1)*(padding+lineHeight)), menuRect.width - (2 * padding), lineHeight);
        }

        instructionsRect = new Rect(padding, padding, menuRect.width - (2 * padding), numInstructionLines * (padding + lineHeight) - padding);
        instructionButton = new Rect(padding, padding + ((numInstructionLines) * (padding + lineHeight)), menuRect.width - (2 * padding), lineHeight);
    }
	
	// Update is called once per frame
	void Update () {
        switch (state)
        {
           /* case GameState.MainMenu:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    changeState(GameState.Play);
                }
                break;*/
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
        GUI.BeginGroup(menuRect);
        switch (state)
        {
            case GameState.MainMenu:
                GUI.Box(menuRect, "");
                GUI.Label(labelRect, "Slamdroid");
                if(GUI.Button(buttonRects[0],"Play Game")){
                    changeState(GameState.Play);
                } else if (GUI.Button(buttonRects[1], "Instructions"))
                {
                    changeState(GameState.Instructions);
                }
                else if (GUI.Button(buttonRects[2], "Upgrades"))
                {
                    changeState(GameState.Upgrade);
                }
                break;
            case GameState.Pause:
                GUI.Box(menuRect, "");
                GUI.Label(labelRect, "The game is paused...");
                if (GUI.Button(buttonRects[0], "Main Menu"))
                {
                    changeState(GameState.MainMenu);
                } else if (GUI.Button(buttonRects[1], "Instructions"))
                {
                    changeState(GameState.Instructions);
                }
                else if (GUI.Button(buttonRects[2], "Upgrades"))
                {
                    changeState(GameState.Upgrade);
                }
                break;
            case GameState.Instructions:
                GUI.Box(menuRect, "");
                GUI.Label(instructionsRect, "(Instructions go here)");
                if (GUI.Button(instructionButton, "Back"))
                {
                    changeState(lastState);
                }
                break;
            case GameState.Upgrade:
                GUI.Box(menuRect, "");
                GUI.Label(labelRect, "Upgrades:");
                for(int i = 0; i < upgrades.Length; i++)
                {
                    if (upgrades[i].maxUpgrades != 0 && upgrades[i].maxUpgrades < upgrades[i].numUpgrades)
                    {
                        GUI.Label(buttonRects[i], ("(" + upgrades[i].name + ": fully upgraded)"));
                    }
                    else {
                        totalCost = upgrades[i].baseCost + (upgrades[i].numUpgrades * upgrades[i].costIncrement);
                        if (playerScript.Money < totalCost)
                        {

                            GUI.Label(buttonRects[i], ("(" + upgrades[i].name + ": fully upgraded)"));
                        }
                        else
                        {
                            if (GUI.Button(buttonRects[i], upgrades[i].name))
                            {
                                playerScript.Money -= totalCost;
                                upgrades[i].upgradeEffect();
                            }
                        }
                    }
                }
                if (GUI.Button(buttonRects[upgrades.Length], "Back"))
                {
                    changeState(lastState);
                }
                break;
        }
        GUI.EndGroup();
    }

    public void changeState(GameState newState)
    {
        lastState = state;
        if (newState == GameState.Play)
        {
            playerScript.enabled = true;
        }
        else
        {
            playerScript.enabled = false;
        }
        state = newState;
    }
}
