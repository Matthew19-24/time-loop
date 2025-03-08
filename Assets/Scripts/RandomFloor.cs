using UnityEngine;

public class RandomFloor : MonoBehaviour
{
    public Sprite[] floorSprites; // Assign your 5 floor sprites in the Inspector
    private SpriteRenderer spriteRenderer;
    private int currentIndex;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (floorSprites.Length > 0)
        {
            ChangeFloorRandomly(); // Set a random floor at the start
        }
    }

    public void ChangeFloorRandomly()
    {
        if (floorSprites.Length <= 1) return;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, floorSprites.Length);
        } while (newIndex == currentIndex); // Ensure a different sprite is selected

        currentIndex = newIndex;
        spriteRenderer.sprite = floorSprites[currentIndex];
    }
    
}
