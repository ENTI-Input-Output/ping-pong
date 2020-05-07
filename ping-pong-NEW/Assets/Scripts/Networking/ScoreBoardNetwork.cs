using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ScoreBoardNetwork : MonoBehaviourPun
{
    TextMeshProUGUI scoreText;
    public int P1 = 0;
    public int P2 = 0;
    private PhotonView PV;

    [PunRPC]
    void UpdateScorePointP1()
    {
        P1++;
        UpdateScoreLocal();
    }

    [PunRPC]
    void UpdateScorePointP2()
    {
        P2++;
        UpdateScoreLocal();
    }

    void UpdateScoreLocal()
    {
        scoreText.text = P1.ToString() + ":" + P2.ToString();
    }

    void Start()
    {
        PV = transform.GetComponent<PhotonView>();
        scoreText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        //DEBUG
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            PV.RPC("UpdateScorePointP1", RpcTarget.AllBuffered);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PV.RPC("UpdateScorePointP2", RpcTarget.AllBuffered);
        }
    }

    //Called from GameLogic to update local score and send it to the other player
    public void UpdateLocalPlayerScore()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("UpdateScorePointP1", RpcTarget.AllBuffered);
        }
        else
        {
            PV.RPC("UpdateScorePointP2", RpcTarget.AllBuffered);
        }
    }


    /*
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
        }

        else
        {
        }
    }*/
}
