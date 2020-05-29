using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Main;
    public GameObject Settings;
    public GameObject RegularMode;
    public GameObject TargetMode;
    public GameObject Player;

    //[SerializeField]
    private string SceneToLoad;

    private void Start()
    {
        Settings.SetActive(false);
        RegularMode.SetActive(false);
        TargetMode.SetActive(false);
    }
    public void RegularModeMatchButton()
    {
        RegularMode.SetActive(true);
        //Main.SetActive(false);
        Settings.SetActive(false);
        TargetMode.SetActive(false);

        SceneToLoad = "RegularMatch";
        DataManager.Instance.RoomType = "RegularRoom";
    }

    public void TargetModeMatchButton()
    {
        TargetMode.SetActive(true);
        //Main.SetActive(false);
        Settings.SetActive(false);
        RegularMode.SetActive(false);

        SceneToLoad = "TargetMatch";
        DataManager.Instance.RoomType = "TargetRoom";
    }

    public void SettingsButton()
    {
        Settings.SetActive(true);
        //Main.SetActive(false);
        RegularMode.SetActive(false);
        TargetMode.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void OnPlayClick()
    {
        DataManager.Instance.IsPlayer = true;
        Destroy(Player);
        SceneManager.LoadScene(SceneToLoad);
    }

    public void OnObserverClick()
    {
        DataManager.Instance.IsPlayer = false;
        Destroy(Player);
        SceneManager.LoadScene(SceneToLoad);
    }

    public void OnBackClick()
    {
        Main.SetActive(true);
        RegularMode.SetActive(false);
        TargetMode.SetActive(false);
        Settings.SetActive(false);
    }
}
