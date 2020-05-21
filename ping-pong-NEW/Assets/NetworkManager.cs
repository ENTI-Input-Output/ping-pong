using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject RegularObserver;
    public GameObject TargetObserver;
    private List<RoomInfo> roomsInLobby;

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

    private void RoomsUpdate()
    {
        foreach (Room room in roomsInLobby)
        {
            if (room.Name.Contains("RegularRoom") && room.PlayerCount < 4)
            {
                RegularObserver.SetActive(true);
                break;
            }
            else
            {
                RegularObserver.SetActive(false);
                break;
            }
        }

        foreach (Room room in roomsInLobby)
        {
            if (room.Name.Contains("TargetRoom") && room.PlayerCount < 4)
            {
                RegularObserver.SetActive(true);
                break;
            }
            else
            {
                RegularObserver.SetActive(false);
                break;
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        roomsInLobby = roomList;
        RoomsUpdate();
    }
}
