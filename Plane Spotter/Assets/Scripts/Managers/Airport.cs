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

    private TrajectoryLine trajectoryLine;
    private AirportFlights airportFlights;
    private CompassManager compassManager;
    private bool showTrajectoryLine = false;

    [Range(1,5)]
    public float waitTimeBeforeInstantiation;

    public float rotationalSpeed;

    public GPS gps;
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
        // convert latitude/longitude to x/y coordinates
        Vector3 unityCoords = new Vector3(
                   distance_multiplier * (float)(Latitude - gps.getLatitude()),
                   elevation_multiplier * (float)Elevation,
                   distance_multiplier * (float)(Longitude - gps.getLongitude()));
        // get the position binding script
        PositionBindManager posManager = this.GetComponent<PositionBindManager>();
        posManager.SetBoundPosAndScale(this.gameObject, unityCoords);
    }

    // Start is called before the first frame update
    void Start()
    {
        compassManager = GameObject.FindGameObjectWithTag("CompassCamera")
                                   .GetComponent<CompassManager>();

        /*trajectoryLine = GameObject.FindObjectOfType<TrajectoryLine>();
        airportFlights = GetComponent<AirportFlights>();
        airportFlights.GetAirportFlightsFromFA();*/
    }


    // Update is called once per frame
    void Update()
    {
        // rotate the cubes for aesthetics
        transform.Rotate(new Vector3(0, rotationalSpeed, 0), Space.Self);

        // reset object position before rotating it around the camera
        SetPosition();
        // object position in the real world is affected by the direction of
        // the camera, specifically when the app opens. so offset it here
        // store multiple values of recent compass data to average them and spawn airports
        // more accurately
        // float compAvg = compassManager.GetCompassAverage();
        // transform.RotateAround(gps.transform.position, Vector3.up, -compAvg);
    }

}

