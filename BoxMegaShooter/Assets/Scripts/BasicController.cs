using UnityEngine;
using System.Collections;

public class BasicController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Debug.Log(string.Format("Horizontal = {0}", Input.GetAxis("Horizontal")));
    }
}
