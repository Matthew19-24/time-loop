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
    public static int winScore = 7; // Assign the win score in the Inspector
    public TextMeshProUGUI winText; // Assign the score text in the Inspector
    public static List<GameObject> inventory = new List<GameObject>();
    public AudioSource pickup; // Add this variable for the pickup sound effect
    public GameObject componentPrefab;
    public GameObject collapseButton;


    [System.Obsolete]
    void Start()
    {
        initialPosition = transform.position;
        body = transform.Find("Body");
        if (body == null)
        {
            Debug.LogError("Body transform not found.");
            return;
        }

        idleDown = body.Find("idle_down")?.gameObject;
        idleSide = body.Find("idle_side")?.gameObject;
        idleUp = body.Find("idle_up")?.gameObject;
        runDown = body.Find("run_down")?.gameObject;
        runSide = body.Find("run_side")?.gameObject;
        runUp = body.Find("run_up")?.gameObject;

        if (idleDown == null || idleSide == null || idleUp == null || runDown == null || runSide == null || runUp == null)
        {
            Debug.LogError("One or more animation states are not found.");
            return;
        }

        SetActiveState(idleDown);
        lastActiveState = idleDown;

        // Set the win score text
        winText.text = winScore.ToString();
    }

    void FixedUpdate()
    {
        if (joystick.gameObject.activeInHierarchy)
        {
            hInput = joystick.Horizontal * moveSpeed;
            vInput = joystick.Vertical * moveSpeed;
        }
        else
        {
            hInput = 0;
            vInput = 0;
        }

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
        if (idleDown == null || idleSide == null || idleUp == null || runDown == null || runSide == null || runUp == null)
        {
            Debug.LogError("One or more animation states are not assigned.");
            return;
        }

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

        inventory = new List<GameObject>();

        GameObject[] gadgets = GameObject.FindGameObjectsWithTag("Gadget");
        foreach (GameObject gadget in gadgets)
        {
            Destroy(gadget);
        }

        if (ComponentSpawner.gadgetsInfo != null)
        {
            foreach (GadgetInfo gadgetInfo in ComponentSpawner.gadgetsInfo)
            {
                Debug.Log("Respawn gadgets loop starting.");

                if (componentPrefab != null)
                {
                    GameObject newGadget = Instantiate(componentPrefab, gadgetInfo.spawnLocation, Quaternion.identity);
                    newGadget.SetActive(true);

                    GadgetComponent newGadgetComponent = newGadget.GetComponent<GadgetComponent>();
                    if (newGadgetComponent != null)
                    {
                        newGadgetComponent.spawnLocation = gadgetInfo.spawnLocation;
                        newGadgetComponent.componentValue = gadgetInfo.componentValue; // Set the initial value

                        // Set the sprite of the respawned gadget
                        SpriteRenderer spriteRenderer = newGadgetComponent.GetComponent<SpriteRenderer>();
                        if (spriteRenderer != null)
                        {
                            spriteRenderer.sprite = gadgetInfo.sprite;
                        }
                        else
                        {
                            Debug.LogError("SpriteRenderer not found on instantiated gadget.");
                        }
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
        }
        else
        {
            Debug.LogError("ComponentSpawner.gadgetsInfo is null.");
        }

        // Simulate a click on the collapseButton
        if (collapseButton != null)
        {
            Button button = collapseButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.Invoke();
            }
            else
            {
                Debug.LogError("Button component not found on collapseButton.");
            }
        }
        else
        {
            Debug.LogError("collapseButton is not assigned.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (inventory.Count < 9) {
            if (collision.gameObject.CompareTag("Gadget"))
            {
                // Create a new copy of the collided GameObject
                GameObject newGadget = Instantiate(collision.gameObject);
                newGadget.SetActive(false); // Deactivate the new gadget to keep it in the inventory

                // Add the new copy to the inventory
                inventory.Add(newGadget);
                
                // Destroy the original collided GameObject
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

}