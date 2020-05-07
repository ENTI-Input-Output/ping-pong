using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FieldController : MonoBehaviourPun
{
    private BallController ballController;

    [Header("Field Params")]
    public int FieldNum = -1;

    private void Start()
    {
        ballController = GameObject.FindGameObjectWithTag("ball").GetComponent<BallController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Surface>() && other.GetComponent<Surface>().SurfaceType == SurfaceType.Paddle)
        {
            GameLogic.Instance.PaddleOverField[GetComponent<Surface>().FieldNum] = true;
        }

        if(PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            if (other.transform.CompareTag("ball") && FieldNum == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                ballController.ChangeOwner(PhotonNetwork.LocalPlayer.ActorNumber);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Surface>() && other.GetComponent<Surface>().SurfaceType == SurfaceType.Paddle)
        {
            GameLogic.Instance.PaddleOverField[GetComponent<Surface>().FieldNum] = false;
        }
    }
}
