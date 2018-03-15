using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInstructions : MonoBehaviour {
    public GameObject redPlayerObj;
    public GameObject bluePlayerObj;

    public GameObject redPlayerInstructions;
    public GameObject bluePlayerInstructions;

    private Player redPlayer;
    private Player bluePlayer;

    private Punch redPlayerPunch;
    private Punch bluePlayerPunch;

    private Text redPlayerText;
    private Text bluePlayerText;

    private string RED_THROW_AIRPLANE_MSG = "Press SPACE twice to throw the airplane!";
    private string BLUE_THROW_AIRPLANE_MSG = "Press kENTER twice to throw the airplane!";

    private string RED_KICK_PLAYER_MSG = "Press SPACE to kick player!";
    private string BLUE_KICK_PLAYER_MSG = "Press kENTER to kick player!";

    private string RED_KICK_AIRPLANE_MSG = "Press SPACE to kick opponent's airplane!";
    private string BLUE_KICK_AIRPLANE_MSG = "Press kENTER to kick opponent's airplane!";

    private string WAIT_COOLDOWN = "Abilities on cooldown... Wait ";

	// Use this for initialization
	void Start () {
        redPlayer = redPlayerObj.GetComponent<Player>();
        redPlayerPunch = redPlayerObj.GetComponent<Punch>();

        bluePlayer = bluePlayerObj.GetComponent<Player>();
        bluePlayerPunch = bluePlayerObj.GetComponent<Punch>();

        redPlayerText = redPlayerInstructions.GetComponent<Text>();
        bluePlayerText = bluePlayerInstructions.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        if (TrackTargets.gameStart)
        {
            if (!FinishingLine.gameOver)
            {
                this.GetComponent<Image>().enabled = true;

                if (bluePlayer.getHasAirplane())
                {
                    bluePlayerText.text = BLUE_THROW_AIRPLANE_MSG;
                }
                else if (bluePlayerPunch.getCanKickPlayer())
                {
                    bluePlayerText.text = BLUE_KICK_PLAYER_MSG;
                }
                else if (bluePlayerPunch.getCanKickAirplane())
                {
                    bluePlayerText.text = BLUE_KICK_AIRPLANE_MSG;
                }
                else if (bluePlayerPunch.getAbilityActivated())
                {
                    bluePlayerText.text = WAIT_COOLDOWN + (int)bluePlayerPunch.getCooldown() + " seconds";
                }
                else
                {
                    bluePlayerText.text = "";
                }

                if (redPlayer.getHasAirplane())
                {
                    redPlayerText.text = RED_THROW_AIRPLANE_MSG;
                }
                else if (redPlayerPunch.getCanKickPlayer())
                {
                    redPlayerText.text = RED_KICK_PLAYER_MSG;
                }
                else if (redPlayerPunch.getCanKickAirplane())
                {
                    redPlayerText.text = RED_KICK_AIRPLANE_MSG;
                }
                else if (redPlayerPunch.getAbilityActivated())
                {
                    redPlayerText.text = WAIT_COOLDOWN + redPlayerPunch.getCooldown() + " seconds";
                }
                else
                {
                    redPlayerText.text = "";
                }
            }
        }
        else
        {
            bluePlayerText.text = "";
            redPlayerText.text = "";
            this.GetComponent<Image>().enabled = false;
        }
	}
}
