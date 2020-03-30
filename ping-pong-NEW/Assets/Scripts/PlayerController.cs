using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{
    private bool triggerPulled = false;
    public Hand LeftHand;
    public GameObject Ball;

    private void Update()
    {
        //Trigger Pull
        if (SteamVR_Actions.default_GenerateBall.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            triggerPulled = true;
        }

        //Trigger Release
        if (SteamVR_Actions.default_GenerateBall.GetStateUp(SteamVR_Input_Sources.LeftHand))
        {
            triggerPulled = false;
        }

        if (triggerPulled && Ball.GetComponent<Interactable>().attachedToHand == null)
        {
            LeftHand.AttachObject(Ball, GrabTypes.Trigger);
            LeftHand.HoverLock(Ball.GetComponent<Interactable>());
            //Ball.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if (!triggerPulled && Ball.GetComponent<Interactable>().attachedToHand != null)
        {
            //Ball.GetComponent<Rigidbody>().isKinematic = false;
            LeftHand.DetachObject(Ball);
            LeftHand.HoverUnlock(Ball.GetComponent<Interactable>());
        }
    }
}
