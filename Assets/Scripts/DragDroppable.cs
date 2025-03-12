using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragDroppableUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 pointerOffset; // Offset between click point and object position
    private bool isDragging = false;
    private Vector2 originalPosition; // Store the initial position
    private int originalSiblingIndex; // Store the original UI layer order
    private Canvas overrideCanvas; // Temporary canvas for sorting
    private Transform originalParent; // Store the original parent
    private Transform lastParent; // Store the last parent before dragging
    public GameObject timer; // Assign the Timer GameObject in the Inspector

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        originalPosition = rectTransform.anchoredPosition; // Save starting position
        originalParent = rectTransform.parent; // Save starting parent
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        canvasGroup.alpha = 0.7f; // Slight transparency while dragging
        canvasGroup.blocksRaycasts = false; // Prevents blocking other UI elements

        originalSiblingIndex = transform.GetSiblingIndex(); // Save original UI layer order
        transform.SetAsLastSibling(); // Move object to the top layer

        // Save the last parent before dragging
        lastParent = rectTransform.parent;

        // Create a temporary sorting override canvas
        overrideCanvas = gameObject.AddComponent<Canvas>();
        overrideCanvas.overrideSorting = true;
        overrideCanvas.sortingOrder = 9999; // Ensure it's above everything

        // Get the offset between the click point and object's position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pointerOffset);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform, // Parent rect is the reference
            eventData.position,
            eventData.pressEventCamera,
            out localPointerPosition))
        {
            rectTransform.anchoredPosition = localPointerPosition - pointerOffset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        canvasGroup.alpha = 1f; // Restore transparency
        canvasGroup.blocksRaycasts = true; // Re-enable raycasts

        // Check if the object is dropped on a valid target
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = eventData.position;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        bool droppedInValidTarget = false;
        foreach (RaycastResult result in raycastResults)
        {
            if ((originalParent.CompareTag("Slot") && (result.gameObject.CompareTag("Equation") || result.gameObject.CompareTag("KeyHole"))) ||
                (result.gameObject.CompareTag("Slot") || result.gameObject.CompareTag("Equation") || result.gameObject.CompareTag("Sum") || result.gameObject.CompareTag("KeyHole")))
            {
                // Center the object in the slot
                RectTransform slotRectTransform = result.gameObject.GetComponent<RectTransform>();

                // Check if the slot already has a child
                if (slotRectTransform.childCount > 0)
                {
                    // Get the existing child and send it back to the slot the current one came from
                    Transform existingChild = slotRectTransform.GetChild(0);
                    if (existingChild.CompareTag("Gadget"))
                    {
                        existingChild.SetParent(lastParent);
                        existingChild.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    }
                }

                rectTransform.SetParent(slotRectTransform);
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.SetSiblingIndex(slotRectTransform.childCount - 1); // Ensure the gadget is on top
                droppedInValidTarget = true;

                // Check if the dropped slot was a 'KeyHole' slot
                if (result.gameObject.CompareTag("KeyHole"))
                {
                    GadgetComponent gadgetComponent = GetComponent<GadgetComponent>();
                    if (gadgetComponent != null && gadgetComponent.componentValue == Character.winScore)
                    {
                        // Add your win game logic here
                        Debug.Log("Win condition met!");
                        if (timer != null)
                        {
                            Timer timerComponent = timer.GetComponent<Timer>();
                            if (timerComponent != null)
                            {
                                timerComponent.PauseTimer();
                            }
                        }
                        GameObject winObject = FindInactiveObjectByTag("Win");
                        if (winObject != null)
                        {
                            Debug.Log("Activating Win object");
                            winObject.SetActive(true);
                        }
                        else
                        {
                            Debug.LogError("Win object not found");
                        }

                        GameObject joystick = GameObject.FindGameObjectWithTag("Joystick");
                        if (joystick != null)
                        {
                            Debug.Log("Deactivating Joystick object");
                            joystick.SetActive(false);
                        }
                        else
                        {
                            Debug.LogError("Joystick object not found");
                        }
                    }
                }

                break;
            }
        }

        if (!droppedInValidTarget)
        {
            // Reset position to the original position if not dropped in a valid target
            rectTransform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
        }

        // Restore original UI layer order
        transform.SetSiblingIndex(originalSiblingIndex);

        // Remove the temporary sorting override canvas
        if (overrideCanvas != null)
        {
            Destroy(overrideCanvas);
            overrideCanvas = null;
        }
    }

    public void ResetPosition()
    {
        if (rectTransform != null && originalParent != null)
        {
            rectTransform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
        }
    }

    private GameObject FindInactiveObjectByTag(string tag)
    {
        GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in objects)
        {
            if (obj.CompareTag(tag) && !obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }
}
