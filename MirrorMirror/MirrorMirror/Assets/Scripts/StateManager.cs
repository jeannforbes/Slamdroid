﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void effect();
struct UpgradeChoice
{
    public UpgradeChoice(string name, effect upgradeEffect,int baseCost,int costIncrement,int maxUpgrades)
    {
        this.name = name;
        this.upgradeEffect = upgradeEffect;
        this.baseCost = baseCost;
        this.costIncrement = costIncrement;
        this.numUpgrades = 0;
        this.maxUpgrades = maxUpgrades;
    }

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
    private static PlayerMovement playerScript;

    private GameState state;
    private GameState lastState;

    //GUI
    private const int numInstructionLines = 3;
    private const int numUpgrades = 0;
    private const int maxButtons = 3;

    private const float lineHeight = 20f;
    private const float menuWidth = 300f;
    private const float padding = 5f;

    private Rect menuRect;
    private Rect labelRect;
    private Rect[] buttonRects;
    private Rect instructionsRect;
    private Rect instructionButton;
    private Rect cansRect;

    private string instructionString;

    //Upgrades
    private UpgradeChoice[] upgrades;
    private int totalCost = 0;
    private string upgradeStr;

    //private List<GameObject> worldList;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();

        changeState(GameState.MainMenu);
        menuRect = new Rect(0.0f, 0.0f, menuWidth, Screen.height);
        labelRect = new Rect(padding, padding, menuRect.width-(2*padding), lineHeight);
        buttonRects = new Rect[maxButtons];

        for(int i = 0; i < buttonRects.Length; i++)
        {
            buttonRects[i] = new Rect(padding, padding+((i+1)*(padding+lineHeight)), menuRect.width - (2 * padding), lineHeight);
        }

        instructionsRect = new Rect(padding, padding, menuRect.width - (2 * padding), numInstructionLines * (padding + lineHeight) - padding);
        instructionButton = new Rect(padding, padding + ((numInstructionLines) * (padding + lineHeight)), menuRect.width - (2 * padding), lineHeight);

        cansRect = new Rect(padding, Screen.height - (padding + lineHeight), menuRect.width - (2 * padding), lineHeight);

        upgrades = new UpgradeChoice[]{
            //new UpgradeChoice("Increased Boost", new effect(playerScript.increaseBoost), 2, 3, 6),
           // new UpgradeChoice("Slower Boost Bar", new effect(playerScript.decreaseBarSpeed), 3, 4, 2)
        };

        instructionString = "Jump up the shaft and avoid obstacles.\n\n";

        instructionString += "Press the left arrow to jump to the left wall.\nPress the right arrow to jump to the right wall.\n";
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
                if (Input.GetKeyDown(KeyCode.P) && playerScript.moveState == MoveState.start)
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
                GUI.Label(instructionsRect, instructionString);
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
                    if (upgrades[i].maxUpgrades != 0 && upgrades[i].maxUpgrades <= upgrades[i].numUpgrades)
                    {
                        GUI.Label(buttonRects[i], (upgrades[i].name + " (" + upgrades[i].numUpgrades + "," + upgrades[i].maxUpgrades + ") - N/A"));
                    }
                    else {
                        totalCost = upgrades[i].baseCost + (upgrades[i].numUpgrades * upgrades[i].costIncrement);
                        upgradeStr = (upgrades[i].name + " (" + upgrades[i].numUpgrades + "," + upgrades[i].maxUpgrades + ") - " + totalCost);
                        if (playerScript.money < totalCost)
                        {

                            GUI.Label(buttonRects[i], upgradeStr);
                        }
                        else
                        {
                            if (GUI.Button(buttonRects[i], upgradeStr))
                            {
                                playerScript.money -= totalCost;
                                upgrades[i].numUpgrades ++;
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

        if(state == GameState.Play)
        {
            GUI.TextArea(cansRect, "Creature cans - " + playerScript.money);
        }else
        {
            GUI.Label(cansRect, "Creature cans:  " + playerScript.money);
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

    //Resets level for a new run.
    public void worldReset()
    {
        //clearing obstacles happens here
    }
}
