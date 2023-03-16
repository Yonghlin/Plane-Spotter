using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using gps = GPS;

public class Airport : MonoBehaviour
{
    public string Code;
    public string Name;
    public double Elevation;
    public double Longitude;
    public double Latitude;

    // GPS
    public GPS gps;

    private float ElevationFloat;
    private float LongitudeFloat; 
    private float LatitudeFloat;

    // Start is called before the first frame update
    void Start()
    {
        // set initial location here
        ElevationFloat = (float)Elevation;
        LongitudeFloat = (float)Longitude;
        LatitudeFloat = (float)Latitude;

        //obj.transform.position.Set(LatitudeFloat, ElevationFloat, LongitudeFloat);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 unityCoords = GPSEncoder.GPSToUCS(
            (float) (Latitude - gps.getLatitude()),
            (float) (Longitude - gps.getLongitude())
        );

        transform.position = new Vector3(
            unityCoords.x / 100,
            (float) 0,
            unityCoords.y / 100
        );

        Debug.Log("distance (x): " + unityCoords.x);
        Debug.Log("distance (z): " + unityCoords.y); // b/c it's a Vector2 not a Vector3
    }
}


