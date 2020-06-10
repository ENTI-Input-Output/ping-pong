using UnityEngine;
using Valve.VR.InteractionSystem;

public class ShowControllers : MonoBehaviour
{
    public bool ShowLeftController = false;
    public bool ShowLeftSkeleton = false;

    public bool ShowRightController = false;
    public bool ShowRightSkeleton = false;


    private void Update()
    {
        ControllersLogic();
    }

    private void ControllersLogic()
    {
        //Left
        if (ShowLeftController)
            Player.instance.hands[0].ShowController();
        else
            Player.instance.hands[0].HideController();

        if (ShowLeftSkeleton)
            Player.instance.hands[0].ShowSkeleton();
        else
            Player.instance.hands[0].HideSkeleton();

        //Right
        if (ShowRightController)
            Player.instance.hands[1].ShowController();
        else
            Player.instance.hands[1].HideController();

        if (ShowRightSkeleton)
            Player.instance.hands[1].ShowSkeleton();
        else
            Player.instance.hands[1].HideSkeleton();
    }
}
