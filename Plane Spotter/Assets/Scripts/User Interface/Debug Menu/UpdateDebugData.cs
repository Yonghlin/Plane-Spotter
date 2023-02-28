using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using gps = GPS;

public class UpdateDebugData : MonoBehaviour
{
    public TMP_Text latitude;
    public TMP_Text longitude;
    public TMP_Text altitude;
    public TMP_Text timesRun;
    public GPS gps;

    // Update is called once per frame
    void Update()
    {
        latitude.text = "Latitude: " + gps.getLatitude();
        longitude.text = "Longitude: " + gps.getLongitude();
        altitude.text = "Altitude: " + gps.getAltitude();
        timesRun.text = "Times Run: " + gps.getTimesRun();
        
    }
}
