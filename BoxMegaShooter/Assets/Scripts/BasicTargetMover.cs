using UnityEngine;
using System.Collections;

public class BasicTargetMover : MonoBehaviour
{
    public float SpinSpeed = 10f;
    public float MotionMagnitude = 0.1f;
    //// Use this for initialization
    //void Start () 
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        // rotate
        gameObject.transform.Rotate(Vector3.up * SpinSpeed * Time.deltaTime);

        // move up and down
        gameObject.transform.Translate(Vector3.up * Mathf.Cos(Time.timeSinceLevelLoad) * MotionMagnitude);
    }
}
