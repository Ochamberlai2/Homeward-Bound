using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Allows the camera to be moved via the ui button 
 */
 [CreateAssetMenu(menuName ="Actions/Allow Camera Movement")]
public class Action_AllowCameraMovement : ScenarioAction
{
    protected override void ActionFunction()
    {
        CameraMovement.CameraMovementButton.interactable = true;
    }
}
