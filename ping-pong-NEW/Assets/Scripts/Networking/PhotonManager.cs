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
        }
        else
        {
            currentPlayer.transform.position = new Vector3(-1.2f, 0, 3.367f);
            currentPlayer.transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        GameLogic.Instance.AssignTurn(PhotonNetwork.CurrentRoom.masterClientId);
        Debug.Log("Actor number = " + PhotonNetwork.LocalPlayer.ActorNumber);
        Debug.Log("Master client ID = " + PhotonNetwork.CurrentRoom.masterClientId);
    }
}
