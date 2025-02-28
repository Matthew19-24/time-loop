using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public FixedJoystick joystick;
    public float moveSpeed;
    float hInput, vInput;

    int score = 0;
    public GameObject winText;

    public static int winScore = 5; // Make winScore static to persist across scene reloads

    public float timerDuration; // Duration of the timer in seconds
    private float timer;
    public TextMeshPro timerText; // Assign in Inspector

    private Vector3 initialPosition;
    private Vector2[] initialGadgetPositions;
    private int[] initialGadgetValues;
    public GameObject gadgetPrefab; // Assign in Inspector

    public TextMeshPro scoreText; // Assign in Inspector
    public TextMeshPro goalText; // Assign in Inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position;
        timer = timerDuration;
        Debug.Log("Win score set to: " + winScore);

        if (timerText == null)
        {
            Debug.LogError("Timer TextMeshPro component not assigned.");
        }
        else
        {
            UpdateTimerText();
        }

        if (scoreText == null)
        {
            Debug.LogError("Score TextMeshPro component not assigned.");
        }
        else
        {
            UpdateScoreText();
        }

        if (goalText == null)
        {
            Debug.LogError("Goal TextMeshPro component not assigned.");
        }
        else
        {
            UpdateGoalText();
        }

        // Delay the initialization of initial gadget positions and values
        Invoke("InitializeGadgetPositionsAndValues", 0.1f);

        // Add click listener to winText
        if (winText != null)
        {
            winText.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(RestartGame);
        }
    }

    void InitializeGadgetPositionsAndValues()
    {
        // Store initial gadget positions and values
        GadgetSpawner gadgetSpawner = FindObjectOfType<GadgetSpawner>();
        if (gadgetSpawner != null)
        {
            initialGadgetPositions = new Vector2[gadgetSpawner.gadgetCount];
            initialGadgetValues = new int[gadgetSpawner.gadgetCount];
            GameObject[] gadgets = GameObject.FindGameObjectsWithTag("Gadget");
            Debug.Log("Gadgets found: " + gadgets.Length);
            for (int i = 0; i < gadgetSpawner.gadgetCount; i++)
            {
                initialGadgetPositions[i] = gadgets[i].transform.position;
                initialGadgetValues[i] = gadgets[i].GetComponent<Gadget>().gadgetValue;
            }
            Debug.Log("Initial gadget positions: " + initialGadgetPositions.Length);
        }
        else
        {
            Debug.LogError("GadgetSpawner script not found.");
        }
    }

    // Update is called once per frame
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

    void FixedUpdate()
    {
        hInput = joystick.Horizontal * moveSpeed;
        vInput = joystick.Vertical * moveSpeed;       
 
        transform.Translate(hInput,vInput, 0);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Gadget")
        {
            score += collision.gameObject.GetComponent<Gadget>().gadgetValue;
            UpdateScoreText();

            Destroy(collision.gameObject);

            if (score == winScore)
            {
                winText.SetActive(true);
                // Stop the timer but keep the current time displayed
                timer = Mathf.Ceil(timer);
                UpdateTimerText();
                enabled = false; // Disable further updates
            }
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.Ceil(timer).ToString();
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Gadget # " + score.ToString();
        }
    }

    void UpdateGoalText()
    {
        if (goalText != null)
        {
            goalText.text = "Goal Gadget # " + winScore.ToString();
        }
    }

    void ResetGame()
    {
        Debug.Log("Resetting game...");

        // Reset player position
        transform.position = initialPosition;
        Debug.Log("Player position reset to: " + initialPosition);

        // Reset score
        score = 0;
        UpdateScoreText();
        Debug.Log("Score reset to: " + score);

        // Reset timer
        timer = timerDuration;
        Debug.Log("Timer reset to: " + timerDuration);

        // Destroy existing gadgets
        GameObject[] gadgets = GameObject.FindGameObjectsWithTag("Gadget");
        foreach (GameObject gadget in gadgets)
        {
            Destroy(gadget);
        }
        Debug.Log("Existing gadgets destroyed.");
        Debug.Log("Initial gadget positions: " + initialGadgetPositions.Length);

        // Respawn gadgets at initial positions with initial values
        for (int i = 0; i < initialGadgetPositions.Length; i++)
        {
            Debug.Log("Respawn gadgets loop starting.");

            if (gadgetPrefab != null)
            {
                GameObject newGadget = Instantiate(gadgetPrefab, initialGadgetPositions[i], Quaternion.identity);
                newGadget.SetActive(true);
                Debug.Log("Instantiated new gadget at position: " + initialGadgetPositions[i]);

                Gadget gadgetComponent = newGadget.GetComponent<Gadget>();
                if (gadgetComponent != null)
                {
                    gadgetComponent.spawnLocation = initialGadgetPositions[i];
                    gadgetComponent.gadgetValue = initialGadgetValues[i]; // Set the initial value
                    Debug.Log("Gadget component assigned at position: " + initialGadgetPositions[i] + " with value: " + initialGadgetValues[i]);
                }
                else
                {
                    Debug.LogError("Gadget component not found on instantiated gadget.");
                }
            }
            else
            {
                Debug.LogError("Gadget prefab is not assigned.");
            }
        }

        // Update timer text
        UpdateTimerText();
        Debug.Log("Game reset: Player position, score, timer, and gadgets have been reset.");
    }

    void RestartGame()
    {
        winScore += 1; // Increment winScore by 1
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}
