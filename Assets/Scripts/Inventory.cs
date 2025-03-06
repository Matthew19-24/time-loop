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
        for(int x = 0; x < inventorySlots.Length; x++)
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
}
