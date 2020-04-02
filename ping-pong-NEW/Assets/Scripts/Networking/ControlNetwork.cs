using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ControlNetwork : MonoBehaviourPun
{
    private Rigidbody ballRB;
    private bool changer;
    private void Start()
    {
        ballRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (ballRB.isKinematic&& changer)
        {
            ballRB.gameObject.GetComponent<BallController>().ChangeOwner();
            changer = false;
        }
        else if(ballRB.isKinematic==false)
        {
            changer = true;
        }
    }
}
