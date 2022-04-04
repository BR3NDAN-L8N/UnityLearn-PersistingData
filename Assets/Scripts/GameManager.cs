using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // PROPS - GATHER ON AWAKE
    DataManager DM;

    // PROPS - SERIALIZED
    [SerializeField] private TMP_InputField textInput_playerName;
    [SerializeField] private TMP_Text textDisplay_highScore;

    // PROPS - MANUALLY SET
    private string defaultPlayerName = "Your Name Here...";

    // AWAKE()
    private void Awake()
    {
        DM = GameObject.FindObjectOfType<DataManager>();
    }

    // START()
    private void Start()
    {
        DM.LoadData();
        textInput_playerName.SetTextWithoutNotify(DataManager.Instance.playerName_highScore);
        textDisplay_highScore.text = "High Score: " + DataManager.Instance.highestScore + " : " + DataManager.Instance.playerName_highScore;
    }

    // MENU - START BUTTON
    public void menubutton_start()
    {
        // HANDLE PLAYER NAME
        string playerNameText = textInput_playerName.text;  // get input text
        Debug.Log(playerNameText);                                  // log for reference
        if (playerNameText.Equals(defaultPlayerName)) { // handle player name not set
            playerNameText = "unknown";
        }

        // SET PLAYER NAME IN INSTANCE
        DM.SaveData_currentPlayersName(playerNameText);

        // LOAD SCENE
        SceneNavigator.GoToMainGameScene();
    }
}
