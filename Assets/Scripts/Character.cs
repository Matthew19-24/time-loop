using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Add this for Image
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic; // Add this for IEnumerator

public class Character : MonoBehaviour
{

    public FixedJoystick joystick;
    
    public float moveSpeed;
    float hInput, vInput;
    private Vector3 initialPosition;
    private Transform body;
    private GameObject idleDown, idleSide, idleUp, runDown, runSide, runUp;
    private GameObject lastActiveState;
    public AudioSource runningSound; // Variable for the running sound effect
    public AudioSource resetSound; 
    public GameObject music;
    public Image fadeImage; // Assign a UI Image in the Inspector for fading
    public GameObject building; // Assign the Building object in the Inspector
    public int winScore; // Assign the win score in the Inspector
    public TextMeshProUGUI winText; // Assign the score text in the Inspector
    private List<GameObject> inventory = new List<GameObject>();
    public AudioSource pickup; // Add this variable for the pickup sound effect

    void Start()
    {
        initialPosition = transform.position;
        body = transform.Find("Body");
        idleDown = body.Find("idle_down").gameObject;
        idleSide = body.Find("idle_side").gameObject;
        idleUp = body.Find("idle_up").gameObject;
        runDown = body.Find("run_down").gameObject;
        runSide = body.Find("run_side").gameObject;
        runUp = body.Find("run_up").gameObject;
        SetActiveState(idleDown);
        lastActiveState = idleDown;

        // Set the win score text
        winText.text = winScore.ToString();
    }

    void FixedUpdate()
    {
        hInput = joystick.Horizontal * moveSpeed;
        vInput = joystick.Vertical * moveSpeed;       

        transform.Translate(hInput, vInput, 0);

        UpdateAnimationState();
    }

    void UpdateAnimationState()
    {
        int soundPref = PlayerPrefs.GetInt("Sound", 1); // Default to 1 (true) if not set

        if (hInput == 0 && vInput == 0)
        {
            if (lastActiveState == runSide)
            {
                SetActiveState(idleSide);
            }
            else if (lastActiveState == runUp)
            {
                SetActiveState(idleUp);
            }
            else if (lastActiveState == runDown)
            {
                SetActiveState(idleDown);
            }
            runningSound.Stop();
        }
        else
        {
            if (Mathf.Abs(hInput) > Mathf.Abs(vInput))
            {
                SetActiveState(runSide);
                transform.localScale = new Vector3(-Mathf.Sign(hInput), 1, 1); // Reverse mirroring for right direction
            }
            else if (vInput > 0)
            {
                SetActiveState(runUp);
            }
            else
            {
                SetActiveState(runDown);
            }
            if (soundPref == 1 && !runningSound.isPlaying)
            {
                runningSound.Play();
            }
        }
    }

    void SetActiveState(GameObject activeObject)
    {
        idleDown.SetActive(activeObject == idleDown);
        idleSide.SetActive(activeObject == idleSide);
        idleUp.SetActive(activeObject == idleUp);
        runDown.SetActive(activeObject == runDown);
        runSide.SetActive(activeObject == runSide);
        runUp.SetActive(activeObject == runUp);
        lastActiveState = activeObject;
    }


    public void ResetGame()
    {        
        resetSound.Play();
        Debug.Log("ResetGame called");
        transform.position = initialPosition;
        // Add other logic here for Reset game
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Gadget")
        {
            inventory.Add(collision.gameObject);
            
            Destroy(collision.gameObject);

            // Play the pickup sound if Music is set to true
            int musicPref = PlayerPrefs.GetInt("Sound", 1); // Default to 1 (true) if not set
            if (musicPref == 1 && pickup != null)
            {
                pickup.Play();
            }
        }
    }

}