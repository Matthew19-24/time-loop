using UnityEngine;

public class Inventory : MonoBehaviour
{

    public AudioSource sound;

public void OpenInventory(){
    int musicPref = PlayerPrefs.GetInt("Sound", 1); // Default to 1 (true) if not set
    if (musicPref == 1 && sound != null)
    {
        sound.Play();
    }
    }

    public void CloseInventory(){
    int musicPref = PlayerPrefs.GetInt("Sound", 1); // Default to 1 (true) if not set
    if (musicPref == 1 && sound != null)
    {
        sound.Play();
    }
    }
}
