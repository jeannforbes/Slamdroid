using UnityEngine;
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
        /*MainMenu,*/Play,Pause,Instructions/*,Upgrade*/
    }

    private GameObject player;
    private static PlayerMovement playerScript;

    public GameState state;
    private GameState lastState;

    //GUI
    public GUIStyle menuStyle;
    public GUIStyle titleStyle;
    public GUIStyle textStyle;
    public GUIStyle buttonStyle;

    public GUIStyle leftArrow;
    public GUIStyle rightArrow;
    

    public float lineHeight = 25f;// The height of a single line of text used to calculate line spacing.
    public float menuWidthPercent = 0.35f;// How wide the menu is as a percentage of the screen width
    public float keyImageWidth = 50f;
    public float keyImageHeight = 50f;
    public float popupWidth = 250f;
    public float popupHeight = 100f;
    public float popupHeightOffset = 100f;
    public float padding = 5f;//padding between the menu rect and sub-elements
    private float menuWidth;

    private Rect menuRect;
    private Rect labelRect;
    private Rect[] buttonRects;
    private Rect instructionsRect;
    private Rect instructionButton;
    private Rect popupRect;
    private Rect lArrowRect;
    private Rect rArrowRect;
    private Rect infoRect;
    private Rect cansRect;

    public int numUpgrades = 0; //Number of upgrades available.
    public int maxButtons = 3; // The maximum number of button slots available in the game menus.

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
    private int numInstructionLines = 6;
    private int numInfoLines = 3;
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
    private int numInstructionLines = 8;
    private int numInfoLines = 3;
#else
    private int numInstructionLines = 1;
    private int numInfoLines = 1;
#endif

    private string instructionString;
    private string infoString;

    //Upgrades
    private UpgradeChoice[] upgrades;
    private int totalCost = 0;
    private string upgradeStr;

    //private List<GameObject> worldList;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();

        //Calculates the menu width
        menuWidth = Screen.width * menuWidthPercent;
        //print(menuWidth);

        changeState(GameState.Play);
        menuRect = new Rect(0.0f, 0.0f, menuWidth, Screen.height);
        labelRect = new Rect(padding, padding, menuRect.width-(2*padding), lineHeight);
        buttonRects = new Rect[maxButtons];

        for(int i = 0; i < buttonRects.Length; i++)
        {
            buttonRects[i] = new Rect(padding, padding+((i+1)*(padding+lineHeight)), menuRect.width - (2 * padding), lineHeight);
        }

        instructionsRect = new Rect(padding, padding, menuRect.width - (2 * padding), numInstructionLines * (padding + lineHeight) - padding);
        instructionButton = new Rect(padding, padding + ((numInstructionLines) * (padding + lineHeight)), menuRect.width - (2 * padding), lineHeight);
        
        popupRect = new Rect((Screen.width-popupWidth)/2, (Screen.height-popupHeight)/2-popupHeightOffset, popupWidth, popupHeight);
        lArrowRect = new Rect((Screen.width - (3*keyImageWidth)) / 2 , (Screen.height - popupHeight) / 2 - popupHeightOffset + padding, keyImageWidth, keyImageHeight);
        rArrowRect = new Rect((Screen.width + (keyImageWidth)) / 2 , (Screen.height - popupHeight) / 2 - popupHeightOffset + padding, keyImageWidth, keyImageHeight);
        infoRect = new Rect((Screen.width - popupWidth) / 2 + padding, (Screen.height - popupHeight) / 2 - popupHeightOffset + keyImageHeight + 2 * padding, popupWidth - 2 * padding, numInfoLines * (padding + lineHeight) - padding);

        cansRect = new Rect(padding, Screen.height - (padding + lineHeight), menuRect.width - (2 * padding), lineHeight);

        upgrades = new UpgradeChoice[]{
            //new UpgradeChoice("Increased Boost", new effect(playerScript.increaseBoost), 2, 3, 6),
           // new UpgradeChoice("Slower Boost Bar", new effect(playerScript.decreaseBarSpeed), 3, 4, 2)
        };

        instructionString = "Jump up the shaft and avoid obstacles.\n\n";
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
        instructionString += "Press the A or left keys to jump to the left wall.\nPress the D or right keys to jump to the right wall.\n";
        infoString = "Use A and D or arrow keys to jump.\n\nTip: Jump against the same wall to gain height! \n";
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        instructionString += "Tap the left side of the screen to jump to the left wall.\nTap the right side of the screen to jump to the right wall.\n";
        infoString = "(Not written)";
