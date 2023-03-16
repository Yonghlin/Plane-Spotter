using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using gps = GPS;
using airportManager = AirportManager;

public class UpdateDebugData : MonoBehaviour
{
    public TMP_Text latitude;
    public TMP_Text longitude;
    public TMP_Text altitude;
    public TMP_Text timesRun;
    public GPS gps;
    public TMP_Text numAirports;
    //public airportManager airportManager;

    // Update is called once per frame
    void Update()
    {
        latitude.text = "Latitude: " + gps.getLatitude();
        longitude.text = "Longitude: " + gps.getLongitude();
        altitude.text = "Altitude: " + gps.getAltitude();
        timesRun.text = "Times Run: " + gps.getTimesRun();
        //numAirports.text = "Num Airports: " + airportManager.GetNumAirports();
    }
}
