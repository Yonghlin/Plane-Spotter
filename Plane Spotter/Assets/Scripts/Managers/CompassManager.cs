using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompassManager : MonoBehaviour
{
    public TMP_Text yaw;
    public TMP_Text comp;

    // Start is called before the first frame update
    void Start()
    {
        //transform.rotation = Quaternion.Euler(0, -Input.compass.trueHeading, 0);
        Input.gyro.enabled = true;
        Input.compass.enabled = true;

        //transform.Rotate(-attitude.x, -attitude.y, -attitude.z, Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        yaw.text = "Cam Rot (Yaw): " + Input.gyro.attitude.eulerAngles.x.ToString();
        comp.text = "Compass: " + Input.compass.magneticHeading.ToString(); 
        //comp.text = "accel: " + Input.acceleration.z;

        //Debug.Log("Is Gyro Enabled: " + Input.gyro.enabled);
        //Debug.Log("Input Gyro: " + Input.gyro.attitude);
    }
}
