using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Main;
    public GameObject Settings;
    public GameObject RegularMode;
    public GameObject TargetMode;
    public GameObject Player;
    public AudioClip clickSound;

    //[SerializeField]
    private string SceneToLoad;

    private void ClickSound()
    {
        GetComponent<AudioSource>().PlayOneShot(clickSound);
    }

    private void Start()
    {
        Settings.SetActive(false);
        RegularMode.SetActive(false);
        TargetMode.SetActive(false);
    }
    public void RegularModeMatchButton()
    {
        ClickSound();
        RegularMode.SetActive(true);
        //Main.SetActive(false);
        Settings.SetActive(false);
        TargetMode.SetActive(false);

        SceneToLoad = "RegularMatch";
        DataManager.Instance.RoomType = "RegularRoom";
    }

    public void TargetModeMatchButton()
    {
        ClickSound();
        TargetMode.SetActive(true);
        //Main.SetActive(false);
        Settings.SetActive(false);
        RegularMode.SetActive(false);

        SceneToLoad = "TargetMatch";
        DataManager.Instance.RoomType = "TargetRoom";
    }

    public void SettingsButton()
    {
        ClickSound();
        Settings.SetActive(true);
        //Main.SetActive(false);
        RegularMode.SetActive(false);
        TargetMode.SetActive(false);
    }

    public void ExitButton()
    {
        ClickSound();
        Application.Quit();
    }

    public void OnPlayClick()
    {
        ClickSound();
        DataManager.Instance.IsPlayer = true;
        Destroy(Player);
        SceneManager.LoadScene(SceneToLoad);
    }

    public void OnObserverClick()
    {
        ClickSound();
        DataManager.Instance.IsPlayer = false;
        Destroy(Player);
        SceneManager.LoadScene(SceneToLoad);
    }

    public void OnBackClick()
    {
        ClickSound();
        Main.SetActive(true);
        RegularMode.SetActive(false);
        TargetMode.SetActive(false);
        Settings.SetActive(false);
    }
}
