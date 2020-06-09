using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int ScoreInc = 5;

    public TargetSystem TargetSystem;

    public void CheckPosition()
    {
        //todo
        //check if it is overlapping with other objects and move it if so
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.GetComponent<Surface>() && collision.transform.GetComponent<Surface>().SurfaceType == SurfaceType.Target)
        {
            transform.position = new Vector3(Random.Range(TargetSystem.XLimits.x, TargetSystem.XLimits.y), Random.Range(TargetSystem.YLimits.x, TargetSystem.YLimits.y), Random.Range(TargetSystem.ZLimits.x, TargetSystem.ZLimits.y));
        }
    }

    private void OnDestroy()
    {
        TargetSystem.CurrentTargets.Remove(gameObject);
    }
}
