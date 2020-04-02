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
        GameObject currentPlayer= PhotonNetwork.Instantiate("OurPlayer", new Vector2(Random.Range(-5, 5), 0), Quaternion.identity);
       
        //PhotonNetwork.Instantiate("Ball", new Vector2(Random.Range(-5, 5), 0), Quaternion.identity);
    }
}
