using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public AudioSource sound;
    public GameObject[] inventorySlots;

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
        int sum = 0;

        foreach (GameObject equationObject in equationObjects)
        {
            Debug.Log($"Processing equation object: {equationObject.name}");
            foreach (Transform child in equationObject.transform)
            {
                Debug.Log($"Checking child: {child.name}");
                if (child.CompareTag("Gadget"))
                {
                    Debug.Log($"Child {child.name} is a Gadget.");
                    GadgetComponent gadgetComponent = child.GetComponent<GadgetComponent>();
                    if (gadgetComponent != null)
                    {
                        Debug.Log($"Found GadgetComponent with ID: {gadgetComponent.componentId}");
                        // Remove the gadget from the main inventory
                        for (int i = 0; i < Character.inventory.Count; i++)
                        {
                            GadgetComponent originalGadgetComponent = Character.inventory[i].GetComponent<GadgetComponent>();

                            if (originalGadgetComponent != null && originalGadgetComponent.componentId == gadgetComponent.componentId)
                            {
                                Debug.Log($"Removing GadgetComponent with ID: {originalGadgetComponent.componentId} from Character.inventory at index {i}");
                                Character.inventory.RemoveAt(i);
                                break;
                            }
                        }

                        // Add the component value to the sum
                        sum += gadgetComponent.componentValue;
                        Debug.Log($"Added {gadgetComponent.componentValue} to sum. Current sum: {sum}");

                        // Destroy the gadget component
                        Debug.Log($"Destroying gadget component: {child.name}");
                        Destroy(child.gameObject);
                    }
                }
            }
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
                            inventorySlots[i].SetActive(true);
                        }
                    }
                }
            }
        }
    }
}
