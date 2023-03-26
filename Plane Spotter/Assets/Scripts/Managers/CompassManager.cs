using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //transform.rotation = Quaternion.Euler(0, -Input.compass.trueHeading, 0);
        Input.gyro.enabled = true;
        Input.compass.enabled = true;

        // TODO Make this work. Get airport objects to be in absolute world position,
        // by setting camera rotation appropriately based on phone's current orientation.
        Quaternion attitude = Input.gyro.attitude;
        //transform.Rotate(-attitude.x, -attitude.y, -attitude.z, Space.Self);
        //transform.rotation = Quaternion.Euler(attitude.x, attitude.y, attitude.z);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Is Gyro Enabled: " + Input.gyro.enabled);
        Debug.Log("Is Compass Enabled " + Input.compass.enabled);
        Debug.Log("Input Gyro: " + Input.gyro.attitude);

        //transform.rotation = Quaternion.Euler(phoneRotX, phoneRotY, phoneRotZ);
        //transform.rotation = Input.gyro.attitude;

        Quaternion attitude = Input.gyro.attitude;
        //ransform.rotation = Quaternion.Euler(attitude.x, attitude.y, attitude.z);
    }
}
