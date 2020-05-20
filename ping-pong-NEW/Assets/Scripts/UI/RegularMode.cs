using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularMode : MonoBehaviour
{
    public GameObject MainMenu;

    public void Play()
    {
        //TODO
    }
    public void Observer()
    {
        //TODO
    }
    public void BackButton()
    {
        MainMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
