using UnityEngine;

public class Player : MonoBehaviour
{

    public FixedJoystick joystick;
    public float moveSpeed;
    float hInput, vInput;

    int score = 0;
    public GameObject winText;

    public int winScore;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        hInput = joystick.Horizontal * moveSpeed;
        vInput = joystick.Vertical * moveSpeed;       
 
        transform.Translate(hInput,vInput, 0);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Gadget")
        {
            score += collision.gameObject.GetComponent<Gadget>().gadgetValue;

            Destroy(collision.gameObject);

            if(score == winScore)
            {
                winText.SetActive(true);
            }

        }


    }
}
