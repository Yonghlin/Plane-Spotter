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
    public GeoConverter converter;
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

    // Update is called once per frame
    void Update()
    {
        // rotate the cubes for aesthetics
        transform.Rotate(new Vector3(0, rotationalSpeed, 0), Space.Self);

        SetPosition(); // todo possibly unnecessary
    }

}

