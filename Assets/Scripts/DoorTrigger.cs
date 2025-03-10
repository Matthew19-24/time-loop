using UnityEngine;
using System.Collections.Generic;

public class DoorTrigger : MonoBehaviour
{
    public GameObject doorUI; // Assign the doorUI GameObject in the Inspector
    public GameObject expandButton; // Assign the expandButton GameObject in the Inspector
    private Inventory inventory;
    private Dictionary<Transform, Vector3> originalPositions;
    private Dictionary<Transform, Transform> originalParents;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Inventory component not found on the same GameObject.");
        }

        // Save the original positions and parents of all children recursively
        originalPositions = new Dictionary<Transform, Vector3>();
        originalParents = new Dictionary<Transform, Transform>();
        SaveOriginalStates(doorUI.transform);
    }

    private void SaveOriginalStates(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (!originalPositions.ContainsKey(child))
            {
                originalPositions[child] = child.localPosition;
                originalParents[child] = child.parent;
            }
            SaveOriginalStates(child);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // When the player steps on the pad
        {
            Debug.Log("Player stepped on the trigger!");
            OpenDoorUI(); 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // When the player steps off the pad
        {
            Debug.Log("Player stepped off the trigger!");
            CloseDoorUI(); 
        }
    }

    public void OpenDoorUI()
    {
        doorUI.SetActive(true); // Show the doorUI
        expandButton.SetActive(false); // Hide the expandButton
        if (inventory != null)
        {
            inventory.OpenInventory();
        }
    }

    public void CloseDoorUI()
    {
        doorUI.SetActive(false); // Hide the doorUI
        expandButton.SetActive(true); // Show the expandButton

        // Restore the original positions and parents of all children recursively
        RestoreOriginalStates(doorUI.transform);

        // Reset positions of all DragDroppable components
        ResetDragDroppablePositions(doorUI.transform);

        if (inventory != null)
        {
            inventory.CloseInventory();
        }
    }

    public void RestoreOriginalStates(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (originalPositions.ContainsKey(child))
            {
                child.localPosition = originalPositions[child];
                child.SetParent(originalParents[child]);
            }
            RestoreOriginalStates(child);
        }
    }

    private void ResetDragDroppablePositions(Transform parent)
    {
        foreach (Transform child in parent)
        {
            DragDroppableUI dragDroppable = child.GetComponent<DragDroppableUI>();
            if (dragDroppable != null)
            {
                if (dragDroppable.gameObject != null)
                {
                    dragDroppable.ResetPosition();
                }
            }
            ResetDragDroppablePositions(child);
        }
    }
}
