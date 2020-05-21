using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

public class DataManager : MonoBehaviour
{
    #region Singleton
    public static DataManager Instance;

    //Awake
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public bool IsPlayer = false;
    public string RoomType;
    public List<RoomInfo> roomsInLobby;
}