#endif
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
            /*case GameState.MainMenu:
                GUI.Box(menuRect, "",menuStyle);
                GUI.Label(labelRect, "Slamdroid", titleStyle);
                if(GUI.Button(buttonRects[0],"Play Game",buttonStyle)){
                    changeState(GameState.Play);
                } else if (GUI.Button(buttonRects[1], "Instructions", buttonStyle))
                {
                    changeState(GameState.Instructions);
                }
                else if (GUI.Button(buttonRects[2], "Upgrades", buttonStyle))
                {
                    changeState(GameState.Upgrade);
                }
                break;*/
            case GameState.Pause:
                GUI.Box(menuRect, "", menuStyle);
                GUI.Label(labelRect, "The game is paused...", titleStyle);
               /* if (GUI.Button(buttonRects[0], "Main Menu", buttonStyle))
                {
                    changeState(GameState.MainMenu);
                } else */if (GUI.Button(buttonRects[0], "Instructions", buttonStyle))
                {
                    changeState(GameState.Instructions);
                }
                /*else if (GUI.Button(buttonRects[1], "Upgrades", buttonStyle))
                {
                    changeState(GameState.Upgrade);
                }*/
                break;
            case GameState.Instructions:
                GUI.Box(menuRect, "", menuStyle);
                GUI.Label(instructionsRect, instructionString, textStyle);
                if (GUI.Button(instructionButton, "Back", buttonStyle))
                {
                    changeState(lastState);
                }
                break;
            /*case GameState.Upgrade:
                GUI.Box(menuRect, "", menuStyle);
                GUI.Label(labelRect, "Upgrades:", titleStyle);
                for(int i = 0; i < upgrades.Length; i++)
                {
                    if (upgrades[i].maxUpgrades != 0 && upgrades[i].maxUpgrades <= upgrades[i].numUpgrades)
                    {
                        GUI.Label(buttonRects[i], (upgrades[i].name + " (" + upgrades[i].numUpgrades + "," + upgrades[i].maxUpgrades + ") - N/A"), textStyle);
                    }
                    else {
                        totalCost = upgrades[i].baseCost + (upgrades[i].numUpgrades * upgrades[i].costIncrement);
                        upgradeStr = (upgrades[i].name + " (" + upgrades[i].numUpgrades + "," + upgrades[i].maxUpgrades + ") - " + totalCost);
                        if (playerScript.money < totalCost)
                        {

                            GUI.Label(buttonRects[i], upgradeStr, textStyle);
                        }
                        else
                        {
                            if (GUI.Button(buttonRects[i], upgradeStr,buttonStyle))
                            {
                                playerScript.money -= totalCost;
                                upgrades[i].numUpgrades ++;
                                upgrades[i].upgradeEffect();
                            }
                        }
                    }
                }

                if (GUI.Button(buttonRects[upgrades.Length], "Back", buttonStyle))
                {
                    changeState(lastState);
                }
                break;*/
        }
        GUI.EndGroup();
        if(state == GameState.Play)
        {
            //GUI.TextArea(cansRect, "Creature cans - " + playerScript.money,menuStyle);
            if (playerScript.moveState == MoveState.tutorialPopup)
            {
                //GUI.BeginGroup(popupRect);
                GUI.Box(popupRect, "", menuStyle);
                GUI.Box(lArrowRect, "", leftArrow);
                GUI.Box(rArrowRect, "", rightArrow);
                GUI.TextArea(infoRect, infoString, textStyle);
                //GUI.EndGroup();
            }
        }
        /*else
        {
            GUI.Label(cansRect, "Creature cans:  " + playerScript.money,textStyle);
        }
        */
    }

    public void changeState(GameState newState)
    {
        lastState = state;
        if (newState == GameState.Play)
        {
            //playerScript.transitionFrame = true;
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
