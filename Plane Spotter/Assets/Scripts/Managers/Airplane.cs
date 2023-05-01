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
    public String DestinationName;
    public String DestinationCity;

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
    public GeoConverter converter;
    public float distance_multiplier;
    public float elevation_multiplier;

    private CompassManager compassManager;

    private void SetPosition()
    {
        PositionBindManager posManager = this.GetComponent<PositionBindManager>();
        Vector3 posNew = posManager.GetRawPosition(
            (float) Longitude,
            (float) Elevation,
            (float) Latitude,
            (float) gps.getLongitude(),
            (float) gps.getAltitude(),
            (float) gps.getLatitude()
        );
        posManager.SetBoundPosAndScale(this.gameObject, posNew);
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

        Vector3 pos1 = converter.GeoToCartesian((float) pos1_longitude, (float) pos1_elevation, (float) pos1_latitude);
        Vector3 pos2 = converter.GeoToCartesian((float) pos2_longitude, (float) pos2_elevation, (float) pos2_latitude);
        Vector3 dir = (pos2 - pos1);

        transform.LookAt(dir);
        transform.Rotate(0, 180, 0, Space.Self);

        SetPosition();
    }

    // Update is called once per frame
    void Update()
    {
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
