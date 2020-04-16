using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleNetwork : MonoBehaviour
{
    private BallController ballController;

    private void Start()
    {
        if (!ballController)
        {
            ballController = GameObject.FindGameObjectWithTag("ball").GetComponent<BallController>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("ball"))
        {
            ballController.ChangeOwner();
        }
    }
}
