using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    bool isPlayer = false;
    public Transform Player1Transform;
    public Transform Player2Transform;

    //Provisional
    public GameObject EnvCamera;

    // Start is called before the first frame update
    void Start()
    {
        //COMMENTED CAUSE IT'S BEING CALLED FROM BUTTONS (METHODS BELOW)    <---------------------------
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
        if (isPlayer)
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
            //TODO: INSTANTIATE A CAMERA (OR WHATEVER)
        }
    }

    public void OnPlayerClick()
    {
        isPlayer = true;

        PhotonNetwork.AutomaticallySyncScene = true;
        //CONECTAR AL SERVER
        PhotonNetwork.ConnectUsingSettings();

        EnvCamera.SetActive(false);
    }

    public void OnObserverClick()
    {
        isPlayer = false;

        PhotonNetwork.AutomaticallySyncScene = true;
        //CONECTAR AL SERVER
        PhotonNetwork.ConnectUsingSettings();
    }

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
    }
}
