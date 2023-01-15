using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanButtons : MonoBehaviour
{
    public Sprite musicOn, musicOff;
    private void Start()
    {
        if (PlayerPrefs.GetString("music") == "No" && gameObject.name == "music")
            GetComponent<Image>().sprite = musicOff;
    }
    public void Restart()
    {
        if (PlayerPrefs.GetString("music") == "Yes")
            GetComponent<AudioSource>().Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadShop()
    {
        if (PlayerPrefs.GetString("music") == "Yes")
            GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Shop");
    }
    public void CloseShop()
    {
        if (PlayerPrefs.GetString("music") == "Yes")
            GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Main");
    }
    public void Music()
    {
        if (PlayerPrefs.GetString("music") == "No")
        {
            GetComponent<AudioSource>().Play();
            PlayerPrefs.SetString("music", "Yes");
            GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            PlayerPrefs.SetString("music", "No");
            GetComponent<Image>().sprite = musicOff;
        }
    }
}
