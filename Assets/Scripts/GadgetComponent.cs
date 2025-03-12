using System;
using TMPro;
using UnityEngine;

public class GadgetComponent : MonoBehaviour
{
    public static int nextId = 0; // Static variable to keep track of the next ID
    public int componentId = -1; // Unique identifier for each GadgetComponent, default to -1
    public int componentValue;
    public Vector2 spawnLocation;
    public SpriteRenderer sprite;
    public TextMeshPro textMesh;
    public TextMeshProUGUI textMeshUI;
    public int originalId;

    void Awake()
    {
            componentId = nextId++; // Assign a unique ID and increment the static variable
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        if(textMesh != null){
            textMesh.text = componentValue.ToString();
        } else if(textMeshUI != null){
            textMeshUI.text = componentValue.ToString();
        }
        
        // Add on collision to respawn the gadget, if no collision add gadget to the list of spawned components
    }

    public static implicit operator GadgetComponent(GameObject v)
    {
        throw new NotImplementedException();
    }


}
