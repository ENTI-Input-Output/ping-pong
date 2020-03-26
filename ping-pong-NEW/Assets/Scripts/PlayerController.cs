using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{
    public Hand LeftHand;
    public GameObject Ball;

    private void Update()
    {
        //Trigger Pull
        if (SteamVR_Actions.default_GenerateBall.GetStateDown(SteamVR_Input_Sources.Any))
        {
            if (Ball.GetComponent<Interactable>().attachedToHand == null)
            {
                LeftHand.AttachObject(Ball, GrabTypes.Trigger);
                LeftHand.HoverLock(Ball.GetComponent<Interactable>());
            }
        }

        //Trigger Release
        if (SteamVR_Actions.default_GenerateBall.GetStateUp(SteamVR_Input_Sources.Any))
        {
            LeftHand.DetachObject(Ball);
            LeftHand.HoverUnlock(Ball.GetComponent<Interactable>());
        }
    }
}
