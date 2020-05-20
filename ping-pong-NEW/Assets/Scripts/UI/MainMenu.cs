using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public GameObject Settings;
    public GameObject RegularMode;
    public GameObject TargetMode;
    
    public void RegularModeMatchButton()
    {
        RegularMode.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void TargetModeMatchButton()
    {
        TargetMode.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void SettingsButton()
    {
        Settings.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
    
}
