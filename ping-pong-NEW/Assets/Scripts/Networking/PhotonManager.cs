using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    bool isPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;
        ////CONECTAR AL SERVER
        //PhotonNetwork.ConnectUsingSettings();
    }

    //CONECTADO AL SERVER
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    //UNIRSE A LA SALA
    public override void OnJoinedLobby()
    {
        //TODO: ITERATE OVER ALL ROOMS CHECKING IF THERE'S THE MAX NUMBER OF "PLAYER" IN EACH
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        if(isPlayer)
            PhotonNetwork.LocalPlayer.NickName = "Player";
        else
            PhotonNetwork.LocalPlayer.NickName = "Observer";

        if (PhotonNetwork.PlayerList[PhotonNetwork.CurrentRoom.PlayerCount - 1].NickName == "Player")
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

            currentPlayer.GetComponent<PlayerController>().PlayerID = PhotonNetwork.CurrentRoom.masterClientId;
            GameLogic.Instance.AssignTurn(PhotonNetwork.CurrentRoom.masterClientId);
        }
        else
        {
            //TODO: INSTANTIATE A CAMERA (OR WHATEVER)
        }

        //DEBUG
        Debug.Log("Actor number = " + PhotonNetwork.LocalPlayer.ActorNumber);
        Debug.Log("Master client ID = " + PhotonNetwork.CurrentRoom.masterClientId);
    }

    public void OnPlayerClick()
    {
        isPlayer = true;

        PhotonNetwork.AutomaticallySyncScene = true;
        //CONECTAR AL SERVER
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnObserverClick()
    {
        isPlayer = false;

        PhotonNetwork.AutomaticallySyncScene = true;
        //CONECTAR AL SERVER
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ShowNickNames()
    { 
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log(player.NickName);
        }
    }
}
