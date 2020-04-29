﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{


    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        //CONECTAR AL SERVER
        PhotonNetwork.ConnectUsingSettings();
    }
    //CONECTADO AL SERVER
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    //UNIRSE A LA SALA

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        GameObject currentPlayer = PhotonNetwork.Instantiate("NewPlayer", Vector3.zero, Quaternion.identity);

        if (PhotonNetwork.IsMasterClient)
        {
            currentPlayer.transform.position = new Vector3(2.286f, 0, 3.367f);
            currentPlayer.transform.rotation = Quaternion.Euler(0, -90, 0);
            currentPlayer.GetComponent<PlayerController>().PlayerID = 0;
            GameLogic.Instance.Players.Add(PhotonNetwork.CurrentRoom.masterClientId, new CustomPlayer(PhotonNetwork.CurrentRoom.masterClientId, 0));
        }
        else
        {
            //currentPlayer = PhotonNetwork.Instantiate("OtherPlayer", Vector3.zero, Quaternion.identity);
            currentPlayer.transform.position = new Vector3(-1.2f, 0, 3.367f);
            currentPlayer.transform.rotation = Quaternion.Euler(0, 90, 0);
            currentPlayer.GetComponent<PlayerController>().PlayerID = 1;
            GameLogic.Instance.Players.Add(PhotonNetwork.LocalPlayer.ActorNumber, new CustomPlayer(PhotonNetwork.LocalPlayer.ActorNumber, 1));
        }
    }
}