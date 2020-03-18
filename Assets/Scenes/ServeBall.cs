using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ServeBall : MonoBehaviour
{
    public SteamVR_Input_Sources hand;
    public SteamVR_Action_Boolean triggerPress;

    private void Update()
    {
        if (triggerPress.GetStateDown(hand))
        {
            BallController ballController = GameObject.Find("Ball").GetComponent<BallController>();
            ballController.paddle = transform.Find("attach").Find("Collider").gameObject;
            ballController.serve = true;
        }
    }

    private void FixedUpdate()
    {
        //var rb = this.gameObject.transform.Find("attach").Find("Collider").GetComponent<Rigidbody>();
        //Debug.Log(rb.velocity);
    }
}
