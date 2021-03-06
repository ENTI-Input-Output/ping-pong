﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    //bool isPlayer = false;
    public Transform Player1Transform;
    public Transform Player2Transform;

    public Transform ObserverTransform;

    //List<RoomInfo> roomsInLobby;

    //Provisional
    public GameObject EnvCamera;

    // Start is called before the first frame update
    void Start()
    {
        OnCustomJoinedLobby();
    }

    //UNIRSE A LA SALA
    public void OnCustomJoinedLobby()
    {
        if (DataManager.Instance.IsPlayer)
        {
            bool joined = false;
            foreach (RoomInfo room in DataManager.Instance.roomsInLobby)
            {
                if (room.Name.Contains(DataManager.Instance.RoomType) && room.PlayerCount <= 1)
                {
                    PhotonNetwork.JoinRoom(room.Name);
                    joined = true;
                    break;
                }
            }

            if (!joined)
                PhotonNetwork.CreateRoom(DataManager.Instance.RoomType + PhotonNetwork.CountOfRooms, new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);

        }
        else
        {
            foreach (RoomInfo room in DataManager.Instance.roomsInLobby)
            {
                if (room.Name.Contains(DataManager.Instance.RoomType) && room.PlayerCount >= 2 && room.PlayerCount < 4)
                {
                    PhotonNetwork.JoinRoom(room.Name);
                    break;
                }
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        DataManager.Instance.roomsInLobby = roomList;
    }

    public override void OnJoinedRoom()
    {
        if (DataManager.Instance.IsPlayer)
            PhotonNetwork.LocalPlayer.NickName = "Player";
        else
            PhotonNetwork.LocalPlayer.NickName = "Observer";

        if (PhotonNetwork.PlayerList[PhotonNetwork.CurrentRoom.PlayerCount - 1].NickName == "Player")
        {
            GameObject currentPlayer = PhotonNetwork.Instantiate("NewPlayer", Vector3.zero, Quaternion.identity);

            bool isFirstPlayer = true;
            foreach (Player player in PhotonNetwork.PlayerListOthers)
            {
                if (player.NickName == "Player")
                {
                    isFirstPlayer = false;
                }
            }

            if (isFirstPlayer)
            {
                currentPlayer.transform.position = Player1Transform.position;
                currentPlayer.transform.rotation = Player1Transform.rotation;
            }
            else
            {
                currentPlayer.transform.position = Player2Transform.position;
                currentPlayer.transform.rotation = Player2Transform.rotation;
            }

            currentPlayer.GetComponent<PlayerController>().PlayerID = PhotonNetwork.CurrentRoom.masterClientId;
            GameLogic.Instance.AssignTurn(PhotonNetwork.CurrentRoom.masterClientId);
        }
        else
        {
            GameObject currentPlayer = PhotonNetwork.Instantiate("Observer", Vector3.zero, Quaternion.identity);
            currentPlayer.transform.position = ObserverTransform.position;
            currentPlayer.transform.rotation = ObserverTransform.rotation;
        }
    }

    //public void OnPlayerClick()
    //{
    //    isPlayer = true;

    //    PhotonNetwork.AutomaticallySyncScene = true;
    //    //CONECTAR AL SERVER
    //    PhotonNetwork.ConnectUsingSettings();

    //    EnvCamera.SetActive(false);
    //}

    //public void OnObserverClick()
    //{
    //    isPlayer = false;

    //    PhotonNetwork.AutomaticallySyncScene = true;
    //    //CONECTAR AL SERVER
    //    PhotonNetwork.ConnectUsingSettings();
    //}

    public void PrintNicknames()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log(player.NickName + " with ID: " + player.ActorNumber);

            if (player == PhotonNetwork.LocalPlayer)
            {
                Debug.Log("You are a " + player.NickName + " and your ID is: " + player.ActorNumber);
            }
        }

        Debug.Log("Room is of type " + PhotonNetwork.CurrentRoom.Name);
    }
}
