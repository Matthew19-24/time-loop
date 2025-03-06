using System;
using TMPro;
using UnityEngine;

public class GadgetComponent : MonoBehaviour
{
    public int componentValue;
    public Vector2 spawnLocation;
    public SpriteRenderer sprite;
    public TextMeshPro textMesh;
    public TextMeshProUGUI textMeshUI;

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
