using UnityEngine;

public enum SurfaceType { Field, Floor, Paddle, Net, None };

[System.Serializable]
public class Surface : MonoBehaviour
{
    public SurfaceType SurfaceType;

    [Header("Field Params")]
    public int FieldNum = -1;

    public Surface(SurfaceType surfaceType)
    {
        SurfaceType = surfaceType;
    }
}
