using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using gps = GPS;

public class Airplane : MonoBehaviour
{
    public string PlaneId;
    public string Code;
    public string Name;
    public double Elevation;
    public double Longitude;
    public double Latitude;

    // GPS
    [Range(1, 5)]
    public float waitTimeBeforeInstantiation;

    private double pos1_longitude;
    private double pos1_latitude;
    private double pos1_elevation;

    private double pos2_longitude;
    private double pos2_latitude;
    private double pos2_elevation;

    private bool grabbedPos1;

    private double longitudeVelocity;
    private double latitudeVelocity;
    private double velocityElevation;

    public float timeToWaitVelocity;
    private float timeWaited = 0;
    private float timeStart;

    public GPS gps;
    public float distance_multiplier;
    public float elevation_multiplier;

    private CompassManager compassManager;

    private void SetPosition()
    {
        // convert latitude/longitude to x/y coordinates
        Vector3 unityCoords = new Vector3(
                   distance_multiplier * (float)(Latitude - gps.getLatitude()),
                   elevation_multiplier * (float)(Elevation - gps.getAltitude()),
                   distance_multiplier * (float)(Longitude - gps.getLongitude()));
        // get the position binding script
        PositionBindManager posManager = this.GetComponent<PositionBindManager>();
        posManager.SetBoundPosAndScale(this.gameObject, unityCoords);
    }

    // Start is called before the first frame update
    void Start()
    {
        timeStart = (float)((DateTime.Now.Ticks) / TimeSpan.TicksPerMillisecond);
        compassManager = GameObject.FindGameObjectWithTag("CompassCamera")
                                   .GetComponent<CompassManager>();
    }

    public void UpdatePosition(float ElevationNew, float LatitudeNew, float LongitudeNew)
    {
        pos2_elevation = ElevationNew;
        pos2_latitude = LatitudeNew;
        pos2_longitude = LongitudeNew;

        Elevation = ElevationNew;
        Latitude = LatitudeNew;
        Longitude = LongitudeNew;

        Vector3 pos1 = new Vector3((float) pos1_latitude, (float) pos1_elevation, (float) pos1_longitude);
        Vector3 pos2 = new Vector3((float) pos2_latitude, (float) pos2_elevation, (float) pos2_longitude);
        Vector3 dir = (pos2 - pos1) * 10000;

        transform.LookAt(dir);
        //transform.LookAt(new Vector3((float)pos1_latitude * 10, (float)pos1_elevation, (float)pos1_longitude * 10), Vector3.up);
/*
        var LatDiff = LatitudeNew - Latitude;
        var LonDiff = LongitudeNew - Longitude;
        var EleDiff = ElevationNew - Elevation;
        transform.Rotate*/

//        transform.Rotate(0, 180f, 0, Space.Self);

        Debug.Log("------------------------------------------");
        Debug.Log("LAT\t" + Latitude);
        Debug.Log("LON\t" + Longitude);
        Debug.Log("ELE\t" + Elevation);
        Debug.Log("---------------------");
        Debug.Log("NEW LAT\t" + LatitudeNew);
        Debug.Log("NEW LON\t" + LongitudeNew);
        Debug.Log("NEW ELE\t" + ElevationNew);

        SetPosition();
    }

    // Update is called once per frame
    void Update()
    {
/*        if ((((float)(DateTime.Now.Ticks) / TimeSpan.TicksPerMillisecond)) - timeStart >= timeToWaitVelocity) {
            // update api

            pos2_longitude = Longitude;
            pos2_latitude = Latitude;
            pos2_elevation = Elevation;
        }*/

        if (!grabbedPos1)
        {
            pos1_latitude = Latitude;
            pos1_longitude = Longitude;
            pos1_elevation = Elevation;
            grabbedPos1 = true;
        }

        // Calculate velocity
        longitudeVelocity = (pos2_longitude - pos1_longitude) / timeWaited;
        latitudeVelocity = (pos2_latitude - pos1_latitude) / timeWaited;
     
        SetPosition(); // todo possibly unnecessary
    }

}
