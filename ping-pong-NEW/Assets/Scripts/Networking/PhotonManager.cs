using System.Collections;
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
        GameObject currentPlayer= PhotonNetwork.Instantiate("NewPlayer", new Vector2(Random.Range(-5, 5), 0), Quaternion.identity);
        if (PhotonNetwork.IsMasterClient)
        {
            //PhotonNetwork.Instantiate("Ball", new Vector3(-0.557f, 0.785f, 3.104f), Quaternion.identity);
        }

    }
}
