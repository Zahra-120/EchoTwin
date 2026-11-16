using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Runs the core loop of Echo Twin:
// 1. Each round lasts a fixed time. While it runs, the player's position is
//    recorded every physics step.
// 2. When the timer hits 0, the recording is handed to the Ghost, which
//    replays it during the next round while the player starts a fresh run.
// 3. Plates latch permanently. When ALL plates in the level are latched,
//    the level is complete and the next scene loads.
public class GameManager : MonoBehaviour
{
    [Header("Drag these in from the Hierarchy")]
    public PlayerController player;
    public GhostReplay ghost;
    public TMP_Text timerText;
    public TMP_Text messageText;

    [Header("Settings")]
    public float roundTime = 8f;
    public string nextLevelName = "Level2";

    List<Vector2> recordedPath = new List<Vector2>();
    List<Plate> plates = new List<Plate>();
    Vector2 startPosition;
    float timeLeft;
    int roundNumber = 1;
    bool roundRunning = true;
    bool levelWon = false;

    void Start()
    {
        startPosition = player.transform.position;
        timeLeft = roundTime;
        timerText.text = "Time: " + Mathf.Ceil(roundTime);
        ghost.gameObject.SetActive(false);
        StartCoroutine(ShowMessage("Round 1 - run over the BLUE plate spot (for your echo), then press the YELLOW plates!", 4f));
    }

    void FixedUpdate()
    {
        if (!roundRunning) return;

        // Record where the player is, every physics step.
        recordedPath.Add(player.transform.position);

        timeLeft -= Time.fixedDeltaTime;
        timerText.text = "Time: " + Mathf.Ceil(Mathf.Max(timeLeft, 0f));

        if (timeLeft <= 0f) EndRound();
    }

    void EndRound()
    {
        roundNumber++;

        // 1. Hand this run over to the ghost (a copy, we clear the original).
        ghost.StartReplay(new List<Vector2>(recordedPath));

        // 2. Start a fresh run for the player.
        recordedPath.Clear();
        player.transform.position = startPosition;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        timeLeft = roundTime;

        StartCoroutine(ShowMessage("Round " + roundNumber + " - your ECHO is replaying your last run!", 4f));
    }

    // Plates call this once, when they are created.
    public void RegisterPlate(Plate plate)
    {
        plates.Add(plate);
    }

    // Plates call this every time one latches. Win when all are latched.
    public void PlateLatched()
    {
        foreach (Plate plate in plates)
            if (!plate.IsLatched) return;

        WinLevel();
    }

    void WinLevel()
    {
        if (levelWon) return;
        levelWon = true;
        roundRunning = false;

        ghost.gameObject.SetActive(false);
        player.enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        timerText.text = "";
        messageText.text = "Level complete!";
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(nextLevelName);
    }

    IEnumerator ShowMessage(string text, float seconds)
    {
        messageText.text = text;
        yield return new WaitForSeconds(seconds);
        if (messageText.text == text) messageText.text = "";
    }
}
