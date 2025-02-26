using UnityEngine;

public class GadgetSpawner : MonoBehaviour
{
    public GameObject gadgetPrefab; // Assign your Carrot prefab in the Inspector
    public int gadgetCount = 5;     // Number of carrots to spawn
    public Vector2 spawnAreaMin;    // Bottom-left corner of spawn area
    public Vector2 spawnAreaMax;    // Top-right corner of spawn area

    void Start()
    {
        SpawnGadgets();

        // Assign the gadgetPrefab to the Player script
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.gadgetPrefab = gadgetPrefab;
            Debug.Log("Gadget prefab assigned to Player.");
        }
        else
        {
            Debug.LogError("Player script not found.");
        }
    }

    void SpawnGadgets()
    {
        for (int i = 0; i < gadgetCount; i++)
        {
            Vector2 randomPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            GameObject newGadget = Instantiate(gadgetPrefab, randomPosition, Quaternion.identity);
            newGadget.SetActive(true);

            Gadget gadgetComponent = newGadget.GetComponent<Gadget>();
            if (gadgetComponent != null)
            {
                gadgetComponent.spawnLocation = randomPosition;
                // Set gadgetValue logic here
                gadgetComponent.gadgetValue = 1;
                Debug.Log("Gadget spawned at position: " + randomPosition);
            }
            else
            {
                Debug.LogError("Gadget component not found on instantiated gadget.");
            }
        }
    }
}
