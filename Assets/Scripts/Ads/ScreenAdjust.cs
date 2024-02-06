using UnityEngine;

public class ScreenAdjust : MonoBehaviour
{
    void Start()
    {
        Screen.autorotateToPortrait = true;

        Screen.autorotateToPortraitUpsideDown = true;

        Screen.orientation = ScreenOrientation.AutoRotation;
    }
}