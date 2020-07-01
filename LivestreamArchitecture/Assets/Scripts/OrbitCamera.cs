using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    // 1: Transform references

    private Transform transformCamera; // refering to camera transformation in space
    private Transform transformPivot; // reference to the pivot object


    // 2: Camera Pivot Transformations, these two values will drive the camera pivot transformations:

    private Vector3 localRotation; // this will store the ongoing pivot rotations every frame, 
                                   //it will be used as a final target rotation for the camera
    private float cameraDistance = 10f; // camera distance that we have in Unity, only in opposite direction

    // 3: Custom Values for fine tuning; these will be customizable properties that will control how the camera animates:

    public float MouseSensitivity = 4f; // controls how much the camera orbits every frame when you move the mouse on the screen
    public float ScrollSensitivity = 3f; // how much the camera zooms in and out based on the scroll wheel input

    public float OrbitDampening = 10f; // this controls how long it takes the camera to reach its destination
    public float ScrollDampening = 6f; // this is dampening towards the target value 
                                       // the bigger the value, the less time it takes to reach the destination, the smaller the value the longer it takes


    // 4: Clamps, these values will define the limits of camera transformations

    public float minCameraRotation = 0f; // lowest possible point of the camera
    public float maxCameraRotation = 90f; // highest possible point of the camera

    public float minCameraDistance = 5f; // closest that the camera can come to the object
    public float maxCameraDistance = 100f; // farthest that the camera can be away from the object

    // 5: Camera Movement On/Off switch

    public bool CameraDisabled = false; // this disabled the camera movement so that we can do other things with the mouse


    // Start is called before the first frame update
    void Start()
    {
        transformCamera = transform; // set the camera transform to this transform
        transformPivot = transform.parent; // set the pivot in the script to the pivot game object, which is camera's parent object

    }

    // Late Update is called after Update()
    private void LateUpdate()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))// on Left Shift:
        {
            CameraDisabled = !CameraDisabled; // set the boolean value to the opposite of what it currently is
        }


        // Rotation 
        if (!CameraDisabled) // receive input only if the camera is not disabled
        {
            if (Input.GetMouseButton(1)) // receive input only if right mouse button pressed
            {

                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) // receive input only if mouse moving
                {
                    localRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity; // increment the x component of the local rotation Vector by mouse X axis multiplied by the mouse sensitivity
                    localRotation.y -= Input.GetAxis("Mouse Y") * MouseSensitivity; // Unity flips the input axis, += gives inverted controls

                    // Clamp the rotation to min and max vaules

                    //if( localRotation.y < minCameraRotation)
                    //{
                    //    localRotation.y = minCameraRotation;
                    //}
                    //else if (localRotation.y > maxCameraRotation)
                    //{
                    //    localRotation.y = maxCameraRotation;
                    //}

                    localRotation.y = Mathf.Clamp(localRotation.y, minCameraRotation, maxCameraRotation);


                }

            }


            // Scrolling Input from mouse Wheel

            if (Input.GetAxis("Mouse ScrollWheel") != 0) // check if the mouse scroll input is there
            {
                float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity; // temporary variable to store input from our scroll wheel, multiplied by sensitivity

                scrollAmount *= (cameraDistance * 0.3f); // magic formula, takes the 30% of the camera distance and multiplies the scroll amount
                                                         // this will allow for scrolling faster the farther away we are, and slower the closer we are

                cameraDistance += scrollAmount * -1f; // add scroll amount to our camera distance and flip it to the opposite direction

                cameraDistance = Mathf.Clamp(cameraDistance, minCameraDistance, maxCameraDistance); // clamp camera distance between min and max values
            }

        }

        // Actual Camera Rig Transformations, these have to happen within Late Update

        // Create a Quaternion Euler, set the pitch and yaw from our local rotation values, and no rotation on Z

        Quaternion qRotation = Quaternion.Euler(localRotation.y, localRotation.x, 0); // using quaternions will prevent the gimbal lock

        transformPivot.rotation = Quaternion.Lerp(transformPivot.rotation, qRotation, Time.deltaTime * OrbitDampening); // setting the rotation of the Pivot transform through Lerp betwen the transform and our Quaternion target rotation

        if (transformCamera.localPosition.z != cameraDistance * -1f)
        {
            transformCamera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(transformCamera.localPosition.z, cameraDistance * -1f, Time.deltaTime * ScrollDampening));
        }


    }
}
