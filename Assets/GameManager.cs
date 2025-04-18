using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject startPanel;        // Start panel
    public GameObject gameOverPanel;     // Game Over panel
    public GameObject pausePanel;        // Pause panel
    public Text scoreText;              // Score text during the game
    public Text finalScoreText;         // Final score text on Game Over

    [Header("Gameplay Elements")]
    public GameObject player;           // Reference to the player object
    public GameObject spawner;          // Reference to the spawner
    public GameManager audioManager;   // Reference to AudioManager

    private float survivalTime = 0f;    // Time the player survives
    private bool isGameActive = false;  // Tracks if the game is active
    private static bool isFirstLoad = true;  // Track if it's the first game load

    [DllImport("__Internal")]
    private static extern void SendScore(int score, int game);

    private void Awake()
    {
        // Don't destroy this GameManager on scene reload
        if (audioManager != null)
        {
            DontDestroyOnLoad(audioManager.gameObject);
        }
    }

    private void Start()
    {
        // Check if it's the first load or a restart
        if (isFirstLoad)
        {
            startPanel.SetActive(true);
            gameOverPanel.SetActive(false);
            pausePanel.SetActive(false);
            scoreText.gameObject.SetActive(false);
            player.SetActive(false);
            spawner.SetActive(false);
        }
        else
        {
            // Skip start panel on restart
            StartGame();
        }
    }

    private void Update()
    {
        // Update the score when game is active
        if (isGameActive)
        {
            survivalTime += Time.deltaTime;
            scoreText.text = "Score: " + Mathf.FloorToInt(survivalTime).ToString();
        }

        // Toggle pause with Escape key
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive)
        {
            if (Time.timeScale == 0f)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        player.SetActive(true);
        spawner.SetActive(true);
        scoreText.gameObject.SetActive(true);

        survivalTime = 0f;
        isGameActive = true;
        Time.timeScale = 1f;  // Ensure game is running
        isFirstLoad = false;  // Mark that we've started at least once
    }

    public void PauseGame()
    {
        isGameActive = false;
        Time.timeScale = 0f;  // Pause the game
        pausePanel.SetActive(true);
        scoreText.gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        isGameActive = true;
        Time.timeScale = 1f;  // Resume the game
        pausePanel.SetActive(false);
        scoreText.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverPanel.SetActive(true);
        scoreText.gameObject.SetActive(false);
        finalScoreText.text = "Score: " + Mathf.FloorToInt(survivalTime).ToString();
        player.SetActive(false);
        spawner.SetActive(false);
        SendScore(Mathf.FloorToInt(survivalTime), 110);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}