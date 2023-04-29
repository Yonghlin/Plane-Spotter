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

    private bool showTrajectoryLine = false;

    [Range(1,5)]
    public float waitTimeBeforeInstantiation;

    public float rotationalSpeed;

    public GPS gps;
    public DataConverter converter;
    public float distance_multiplier;
    public float elevation_multiplier;

    public string getCode()
    {
        return Code;
    }

    public bool isShowingTrajectoryLine()
    {
        return showTrajectoryLine;
    }

    public void setShowingTrajectoryLine(bool showTrajectoryLine)
    {
        this.showTrajectoryLine = showTrajectoryLine;
    }

    public string getElevation()
    {
        return Elevation.ToString();
    }

    public string getName()
    {
        return Name;
    }

    public string getLongitude()
    {
        return Longitude.ToString();
    }

    public string getLatitude()
    {
        return Latitude.ToString();
    }

    private void SetPosition()
    {
        Vector3 loc1 = converter.GeoToCartesian(
            (float) Longitude,
            (float) Elevation,
            (float) Latitude);
        Vector3 loc2 = converter.GeoToCartesian(
            (float) gps.getLongitude(),
            (float) gps.getAltitude(),
            (float) gps.getLatitude());
        Vector3 posNew = loc2 - loc1;

        // Since values are given in feet, they are massive, and the objects
        // will be too tiny to see after their distance and scale is bound.
        // Scale them down here.
        posNew.x *= 0.001f;
        posNew.y *= 0.001f;
        posNew.z *= 0.001f;

        PositionBindManager posManager = this.GetComponent<PositionBindManager>();
        posManager.SetBoundPosAndScale(this.gameObject, posNew);
/*
        Vector3 posNew = converter.ConvertToCartesian(
            distance_multiplier * (float)(Latitude - gps.getLatitude()),
            elevation_multiplier * (float)(Elevation - gps.getAltitude()),
            distance_multiplier * (float)(Longitude - gps.getLongitude()));
        
        PositionBindManager posManager = this.GetComponent<PositionBindManager>();
        posManager.SetBoundPosAndScale(this.gameObject, posNew);*/
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // rotate the cubes for aesthetics
        transform.Rotate(new Vector3(0, rotationalSpeed, 0), Space.Self);

        SetPosition(); // todo possibly unnecessary
    }

}

