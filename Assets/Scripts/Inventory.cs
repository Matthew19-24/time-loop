using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public AudioSource sound;
    public GameObject[] inventorySlots;
    public GameObject doorTrigger;

    void Start()
    {
        // Ensure each inventory slot has the required components
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] != null)
            {
                if (inventorySlots[i].GetComponent<GadgetComponent>() == null)
                {
                    Debug.LogError($"GadgetComponent not found on inventory slot {i}.");
                }
                if (inventorySlots[i].GetComponent<Image>() == null)
                {
                    Debug.LogError($"Image component not found on inventory slot {i}.");
                }
            }
            else
            {
                Debug.LogError($"Inventory slot {i} is null.");
            }
        }
    }

    public void OpenInventory()
    {
        for (int x = 0; x < inventorySlots.Length; x++)
        {
            inventorySlots[x].SetActive(false);
        }

        for (int i = 0; i < Character.inventory.Count; i++)
        {
            GameObject inventoryItem = Character.inventory[i];
            if (inventoryItem != null)
            {
                Debug.Log($"Character count {i} is not null");
                GadgetComponent gadgetComponent = inventoryItem.GetComponent<GadgetComponent>();
                SpriteRenderer spriteRenderer = inventoryItem.GetComponent<SpriteRenderer>();

                if (gadgetComponent != null && spriteRenderer != null)
                {
                    if (inventorySlots[i] != null)
                    {
                        GadgetComponent slotGadgetComponent = inventorySlots[i].GetComponent<GadgetComponent>();
                        Image slotImage = inventorySlots[i].GetComponent<Image>();

                        if (slotGadgetComponent != null && slotImage != null)
                        {
                            slotGadgetComponent.sprite = gadgetComponent.sprite;
                            slotImage.sprite = spriteRenderer.sprite;
                            slotGadgetComponent.componentValue = gadgetComponent.componentValue;
                            slotGadgetComponent.textMeshUI.text = gadgetComponent.componentValue.ToString();
                            slotGadgetComponent.spawnLocation = gadgetComponent.spawnLocation;
                            inventorySlots[i].SetActive(true);
                            slotGadgetComponent.componentId = gadgetComponent.componentId;
                        }
                        else
                        {
                            Debug.LogError($"GadgetComponent or Image component not found on inventory slot {i}.");
                        }
                    }
                    else
                    {
                        Debug.LogError($"Inventory slot {i} is null.");
                    }
                }
                else
                {
                    Debug.LogError("GadgetComponent or SpriteRenderer not found on inventory item.");
                }
            }
            else
            {
                // make it invisible
                inventorySlots[i].SetActive(false);
                Debug.LogError("Inventory item is null.");
            }
        }

        int musicPref = PlayerPrefs.GetInt("Sound", 1); // Default to 1 (true) if not set
        if (musicPref == 1 && sound != null)
        {
            sound.Play();
        }
    }

    public void CloseInventory()
    {
        int musicPref = PlayerPrefs.GetInt("Sound", 1); // Default to 1 (true) if not set
        if (musicPref == 1 && sound != null)
        {
            sound.Play();
        }
    }

    public void Plusinator()
    {
        Debug.Log("Plusinator method started.");
        GameObject[] equationObjects = GameObject.FindGameObjectsWithTag("Equation");
        Debug.Log($"Found {equationObjects.Length} equation objects.");
        List<GadgetComponent> activeGadgets = new List<GadgetComponent>();
        int sum = 0;

        foreach (GameObject equationObject in equationObjects)
        {
            Debug.Log($"Processing equation object: {equationObject.name}");

            foreach (Transform child in equationObject.transform)
            {
                Debug.Log($"Checking child: {child.name}");
                if (child.CompareTag("Gadget") && child.gameObject.activeSelf)
                {
                    Debug.Log($"Child {child.name} is an active Gadget.");
                    GadgetComponent gadgetComponent = child.GetComponent<GadgetComponent>();
                    if (gadgetComponent != null)
                    {
                        activeGadgets.Add(gadgetComponent);
                    }
                }
            }
        }

        Debug.Log($"Active gadgets count: {activeGadgets.Count}");

        if (activeGadgets.Count == 2)
        {
            List<int> indicesToRemove = new List<int>();
            HashSet<int> uniqueComponentIds = new HashSet<int>();

            foreach (GadgetComponent gadgetComponent in activeGadgets)
            {
                Debug.Log($"Found GadgetComponent with ID: {gadgetComponent.componentId}");
                // Collect indices to remove
                for (int i = Character.inventory.Count - 1; i >= 0; i--)
                {
                    GadgetComponent originalGadgetComponent = Character.inventory[i].GetComponent<GadgetComponent>();
                    Debug.Log($"Original gadget check: {originalGadgetComponent.componentId} vs {gadgetComponent.componentId}");

                    if (originalGadgetComponent != null && originalGadgetComponent.componentId == gadgetComponent.componentId)
                    {
                        if (!uniqueComponentIds.Contains(originalGadgetComponent.componentId))
                        {
                            indicesToRemove.Add(i);
                            uniqueComponentIds.Add(originalGadgetComponent.componentId);
                        }
                        break;
                    }
                }

                // Add the component value to the sum
                sum += gadgetComponent.componentValue;
                Debug.Log($"Added {gadgetComponent.componentValue} to sum. Current sum: {sum}");
            }

            // Remove collected indices
            foreach (int index in indicesToRemove.OrderByDescending(i => i))
            {
                Debug.Log($"Removing GadgetComponent from Character.inventory at index {index}");
                Character.inventory.RemoveAt(index);
            }

            // Close and reopen the door UI to refresh the inventory display
            DoorTrigger trigger = doorTrigger.GetComponent<DoorTrigger>();
            trigger.CloseDoorUI();
            trigger.OpenDoorUI();
        }

        Debug.Log($"Sum of equation gadgets: {sum}");

        // Update the inventory display
        UpdateInventoryDisplay();
    }

    private void UpdateInventoryDisplay()
    {
        for (int x = 0; x < inventorySlots.Length; x++)
        {
            inventorySlots[x].SetActive(false);
        }

        for (int i = 0; i < Character.inventory.Count; i++)
        {
            GameObject inventoryItem = Character.inventory[i];
            if (inventoryItem != null)
            {
                GadgetComponent gadgetComponent = inventoryItem.GetComponent<GadgetComponent>();
                SpriteRenderer spriteRenderer = inventoryItem.GetComponent<SpriteRenderer>();

                if (gadgetComponent != null && spriteRenderer != null)
                {
                    if (inventorySlots[i] != null)
                    {
                        GadgetComponent slotGadgetComponent = inventorySlots[i].GetComponent<GadgetComponent>();
                        Image slotImage = inventorySlots[i].GetComponent<Image>();

                        if (slotGadgetComponent != null && slotImage != null)
                        {
                            slotGadgetComponent.sprite = gadgetComponent.sprite;
                            slotImage.sprite = spriteRenderer.sprite;
                            slotGadgetComponent.componentValue = gadgetComponent.componentValue;
                            slotGadgetComponent.textMeshUI.text = gadgetComponent.componentValue.ToString();
                            slotGadgetComponent.spawnLocation = gadgetComponent.spawnLocation;
                            slotGadgetComponent.componentId = gadgetComponent.componentId;
                            inventorySlots[i].SetActive(true);
                        }
                    }
                }
            }
        }
    }
}
