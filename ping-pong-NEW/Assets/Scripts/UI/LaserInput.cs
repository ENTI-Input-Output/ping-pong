using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Valve.VR.Extras;
using Photon.Pun;
using UnityEngine.UI;

[RequireComponent(typeof(SteamVR_LaserPointer))]
public class LaserInput : MonoBehaviour
{
    private MainMenu _mainMenu;

    private void Start()
    {
        _mainMenu = GameObject.Find("MainMenuWorldSpace").GetComponent<MainMenu>();
    }

    private void Update()
    {
        if (SteamVR_Actions.default_GenerateBall.GetStateDown(GetComponent<Hand>().handType))
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
            {
                //Debug.Log(hit.transform.gameObject.name);
                if (hit.transform.GetComponent<Button>().interactable)
                {
                    switch (hit.transform.gameObject.name)
                    {
                        case "Regular Mode Match":
                            _mainMenu.RegularModeMatchButton();
                            break;

                        case "Target Mode Match":
                            _mainMenu.TargetModeMatchButton();
                            break;

                        case "Settings":
                            _mainMenu.SettingsButton();
                            break;

                        case "Quit":
                            _mainMenu.ExitButton();
                            break;

                        case "Play":
                            _mainMenu.OnPlayClick();
                            break;

                        case "Observer":
                            _mainMenu.OnObserverClick();
                            break;

                        case "Back":
                            _mainMenu.OnBackClick();
                            break;
                    }
                }
            }
        }
    }
}