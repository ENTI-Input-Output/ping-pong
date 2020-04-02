using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNetworking : MonoBehaviour
{
    public MonoBehaviour[] scriptsToIgnore;
    public Camera[] cameraToIgnore;


    private PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            foreach (var script in scriptsToIgnore)
            {
                script.enabled = false;
            }
            foreach (var camera in cameraToIgnore)
            {
                camera.enabled = false;
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
