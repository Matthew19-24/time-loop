using UnityEngine;
using System.Collections.Generic;

public class ComponentSpawner : MonoBehaviour
{
    public GameObject componentPrefab; // Assign your Carrot prefab in the Inspector
    public Vector2 spawnAreaMin;    // Bottom-left corner of spawn area
    public Vector2 spawnAreaMax;    // Top-right corner of spawn area
    private int minValue;           // Minimum value for gadget
    private int maxValue;           // Maximum value for gadget
    public static List<GadgetInfo> gadgetsInfo; // List to store gadget info
    public GameObject character;

    public Sprite[] sprites;
    private int componentCount;

    void Start()
    {
        // Generate a random number for gadgetCount based on winScore
        int min = Character.winScore;
        int max = Character.winScore * 2 + 1;

        if (min < 10){
            min = 10;
        }

        if (max > 50) {
            max = 50;
        }

        componentCount = Random.Range(min, max);

        // Set minValue and maxValue based on winScore
        minValue = 1;
        maxValue = Character.winScore + 2;

        // Generate gadget values
        List<int> componentValues = GenerateComponentValues(Character.winScore);
        SpawnComponents(componentValues);
    }

    List<int> GenerateComponentValues(int winScore)
    {
        List<int> values = new List<int>();
        for (int i = 0; i < componentCount - 1; i++)
        {
            int value;
            do
            {
                value = Random.Range(minValue, maxValue + 1);
            } while (value == winScore || value == 0); // Ensure value is not winScore or 0
            values.Add(value);
        }

        // Select a random subset
        int subsetSize = Random.Range(2, componentCount - 2);
        List<int> subset = new List<int>();
        for (int i = 0; i < subsetSize; i++)
        {
            subset.Add(values[Random.Range(0, values.Count)]);
        }

        // Compute the last number
        int subsetSum = 0;
        foreach (int value in subset)
        {
            subsetSum += value;
        }
        int lastValue = winScore - subsetSum;
        if (lastValue < minValue || lastValue > maxValue || lastValue == winScore || lastValue == 0)
        {
            do
            {
                lastValue = Random.Range(minValue, maxValue + 1);
            } while (lastValue == winScore || lastValue == 0); // Ensure lastValue is not winScore or 0
        }
        values.Add(lastValue);

        return values;
    }

    void SpawnComponents(List<int> componentValues)
    {
        gadgetsInfo = new List<GadgetInfo>(); // Initialize the gadgets info list

        for (int i = 0; i < componentCount; i++)
        {
            Vector2 randomPosition;
            
            randomPosition = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            GameObject newComponent = Instantiate(componentPrefab, randomPosition, Quaternion.identity);
            newComponent.SetActive(true);

            GadgetComponent gadgetComponent = newComponent.GetComponent<GadgetComponent>();
            if (gadgetComponent != null)
            {
                gadgetComponent.spawnLocation = randomPosition;
                gadgetComponent.componentValue = componentValues[i];

                // Randomly assign one of the sprites from the sprite array
                SpriteRenderer spriteRenderer = gadgetComponent.GetComponent<SpriteRenderer>();
                Sprite assignedSprite = null;
                if (spriteRenderer != null && sprites.Length > 0)
                {
                    assignedSprite = sprites[Random.Range(0, sprites.Length)];
                    spriteRenderer.sprite = assignedSprite;
                }
                else
                {
                    Debug.LogError("SpriteRenderer not found on instantiated gadget or sprites array is empty.");
                }

                // Check for collisions and respawn if necessary
                CheckAndRespawn(newComponent);

                // Add gadget info to the list
                gadgetsInfo.Add(new GadgetInfo(randomPosition, componentValues[i], assignedSprite));
            }
            else
            {
                Debug.LogError("Gadget component not found on instantiated gadget.");
            }
        }
    }

    void CheckAndRespawn(GameObject component)
    {
        BoxCollider2D collider = component.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            Collider[] colliders = Physics.OverlapBox(collider.bounds.center, collider.bounds.size / 2, Quaternion.identity);
            if (colliders.Length > 1) // More than one collider means it is colliding with something else
            {
                Destroy(component);
                Vector2 newPosition = new Vector2(
                    Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                    Random.Range(spawnAreaMin.y, spawnAreaMax.y)
                );
                GameObject newComponent = Instantiate(componentPrefab, newPosition, Quaternion.identity);
                newComponent.SetActive(true);
                CheckAndRespawn(newComponent); // Recursively check the new component
            }
        }
        else
        {
            Debug.LogError("BoxCollider not found on component.");
        }
    }

    public Sprite RandomSprite()
    {
        return sprites[Random.Range(0, sprites.Length)];
    }

    public static GameObject CreateGadgetComponent(GameObject componentPrefab, int componentValue, Sprite sprites, int componentId)
    {
    // Instantiate the componentPrefab without setting its position
    GameObject newComponent = Instantiate(componentPrefab);
    newComponent.SetActive(false); // Deactivate it to prevent it from appearing in the scene

    GadgetComponent gadgetComponent = newComponent.GetComponent<GadgetComponent>();
    if (gadgetComponent != null)
    {
        gadgetComponent.componentValue = componentValue;
        gadgetComponent.componentId = componentId;

        // Randomly assign one of the sprites from the sprite array
        SpriteRenderer spriteRenderer = gadgetComponent.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites;
    }
    else
    {
        Debug.LogError("Gadget component not found on instantiated gadget.");
    }

    return newComponent;
}
}

public class GadgetInfo
{
    public Vector2 spawnLocation;
    public int componentValue;
    public Sprite sprite;

    public GadgetInfo(Vector2 spawnLocation, int componentValue, Sprite sprite)
    {
        this.spawnLocation = spawnLocation;
        this.componentValue = componentValue;
        this.sprite = sprite;
    }

    

}