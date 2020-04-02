using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Differences : MonoBehaviour
{
    public Material[] matsID;
    public GameObject Ref;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var  player in PhotonNetwork.CurrentRoom.Players){

            Ref.GetComponent<MeshRenderer>().material = matsID[player.Key-1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
