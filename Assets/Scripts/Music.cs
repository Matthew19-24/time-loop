using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource[] musicObjects; // Array for the 'Music' objects
    private AudioSource currentMusic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int musicPref = PlayerPrefs.GetInt("Music", 1); // Default to 1 (true) if not set
        int soundPref = PlayerPrefs.GetInt("Sound", 1); // Default to 1 (true) if not set

        if (musicPref == 1) // music == true
        {
            int randomIndex = Random.Range(0, musicObjects.Length);
            currentMusic = musicObjects[randomIndex];
            currentMusic.Play();
        }
        else // music == false
        {
            foreach (var musicObject in musicObjects)
            {
                musicObject.Stop();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
