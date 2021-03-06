﻿using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

/*
 * Functionality for moving the camera across a linear level between specified points
 */
public class CameraMovement : MonoBehaviour
{
    public static Button CameraMovementButton { get; private set; }

    private Camera camToMove;

    [SerializeField]
    private List<Transform> cameraPositions;

    [SerializeField]
    private float cameraMovementSpeed = 2f;

    private int camPositionIndex;




    // Start is called before the first frame update
    void Start()
    {
        camToMove = Camera.main;
        CameraMovementButton = GameObject.Find("CameraMovementButton").GetComponent<Button>();
    }

   /*
    * Button event for camera movement. Calls a lerp function and increments camera position index 
    */
    public void CameraMovementEvent()
    {
        if (camPositionIndex + 1 == cameraPositions.Count)
        {
            StopAllCoroutines();
            return;
        }
        else if(camPositionIndex + 2 == cameraPositions.Count)
        {
            CameraMovementButton.interactable = false;
        }
        StartCoroutine(Utils.LerpBetweenTransformsOverTime(camToMove.transform, cameraPositions[camPositionIndex], cameraPositions[camPositionIndex +1], cameraMovementSpeed, true));
        camPositionIndex++;
    }


}
