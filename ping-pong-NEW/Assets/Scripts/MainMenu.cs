using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public GameObject Settings;
    
    public void RegularModeMatchButton()
    {
        //Open
    }

    public void TargetModeMatchButton()
    {
        //Open
    }
    public void ObserverButton()
    {
        //Open Observer
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
