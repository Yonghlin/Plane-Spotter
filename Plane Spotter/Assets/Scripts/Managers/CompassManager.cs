using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompassManager : MonoBehaviour
{
    public TMP_Text yaw;

    // Start is called before the first frame update
    void Start()
    {
        //transform.rotation = Quaternion.Euler(0, -Input.compass.trueHeading, 0);
        Input.gyro.enabled = true;

        // TODO Make this work. Get airport objects to be in absolute world position,
        // by setting camera rotation appropriately based on phone's current orientation.
        Quaternion attitude = Input.gyro.attitude;
        //transform.Rotate(-attitude.x, -attitude.y, -attitude.z, Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        //yaw.text = transform.rotation.y.ToString();
        yaw.text = "Cam Rot (Yaw): " + Input.gyro.attitude.eulerAngles.x.ToString();
        transform.rotation = Quaternion.Euler(0f, Input.gyro.attitude.eulerAngles.y, 0f);

        Debug.Log("Is Gyro Enabled: " + Input.gyro.enabled);
        Debug.Log("Input Gyro: " + Input.gyro.attitude);
    }
}
