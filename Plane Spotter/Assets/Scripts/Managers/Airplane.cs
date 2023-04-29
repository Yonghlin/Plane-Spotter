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

    public double Heading;
    public double GroundSpeed;

    private double longitudePrev;
    private double latitudePrev;
    private double altitudePrev;

    public GPS gps;
    public GeoConverter converter;
    public float distance_multiplier;
    public float elevation_multiplier;

    long lastUpdateTime;

    public void UpdateLastUpdateTime()
    {
        lastUpdateTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }

    private float GetFeetTraveled()
    {
        long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        long elapsed = currentTime - lastUpdateTime;

        // Convert the plane's knot speed to fps (feet per second)
        // Convert fps to fpms (feet per millisecond)
        float fps = (float)GroundSpeed * 1.68781f;
        float fpms = fps / 1000;

        // Get number of feet traveled since last API update
        float traveled = fpms * elapsed;
        return traveled;
    }

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

        // Add the feet traveled since the last API update
        // Todo figure out why they move so fast and remove this constant multiplier
        posNew += transform.forward * GetFeetTraveled();

        posNew = posManager.NormalizePosition(posNew);
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
    }

    // Update is called once per frame
    void Update()
    {
        SetPosition();
    }

}
