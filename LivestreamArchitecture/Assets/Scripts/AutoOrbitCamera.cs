using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoOrbitCamera : MonoBehaviour
{
    // 1: Transform references

    private Transform transformCamera; // refering to camera transformation in space
    private Transform transformPivot; // reference to the pivot object


    // 2: Camera Pivot Transformations, these two values will drive the camera pivot transformations:

    private Vector3 localRotation; // this will store the ongoing pivot rotations every frame, 
                                   //it will be used as a final target rotation for the camera
    private float cameraDistance = -10f; // camera distance that we have in Unity, only in opposite direction

    // 3: Custom Values for fine tuning; these will be customizable properties that will control how the camera animates:

    public float MouseSensitivity = 4f; // controls how much the camera orbits every frame when you move the mouse on the screen
    public float ScrollSensitivity = 3f; // how much the camera zooms in and out based on the scroll wheel input

    public float OrbitDampening = 10f; // this controls how long it takes the camera to reach its destination
    public float ScrollDampening = 6f; // this is dampening towards the target value 
                                       // the bigger the value, the less time it takes to reach the destination, the smaller the value the longer it takes


    // 4: Clamps, these values will define the limits of camera transformations

    public float minCameraRotation = 0f; // lowest possible point of the camera
    public float maxCameraRotation = 90f; // highest possible point of the camera

    public float minCameraDistance = -5f; // closest that the camera can come to the object
    public float maxCameraDistance = -100f; // farthest that the camera can be away from the object

    // 5: Camera Movement On/Off switch

    public bool CameraDisabled = false; // this disabled the camera movement so that we can do other things with the mouse

    private float zoomAmount;

    private void Awake()
    {
        StartCoroutine(Timer()); // this starts our timer
    }

    // Start is called before the first frame update
    void Start()
    {
        transformCamera = transform; // set the camera transform to this transform
        transformPivot = transform.parent; // set the pivot in the script to the pivot game object, which is camera's parent object

    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))// on Left Shift:
        {
            CameraDisabled = !CameraDisabled; // set the boolean value to the opposite of what it currently is
        }

        if (!CameraDisabled) // receive input only if the camera is not disabled
        {

            PivotTransformations();

        }
    }

    private void PivotTransformations()
    {

        Quaternion qRotation = Quaternion.Euler(localRotation.y, localRotation.x, 0);

        transformPivot.rotation = Quaternion.Lerp(transformPivot.rotation, qRotation, Time.deltaTime * OrbitDampening);

        if (transformCamera.localPosition.z != cameraDistance)
        {
            transformCamera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(transformCamera.localPosition.z, cameraDistance, Time.deltaTime * ScrollDampening));
        }


    }

    IEnumerator Timer() // Enumerator needs an Awake method
    {
        while (true) // this means that it will loop
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(2, 5)); // after seconds:
            zoomAmount = UnityEngine.Random.Range(-30f, 30f);
            cameraDistance += zoomAmount;
            cameraDistance = Mathf.Clamp(cameraDistance, minCameraDistance, maxCameraDistance);

            yield return new WaitForSeconds(UnityEngine.Random.Range(4, 7));
            localRotation.y = UnityEngine.Random.Range(minCameraRotation, maxCameraRotation);

            yield return new WaitForSeconds(UnityEngine.Random.Range(6, 10));
            localRotation.x = UnityEngine.Random.Range(1f, 180f);


        }

    }
}
