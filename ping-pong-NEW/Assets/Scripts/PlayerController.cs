using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{
    private bool triggerPulled = false;
    public Hand LeftHand;
    private GameObject Ball;
    public GameObject teleporting;

    private void Start()
    {
        Ball = GameObject.FindGameObjectWithTag("ball");
        //Instantiate(teleporting, Vector3.zero, Quaternion.identity);  //Will need this if we want to let the player teleport to points and areas
    }

    private void Update()
    {
        //Trigger Pull
        if (SteamVR_Actions.default_GenerateBall.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Debug.Log("mismuertos");
            Ball.GetComponent<BallController>().ChangeOwner();
            triggerPulled = true;
        }

        //Trigger Release
        if (SteamVR_Actions.default_GenerateBall.GetStateUp(SteamVR_Input_Sources.LeftHand))
        {
            triggerPulled = false;
        }

        if (triggerPulled && Ball.GetComponent<Interactable>().attachedToHand == null&& LeftHand)
        {
            LeftHand.AttachObject(Ball, GrabTypes.Trigger);
            LeftHand.HoverLock(Ball.GetComponent<Interactable>());
            //Ball.transform.localPosition = new Vector3(-0.0033f, -0.0216f, 0.0433f);
            //Ball.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if (!triggerPulled && Ball.GetComponent<Interactable>().attachedToHand != null && LeftHand)
        {
            //Ball.GetComponent<Rigidbody>().isKinematic = false;
            LeftHand.DetachObject(Ball);
            LeftHand.HoverUnlock(Ball.GetComponent<Interactable>());
        }
    }
}
