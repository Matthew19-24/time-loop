using TMPro;
using UnityEngine;

public class NumberDisplay : MonoBehaviour
{
    private TextMeshPro textMesh;
    private Gadget gadget; // Reference to the Gadget component

    void Start()
    {
        // Find the TextMeshPro component in child objects
        textMesh = GetComponentInChildren<TextMeshPro>();

        // Find the Gadget component on the same object
        gadget = GetComponent<Gadget>();

        if (textMesh == null)
        {
            Debug.LogError("TextMeshPro component not found in children of " + gameObject.name);
            return;
        }

        if (gadget != null)
        {
            textMesh.text = gadget.gadgetValue.ToString();
        } else
        {
            Debug.LogWarning("No Gadget component found on " + gameObject.name);
        }
    }
}
