using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNetworking : MonoBehaviour
{
    public MonoBehaviour[] scriptsToIgnore;
    public Camera[] cameraToIgnore;

    //List of scripts to keep
    private string[] scriptsToKeep = { "PUN2_RigidbodySync", "Photon.Pun.PhotonView", "Photon.Pun.PhotonTransformViewClassic" };

    private PhotonView photonView;

    //Awake
    private void Awake()
    {
        MonoBehaviour[] allScripts = GetComponentsInChildren<MonoBehaviour>();
        List<MonoBehaviour> scriptsList = new List<MonoBehaviour>();

        foreach (MonoBehaviour toIgnore in allScripts)
        {
            bool insert = true;
            foreach (string toKeep in scriptsToKeep)
            {
                if (string.Compare(toIgnore.GetType().ToString(), toKeep) == 0)
                {
                    insert = false;
                }
            }

            if (insert)
                scriptsList.Add(toIgnore);
        }

        scriptsToIgnore = scriptsList.ToArray();
    }

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
