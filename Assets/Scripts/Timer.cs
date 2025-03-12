using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timerDuration; // Duration of the timer in seconds
    public GameObject character;
    private float timer;
    public TextMeshProUGUI timerText; // Assign in Inspector
    private bool isPaused = false; // Flag to check if the timer is paused

    void Start()
    {
        timer = timerDuration;
    }

    void Update()
    {
        if (!isPaused && timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                ResetGame();
            }
            UpdateTimerText();
        }
    }

    public void PauseTimer()
    {
        isPaused = true;
    }

    public void ResumeTimer()
    {
        isPaused = false;
    }

    void ResetGame()
    {
        // Reset player position in Character script
        character.GetComponent<Character>().ResetGame();
        timer = timerDuration;
        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.Ceil(timer).ToString();
        }
    }
}