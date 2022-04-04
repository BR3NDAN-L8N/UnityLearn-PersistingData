using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    [SerializeField] private Text HighScoreText;
    public Text ScoreText;
    public GameObject GameOverText;
    public GameObject GameResultText;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    DataManager DM;

    private void Awake()
    {
        DM = GameObject.FindObjectOfType<DataManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        HighScoreText.text = "Best Score : " + DataManager.Instance.playerName_highScore + " :" + DataManager.Instance.highestScore;

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point + 100;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;

        // DETERMINE WINNER
        bool isWinner = m_Points > DataManager.Instance.highestScore;

        // UPDATE RESULT TEXT
        if (isWinner) GameResultText.GetComponent<TextMeshProUGUI>().text = "NEW HIGH SCORE! \n" + m_Points;
        else GameResultText.GetComponent<TextMeshProUGUI>().text = "better luck next time!";

        // ACTIVATE GAME OVER TEXT
        GameOverText.SetActive(true);
        GameResultText.SetActive(true);

        // HANDLE SAVING NEW HIGH-SCORE
        if (isWinner) DM.SaveData_newHighScore(m_Points);
    }
}
