using TMPro;
using UnityEngine;

public class GadgetComponent : MonoBehaviour
{
    public int componentValue;
    public Vector2 spawnLocation;
    public SpriteRenderer sprite;
    public TextMeshPro textMesh;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        textMesh.text = componentValue.ToString();
        
        // Add on collision to respawn the gadget, if no collision add gadget to the list of spawned components
    }
}
