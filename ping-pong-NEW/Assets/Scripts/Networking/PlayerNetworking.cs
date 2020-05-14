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

    public GameObject VRBodyPrefab;

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

        IgnoreScripts();

        MonoBehaviour[] reactivate = VRBodyPrefab.GetComponentsInChildren<MonoBehaviour>();
        foreach(MonoBehaviour script in reactivate)
        {
            script.enabled = true;
        }
    }

    //This code previously was inside the Start method
    private void IgnoreScripts()
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
}
