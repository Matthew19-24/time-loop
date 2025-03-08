using UnityEngine;

public class RandomDoor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private int currentIndex;
    public Sprite[] doorSprites;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (doorSprites.Length > 0)
        {
            ChangeDoorRandomly(); // Set a random floor at the start
        }
    }

    public void ChangeDoorRandomly()
    {
        if (doorSprites.Length <= 1) return;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, doorSprites.Length);
        } while (newIndex == currentIndex); // Ensure a different sprite is selected

        currentIndex = newIndex;
        spriteRenderer.sprite = doorSprites[currentIndex];
    }
    
}
