using UnityEngine;
using System.Collections.Generic;

public class GadgetSpawner : MonoBehaviour
{
    public GameObject gadgetPrefab; // Assign your Carrot prefab in the Inspector
    public int gadgetCount;         // Number of gadgets to spawn
    public Vector2 spawnAreaMin;    // Bottom-left corner of spawn area
    public Vector2 spawnAreaMax;    // Top-right corner of spawn area
    public LayerMask overlapLayerMask; // Layer mask to check for overlaps
    private int minValue;           // Minimum value for gadget
    private int maxValue;           // Maximum value for gadget

    void Start()
    {
        // Assign the gadgetPrefab to the Player script
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.gadgetPrefab = gadgetPrefab;
            Debug.Log("Gadget prefab assigned to Player.");

            // Generate a random number for gadgetCount based on winScore
            gadgetCount = Random.Range(Player.winScore, Player.winScore * 2 + 1);
            Debug.Log("Gadget count set to: " + gadgetCount);

            // Set minValue and maxValue based on winScore
            minValue = 0;
            maxValue = Player.winScore + 2;
            Debug.Log("Min value set to: " + minValue + ", Max value set to: " + maxValue);

            // Generate gadget values
            List<int> gadgetValues = GenerateGadgetValues(Player.winScore);
            SpawnGadgets(gadgetValues);
        }
        else
        {
            Debug.LogError("Player script not found.");
        }
    }

    List<int> GenerateGadgetValues(int winScore)
    {
        List<int> values = new List<int>();
        for (int i = 0; i < gadgetCount - 1; i++)
        {
            int value;
            do
            {
                value = Random.Range(minValue, maxValue + 1);
            } while (value == winScore);
            values.Add(value);
        }

        // Select a random subset
        int subsetSize = Random.Range(2, gadgetCount - 2);
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
        if (lastValue < minValue || lastValue > maxValue || lastValue == winScore)
        {
            lastValue = Random.Range(minValue, maxValue + 1);
        }
        values.Add(lastValue);

        return values;
    }

    void SpawnGadgets(List<int> gadgetValues)
    {
        for (int i = 0; i < gadgetCount; i++)
        {
            Vector2 randomPosition;
            int attempts = 0;
            do
            {
                randomPosition = new Vector2(
                    Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                    Random.Range(spawnAreaMin.y, spawnAreaMax.y)
                );
                attempts++;
            } while (Physics2D.OverlapCircle(randomPosition, 0.5f, overlapLayerMask) && attempts < 100);

            if (attempts >= 100)
            {
                Debug.LogWarning("Could not find a non-overlapping position for gadget after 100 attempts.");
                continue;
            }

            GameObject newGadget = Instantiate(gadgetPrefab, randomPosition, Quaternion.identity);
            newGadget.SetActive(true);

            Gadget gadgetComponent = newGadget.GetComponent<Gadget>();
            if (gadgetComponent != null)
            {
                gadgetComponent.spawnLocation = randomPosition;
                gadgetComponent.gadgetValue = gadgetValues[i];
                Debug.Log("Gadget spawned at position: " + randomPosition + " with value: " + gadgetValues[i]);
            }
            else
            {
                Debug.LogError("Gadget component not found on instantiated gadget.");
            }
        }
    }
}
