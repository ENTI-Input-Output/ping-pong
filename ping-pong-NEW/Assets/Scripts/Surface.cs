using UnityEngine;

public enum SurfaceType { Field, Floor, Paddle, Net, Target, None };

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
