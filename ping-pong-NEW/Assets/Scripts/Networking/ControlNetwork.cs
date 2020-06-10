using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ControlNetwork : MonoBehaviourPun
{
    private Rigidbody ballRB;
    private void Start()
    {
        ballRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //if (ballRB.isKinematic)
        //{
        //    ballRB.gameObject.GetComponent<BallController>().ChangeOwner();
        //}

    }
}
