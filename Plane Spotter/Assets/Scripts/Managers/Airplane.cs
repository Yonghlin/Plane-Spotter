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

    private double longitudePrev;
    private double latitudePrev;
    private double altitudePrev;

    public GPS gps;
    public GeoConverter converter;
    public float distance_multiplier;
    public float elevation_multiplier;

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
    private void UpdateStoredCoordinates(float longitude, float altitude, float latitude)
    {
        longitudePrev = Longitude;
        altitudePrev = Elevation;
        latitudePrev = Latitude;

        Longitude = longitude;
        Elevation = altitude;
        Latitude = latitude;
    }

    public void UpdatePosition(float ElevationNew, float LatitudeNew, float LongitudeNew)
    {
        UpdateStoredCoordinates(LongitudeNew, ElevationNew, LatitudeNew);

        Vector3 pos1 = converter.GeoToCartesian((float) longitudePrev, (float) altitudePrev, (float) latitudePrev);
        Vector3 pos2 = converter.GeoToCartesian((float) Longitude, (float) Elevation, (float) Latitude);
        Vector3 dir = (pos2 - pos1);

        transform.LookAt(dir);
        transform.Rotate(0, 180, 0, Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        SetPosition();
    }

}
