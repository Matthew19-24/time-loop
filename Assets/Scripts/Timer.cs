using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timerDuration; // Duration of the timer in seconds
    public GameObject character;
    private float timer;
    public TextMeshProUGUI timerText; // Assign in Inspector

    void Start()
    {
        timer = timerDuration;
    }

    void Update()
    {
        if (timer > 0)
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