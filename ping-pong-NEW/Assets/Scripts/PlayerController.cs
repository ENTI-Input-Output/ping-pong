﻿using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public int PlayerID;
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
        if (SteamVR_Actions.default_GenerateBall.GetStateDown(SteamVR_Input_Sources.LeftHand) && GameLogic.Instance.ServeTurnID == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            //Ball.GetComponent<BallController>().ChangeOwner(int.Parse(PhotonNetwork.LocalPlayer.UserId));
            Ball.GetComponent<BallController>().ChangeOwner(PhotonNetwork.LocalPlayer.ActorNumber);
            triggerPulled = true;
        }

        //Trigger Release
        if (SteamVR_Actions.default_GenerateBall.GetStateUp(SteamVR_Input_Sources.LeftHand) && GameLogic.Instance.ServeTurnID == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            triggerPulled = false;
            Ball.GetComponent<BallController>().IsLocked = false;
        }

        if (/*!Ball.GetComponent<BallController>().IsLocked &&*/ triggerPulled && Ball.GetComponent<Interactable>().attachedToHand == null && LeftHand)
        {
            LeftHand.AttachObject(Ball, GrabTypes.Trigger);
            LeftHand.HoverLock(Ball.GetComponent<Interactable>());
            //Ball.transform.localPosition = new Vector3(-0.0033f, -0.0216f, 0.0433f);
            Ball.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
        else if (/*!Ball.GetComponent<BallController>().IsLocked &&*/ !triggerPulled && Ball.GetComponent<Interactable>().attachedToHand != null && LeftHand)
        {
            Ball.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            LeftHand.DetachObject(Ball);
            LeftHand.HoverUnlock(Ball.GetComponent<Interactable>());
        }
    }
}
