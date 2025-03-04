using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{

    public FixedJoystick joystick;
    
    public float moveSpeed;
    float hInput, vInput;
    private Vector3 initialPosition;


    void Start()
    {
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        hInput = joystick.Horizontal * moveSpeed;
        vInput = joystick.Vertical * moveSpeed;       
 
        transform.Translate(hInput,vInput, 0);
    }



}