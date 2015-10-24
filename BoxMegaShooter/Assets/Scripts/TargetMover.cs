using UnityEngine;
using System.Collections;
using System;

public class TargetMover : MonoBehaviour
{
    // define the possible states through an enumeration
    [Flags]
    public enum motionDirections
    {
        Spin = 0x1,
        Horizontal = 0x2,
        Vertical = 0x4
    };

    // store the state
    [SerializeField]
    [EnumFlag]
    public motionDirections motionState = motionDirections.Horizontal;

    // motion parameters
    public float spinSpeed = 180.0f;
    public float motionMagnitude = 0.1f;

    // Update is called once per frame
    void Update()
    {

        //// do the appropriate motion based on the motionState
        //switch (motionState)
        //{
        //    case motionDirections.Spin:
        //        // rotate around the up axix of the gameObject
        //        gameObject.transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
        //        break;
        //    case motionDirections.Horizontal:
        //        // move up and down over time
        //        gameObject.transform.Translate(Vector3.right * Mathf.Cos(Time.timeSinceLevelLoad) * motionMagnitude);
        //        break;
        //    case motionDirections.Vertical:
        //        // move up and down over time
        //        gameObject.transform.Translate(Vector3.up * Mathf.Cos(Time.timeSinceLevelLoad) * motionMagnitude);
        //        break;
        //}

        if ((motionState & motionDirections.Spin) != 0)
        {
            // rotate around the up axix of the gameObject
            gameObject.transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
        }
        
        var newZ = gameObject.transform.position.z;
        var newY = (motionState & motionDirections.Vertical) == 0
            ? gameObject.transform.position.y
            : gameObject.transform.position.y + Mathf.Cos(Time.timeSinceLevelLoad) * motionMagnitude;
        var newX = (motionState & motionDirections.Horizontal) == 0
            ? gameObject.transform.position.x
            : gameObject.transform.position.x + Mathf.Cos(Time.timeSinceLevelLoad) * motionMagnitude;

        var newPosition = new Vector3(newX, newY, newZ);
       
        gameObject.transform.position = newPosition;
    }
}
