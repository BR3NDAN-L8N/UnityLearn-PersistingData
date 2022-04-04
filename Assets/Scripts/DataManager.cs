using System;
using System.IO;
using UnityEngine;

/// <summary>
///     Handle saving data. SaveData(), LoadDate(), Instance.playerName_highScore, Instance.highestScore, Instance.playerName_currPlaying
/// </summary>
public class DataManager : MonoBehaviour
{   // SINGLETON
    public static DataManager Instance; // Instance is the Singleton

    // DATA TO SAVE
    public String playerName_highScore;   // name of player with the highest score
    public int? highestScore = null;             // the high score
    public String playerName_currPlaying;  // name of the player who's currently playing

    private void Awake()
    {
        // this if-statement stops duplicate gameObjects being saved
        // when go from any scene back to the scene that intitialized this
        if (Instance != null)  // if Instance was already set,
        {
            Destroy(gameObject); // destroy this new classes instance
            return; // exit method
        }

        // set this.gameObject
        Instance = this;

        // after starting app, a new folder in Heirarcy
        // will be created and hold the gameObject (this)
        // that was passed in as a param. 
        DontDestroyOnLoad(gameObject);

        // data loaded here, can also be loaded by pressing the Load-button in the menu
        LoadData();
    }

    // Why create another class to save data, as oppose to just saving this DataManager class.
    // Saving just a small class is more efficient.
    // this may also make saving lists, arrays, and dictionaries possible?
    [Serializable]  // this lets the JSON util know to save this shit
    class Data
    {
        public String playerName;
        public int highestScore;
    }

    /// <summary>
    /// Save new score data (with current-players name) to a JSON file.
    /// </summary>
    /// <param name="newHighScore">The high score gained after a the last game round.</param>
    public void SaveData_newHighScore(int newHighScore)
    {
        // ready new score to be saved
        Data data = new Data();
        data.playerName = playerName_currPlaying;
        data.highestScore = newHighScore;

        // update this.Instance
        playerName_highScore = playerName_currPlaying;
        highestScore = newHighScore;

        string json = JsonUtility.ToJson(data);  // convert SaveData instance to JSON

        // save data - by writing to a JSON file
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    /// <summary>
    /// Save the current-players name to persist between scenes.
    /// </summary>
    /// <param name="name">A String received after the user inputs their name into a text field.</param>
    public void SaveData_currentPlayersName(String name)
    {
        Instance.playerName_currPlaying = name;
    }

    /// <summary>
    /// Simply load the data that was last saved to JSON.
    /// </summary>
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json"; 
        if (File.Exists(path))  // if (a file was saved previously)
        {
            string json = File.ReadAllText(path);                  // copy the text to a local var
            Data data = JsonUtility.FromJson<Data>(json); // convert stringified json to Class

            playerName_highScore = data.playerName;    // get prev saved name
            highestScore = data.highestScore;                 // get prev saved score
        }
    }
}
