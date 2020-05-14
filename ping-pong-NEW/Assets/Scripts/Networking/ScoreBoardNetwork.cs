using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ScoreBoardNetwork : MonoBehaviourPun
{
    TextMeshProUGUI scoreText;
    public TextMeshProUGUI matchP1;
    public TextMeshProUGUI matchP2;
    public int P1 = 0;
    public int MatchP1 = 0;
    public int P2 = 0;
    public int MatchP2 = 0;
    
    private PhotonView PV;

    [PunRPC]
    void UpdateScorePointP1()
    {
        P1++;
        UpdateScoreLocal();
    }

    [PunRPC]
    void UpdateMatchP1()
    {
        MatchP1++;
        UpdateMatchLocal();
    }

    [PunRPC]
    void UpdateScorePointP2()
    {
        P2++;
        UpdateScoreLocal();
    }

    [PunRPC]
    void UpdateMatchP2()
    {
        MatchP2++;
        UpdateMatchLocal();
    }


    void UpdateScoreLocal()
    {
        scoreText.text = P1.ToString() + ":" + P2.ToString();

    }
    void UpdateMatchLocal()
    {
        matchP1.text = MatchP1.ToString();
        matchP2.text = MatchP2.ToString();
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

    public void UpdateLocalMatchScore()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("UpdateMatchP1", RpcTarget.AllBuffered);
        }
        else
        {
            PV.RPC("UpdateMatchP2", RpcTarget.AllBuffered);
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
