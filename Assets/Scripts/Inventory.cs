using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public AudioSource sound;
    public GameObject[] inventorySlots;
    public GameObject doorTrigger;
    public Sprite[] sprites;
    public GameObject componentPrefab;



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
                            Debug.Log($"Assigned sprite {spriteRenderer.sprite.name} to inventory slot {i}");
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

        // Activate the child object with the tag "Gadget" of the GameObject with the tag "Sum"
        GameObject[] sumObjects = GameObject.FindGameObjectsWithTag("Sum");
        foreach (GameObject sumObject in sumObjects)
        {
            Transform componentTransform = sumObject.transform.Find("Component");
            if (componentTransform != null && componentTransform.CompareTag("Gadget"))
            {
                componentTransform.gameObject.SetActive(false);
                Debug.Log($"Activated Component child {componentTransform.name} with tag Gadget in sum object {sumObject.name}");
            }
            else
            {
                Debug.LogError($"Component child with tag Gadget not found in sum object: {sumObject.name}");
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

        // Deactivate the child object with the tag "Gadget" of the GameObject with the tag "Sum"
        GameObject[] sumObjects = GameObject.FindGameObjectsWithTag("Sum");
        foreach (GameObject sumObject in sumObjects)
        {
            Transform componentTransform = sumObject.transform.Find("Component");
            if (componentTransform != null && componentTransform.CompareTag("Gadget"))
            {
                componentTransform.gameObject.SetActive(false);
                Debug.Log($"Deactivated Component child {componentTransform.name} with tag Gadget in sum object {sumObject.name}");
            }
            else
            {
                Debug.LogError($"Component child with tag Gadget not found in sum object: {sumObject.name}");
            }
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

            // I'm looking here
            Debug.Log($"Sum of equation gadgets: {sum}");
            GameObject[] sumObjects = GameObject.FindGameObjectsWithTag("Sum");
            foreach (GameObject sumObject in sumObjects)
            {
                Debug.Log($"Processing sum object: {sumObject.name}");
                Transform componentTransform = sumObject.transform.Find("Component");
                if (componentTransform != null && componentTransform.CompareTag("Gadget"))
                {
                    Debug.Log($"Found Component child with tag Gadget: {componentTransform.name}");
                    GadgetComponent sumComponent = componentTransform.GetComponent<GadgetComponent>();
                    Sprite assignedSprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];

                    if (sumComponent != null)
                    {
                        sumComponent.componentValue = sum;
                        if (sumComponent.textMeshUI != null)
                        {
                            sumComponent.textMeshUI.text = sum.ToString();
                        }
                        if (sumComponent.sprite != null)
                        {
                            sumComponent.sprite.sprite = assignedSprite;
                        }
                        Image slotImage = sumComponent.GetComponent<Image>();
                        slotImage.sprite = assignedSprite;

                        sumComponent.componentId = GadgetComponent.nextId++;
                        componentTransform.localPosition = Vector3.zero; // Set position to (0, 0)
                        Debug.Log($"Setting Component child {componentTransform.name} active.");
                        componentTransform.gameObject.SetActive(true);

                        GameObject newGadget = ComponentSpawner.CreateGadgetComponent(componentPrefab, sum, assignedSprite, sumComponent.componentId);
                        SpriteRenderer newGadgetSpriteRenderer = newGadget.GetComponent<SpriteRenderer>();
                        if (newGadgetSpriteRenderer != null)
                        {
                            newGadgetSpriteRenderer.sprite = assignedSprite; // Ensure the sprite is assigned
                            Debug.Log($"Assigned sprite {assignedSprite.name} to new gadget");
                        }
                        Character.inventory.Add(newGadget);
                    }
                    else
                    {
                        Debug.LogError($"GadgetComponent not found on Component child: {componentTransform.name}");
                    }
                }
                else
                {
                    Debug.LogError($"Component child with tag Gadget not found in sum object: {sumObject.name}");
                }
                Debug.Log($"Setting sum object {sumObject.name} active.");
                sumObject.SetActive(true);
            }
        }
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
                            slotImage.sprite = spriteRenderer.sprite; // Ensure the sprite is assigned
                            Debug.Log($"Assigned sprite {spriteRenderer.sprite.name} to inventory slot {i}");
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
