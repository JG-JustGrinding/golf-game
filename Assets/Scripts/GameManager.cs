using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GolfBall golfBall;
    public Transform golfHole;
    public bool won;

    // stroke count
    public int strokes;
    public TextMeshProUGUI strokeText;

    // overlays
    public GameObject winOverlay;
    public GameObject pauseOverlay;

    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        won = false;
        strokes = 0;
        UpdateStrokeText();
    }

    public void Win()
    {
        if (won)
        {
            return;
        }

        won = true;
        StartCoroutine(ShowWinOverlay());
    }

    IEnumerator ShowWinOverlay()
    {
        yield return new WaitForSeconds(1f);
        winOverlay.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseOverlay.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseOverlay.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
        {
            GoToMainMenu();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public static void AddStroke()
    {
        if (!Instance)
        {
            return;
        }

        Instance.strokes++;
        UpdateStrokeText();
    }

    public static void UpdateStrokeText()
    {
        Instance.strokeText.text = "Strokes: " + Instance.strokes;
    }
}
