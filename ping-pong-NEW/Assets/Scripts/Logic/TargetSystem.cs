using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TargetSystem : MonoBehaviourPun
{
    [Header("References")]
    public GameObject LittleTargetPrefab;
    public GameObject BigTargetPrefab;

    [Header("Settings")]
    public int ConcurrentTargets;
    public Vector2 LittleBigTargetChance;
    public Vector2 XLimits;
    public Vector2 YLimits;
    public Vector2 ZLimits;

    [Header("Current Targets")]
    public bool SpawnTargets = false;
    [SerializeField]
    private bool _firstSpawn = true;
    //[SerializeField]
    //[HideInInspector]
    public List<GameObject> CurrentTargets;

    [SerializeField]
    private PhotonView _photonView;


    private void Update()
    {
        //if (SpawnTargets)
        //{
        //    SpawnInitialTargets();
        //    SpawnTargets = false;
        //}

        if (CurrentTargets.Count < ConcurrentTargets && !_firstSpawn)
        {
            SpawnTarget();
        }
    }

    public void SpawnInitialTargets()
    {
        for (int i = 0; i < ConcurrentTargets; i++)
        {
            SpawnTarget();
        }
        _firstSpawn = false;
    }

    private void SpawnTarget()
    {
        float chance = Random.value;
        Vector3 position = new Vector3(Random.Range(XLimits.x, XLimits.y), Random.Range(YLimits.x, YLimits.y), Random.Range(ZLimits.x, ZLimits.y));
        GameObject newTarget;

        if (chance < LittleBigTargetChance.x)  //Little Target
        {
            newTarget = Instantiate(LittleTargetPrefab, position, Quaternion.identity, transform);
            newTarget.GetComponent<Target>().TargetSystem = this;
            CurrentTargets.Add(newTarget);
        }
        else if (chance < LittleBigTargetChance.y)   //Big Target
        {
            newTarget = Instantiate(BigTargetPrefab, position, Quaternion.identity, transform);
            newTarget.GetComponent<Target>().TargetSystem = this;
            CurrentTargets.Add(newTarget);
        }

        //Send new target to the opponent
        if (!_photonView)
            _photonView = GetComponent<PhotonView>();
        _photonView.RPC("CreateTarget", RpcTarget.OthersBuffered, position, chance);
    }

    private void SpawnTarget(Vector3 position, float chance)
    {
        GameObject newTarget;

        if (chance < LittleBigTargetChance.x)  //Little Target
        {
            newTarget = Instantiate(LittleTargetPrefab, position, Quaternion.identity, transform);
            newTarget.GetComponent<Target>().TargetSystem = this;
            CurrentTargets.Add(newTarget);
        }
        else if (chance < LittleBigTargetChance.y)   //Big Target
        {
            newTarget = Instantiate(BigTargetPrefab, position, Quaternion.identity, transform);
            newTarget.GetComponent<Target>().TargetSystem = this;
            CurrentTargets.Add(newTarget);
        }

        //Send new target to the opponent
        if (!_photonView)
            _photonView = GetComponent<PhotonView>();
        _photonView.RPC("CreateTarget", RpcTarget.OthersBuffered, position);
    }


    [PunRPC]
    private void CreateTarget(Vector3 position, float chance)
    {
        SpawnTarget(position, chance);
    }
}
