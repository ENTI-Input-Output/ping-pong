using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameObject MainMenu;

    public void MusicSlider()
    {
        //manage
    }

    public void FXSlider()
    {
        //manage
    }
    public void BackButton()
    {
        MainMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
