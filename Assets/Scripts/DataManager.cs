using System;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance; // Instance is the Singleton

    // DATA TO SAVE
    public String playerName;
    public int? highestScore = null;

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
    /// Save data when you have both new player-name and a new highest-score
    /// </summary>
    /// <param name="playerName"></param>
    /// <param name="highestScore"></param>
    public void SaveData(String playerName, int highestScore)
    {
        Data data = new Data(); // create instance of SaveData
        data.playerName = playerName;       // set the player's name to the passed-in variable
        data.highestScore = highestScore;

        string json = JsonUtility.ToJson(data);  // convert SaveData instance to JSON

        // Application.persistentDataPath == where all persistent data is stored by Unity
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);  // Create a JSON file and add the new JSON object
    }

    /// <summary>
    /// Save data when you only have a new player-name. If no high-score has been recorded then it will be set to zero.
    /// </summary>
    /// <param name="playerName">A String received after the user inputs their name into a text field.</param>
    public void SaveData(String playerName)
    {
        Data data = new Data();                  // create instance of SaveData
        data.playerName = playerName;       // set the player's name to the passed-in variable
        
        if (this.highestScore != null)                     // check if a high-score was recorded
        {
            data.highestScore = (int)highestScore; // save the recorded score,
        }
        else                                                       // or
        {
            data.highestScore = 0;                        // set the score to zero
        }

        string json = JsonUtility.ToJson(data);  // convert SaveData instance to JSON

        // Application.persistentDataPath == where all persistent data is stored by Unity
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);  // Create a JSON file and add the new JSON object
    }

    /// <summary>
    /// Save data when you only have a new high-score.
    /// </summary>
    /// <param name="newHighScore">The score from the most recent game played</param>
    public void SaveData(int newHighScore)
    {
        Data data = new Data();                                                                 // create instance of SaveData
        data.playerName = playerName;                                                     // set the player's name to the passed-in variable

        if (this.highestScore != null)                                                             // check if a high-score was recorded
        {
            data.highestScore = Math.Max(newHighScore, (int)highestScore); // determin the highest score that was recorded
        }
        else                                                                                               // or
        {
            data.highestScore = newHighScore;                                             // we'll save the new score
        }

        this.highestScore = data.highestScore;
        string json = JsonUtility.ToJson(data);  // convert SaveData instance to JSON

        // Application.persistentDataPath == where all persistent data is stored by Unity
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);  // Create a JSON file and add the new JSON object
    }

    /// <summary>
    /// Simply load the data.
    /// </summary>
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json"; // grab the data from Unity's persitent stuff
        if (File.Exists(path))  // if a file exists (as in was saved previously)
        {
            string json = File.ReadAllText(path);                  // copy the text to a local var
            Data data = JsonUtility.FromJson<Data>(json); // convert stringified json to Class

            playerName = data.playerName;    // get prev saved name
            highestScore = data.highestScore; // get prev saved score
        }
    }
}
