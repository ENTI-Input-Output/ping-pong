using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Surface>() && other.GetComponent<Surface>().SurfaceType == SurfaceType.Paddle)
        {
            GameLogic.Instance.PaddleOverField[GetComponent<Surface>().FieldNum] = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Surface>() && other.GetComponent<Surface>().SurfaceType == SurfaceType.Paddle)
        {
            GameLogic.Instance.PaddleOverField[GetComponent<Surface>().FieldNum] = false;
        }
    }
}
