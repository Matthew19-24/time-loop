using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource musicObject; // Variable for the 'Music' object
    public AudioSource settingsSound; // Variable for the 'Settings' sound
    public GameObject musicOnButton;
    public GameObject musicOffButton;
    public GameObject soundOnButton;
    public GameObject soundOffButton;


    void Start()
    {
        int musicPref = PlayerPrefs.GetInt("Music", 1); // Default to 1 (true) if not set
        int soundPref = PlayerPrefs.GetInt("Sound", 1); // Default to 1 (true) if not set

        if (musicPref == 1) // sound == true
        {
            musicObject.Play();
            musicOnButton.SetActive(true);
            musicOffButton.SetActive(false);
        }
        else // music == false
        {
            musicObject.Stop();
            musicOnButton.SetActive(false);
            musicOffButton.SetActive(true);
        }

        if (soundPref == 1) // sound == true
        {
            soundOnButton.SetActive(true);
            soundOffButton.SetActive(false);
        }
        else // sound == false
        {
            soundOnButton.SetActive(false);
            soundOffButton.SetActive(true);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SaveMusicOff()
    {
        PlayerPrefs.SetInt("Music", 0); // 0 represents false
        PlayerPrefs.Save();
        musicObject.Stop();
        PlaySettingsSound();
    }

    public void SaveMusicOn()
    {
        PlayerPrefs.SetInt("Music", 1); // 1 represents true
        PlayerPrefs.Save();
        PlaySettingsSound();
        musicObject.Play();
    }

    public void SaveSoundOff()
    {
        PlayerPrefs.SetInt("Sound", 0); // 0 represents false
        PlayerPrefs.Save();
        PlaySettingsSound();

    }

    public void SaveSoundOn()
    {
        PlayerPrefs.SetInt("Sound", 1); // 1 represents true
        PlayerPrefs.Save();
        PlaySettingsSound();

    }

    public void PlaySettingsSound()
    {
        int soundPref = PlayerPrefs.GetInt("Sound", 1); // Default to 1 (true) if not set

        if (soundPref == 1) // sound == true
        {
            settingsSound.Play();
        }

    }
}
