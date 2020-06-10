using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ScoreBoardNetwork : MonoBehaviourPun
{
    //[SerializeField]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI matchP1;
    public TextMeshProUGUI matchP2;
    public int P1 = 0;
    public int MatchP1 = 0;
    public int P2 = 0;
    public int MatchP2 = 0;

    private PhotonView PV;

    [PunRPC]
    void UpdateScorePointP1(int inc)
    {
        P1 += inc;
        UpdateScoreLocal();
    }

    [PunRPC]
    void UpdateMatchP1()
    {
        MatchP1++;
        UpdateMatchLocal();
        //MEthod update gamelogic's score
    }

    [PunRPC]
    void UpdateScorePointP2(int inc)
    {
        P2 += inc;
        UpdateScoreLocal();
    }

    [PunRPC]
    void UpdateMatchP2()
    {
        MatchP2++;
        UpdateMatchLocal();
        //MEthod update gamelogic's score
    }

    [PunRPC]
    void UpdateGameLogic()
    {
        GameLogic.Instance.SetScore();
        P1++;
        UpdateScoreLocal();
    }

    void UpdateScoreLocal()
    {
        scoreText.text = P1.ToString() + ":" + P2.ToString();

        if (PhotonNetwork.IsMasterClient)
        {
            GameLogic.Instance.Games[GameLogic.Instance.CurrentGame].Score[PhotonNetwork.LocalPlayer.ActorNumber] = P1;
            GameLogic.Instance.Games[GameLogic.Instance.CurrentGame].Score[GameLogic.Instance.OpponentID] = P2;
        }
        else
        {
            GameLogic.Instance.Games[GameLogic.Instance.CurrentGame].Score[PhotonNetwork.LocalPlayer.ActorNumber] = P2;
            GameLogic.Instance.Games[GameLogic.Instance.CurrentGame].Score[GameLogic.Instance.OpponentID] = P1;
        }

        GameLogic.Instance.SetScore();
    }
    void UpdateMatchLocal()
    {
        matchP1.text = MatchP1.ToString();
        matchP2.text = MatchP2.ToString();

        P1 = P2 = 0;
        scoreText.text = P1.ToString() + ":" + P2.ToString();
    }

    void Start()
    {
        PV = transform.GetComponent<PhotonView>();
        //scoreText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        //DEBUG
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    PV.RPC("UpdateScorePointP1", RpcTarget.AllBuffered);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    PV.RPC("UpdateScorePointP2", RpcTarget.AllBuffered);
        //}

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PV.RPC("UpdateScorePointP2", RpcTarget.AllBuffered);
            }
            else
            {
                PV.RPC("UpdateScorePointP1", RpcTarget.AllBuffered);
            }
        }
    }

    //LOCAL
    //Called from GameLogic to update local score and send it to the other player
    public void UpdateLocalPlayerScore(int inc)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("UpdateScorePointP1", RpcTarget.AllBuffered, inc);
        }
        else
        {
            PV.RPC("UpdateScorePointP2", RpcTarget.AllBuffered, inc);
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

    //REMOTE
    public void UpdateRemotePlayerScore(int inc)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("UpdateScorePointP2", RpcTarget.AllBuffered, inc);
        }
        else
        {
            PV.RPC("UpdateScorePointP1", RpcTarget.AllBuffered, inc);
        }
    }

    public void UpdateRemoteMatchScore()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("UpdateMatchP2", RpcTarget.AllBuffered);
        }
        else
        {
            PV.RPC("UpdateMatchP1", RpcTarget.AllBuffered);
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
