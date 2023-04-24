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
    private bool showTrajectoryLine = false;

    // GPS
    [Range(1,5)]
    public float waitTimeBeforeInstantiation;
    [Range(10, 60)]
    public int maxCompassInitChecks;
    [Range(0, 2)]
    public float rotationalSpeed;

    private int compassIter = 0;
    private float[] lastCompassReads;
    
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
        lastCompassReads = new float[maxCompassInitChecks];
        StartCoroutine(LateStart());

/*        trajectoryLine = GameObject.FindObjectOfType<TrajectoryLine>();
        airportFlights = GetComponent<AirportFlights>();
        airportFlights.GetAirportFlightsFromFA();*/
    }

    // https://answers.unity.com/questions/971957/how-to-initialize-after-start.html
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(waitTimeBeforeInstantiation);
        // any code here to be run AFTER other GameObject's start functions have run
        // without waiting a number of seconds, the objects won't display
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
        // wraps back around to the beginning of the array, updating old values with new ones
        // merge trueHeading with magneticHeading to (hopefully) improve accuracy

        float heading = Input.compass.magneticHeading;
        // averages between 360 and 0 will rotate the objects 180deg. cancel those here.
        //if (heading < 340 && heading > 20)
        //lastCompassReads[(compassIter + 1) % (maxCompassInitChecks - 1)] = Input.compass.trueHeading;
        lastCompassReads[compassIter % maxCompassInitChecks] = Input.compass.magneticHeading;
        compassIter += 1;
        if (compassIter >= int.MaxValue)
        {
            compassIter = 0;
        }

        float sum = 0;
        for (int i = 0; i < lastCompassReads.Length; i++)
        {
            sum += lastCompassReads[i];
        }
        float avg = sum / lastCompassReads.Length;

        // use the average of multiple compass readings to improve accuracy
        // multiply by a constant, as the objects rotate more than they should
        transform.RotateAround(gps.transform.position, Vector3.up, -avg);
    }

/*    public void ShowFlights()
    {
        AirportFlights airportFlights = FindObjectOfType<AirportFlights>();
        if (airportFlights != null)
        {
            airportFlights.GetAirportFlightsFromFA();
        }
    }*/
}


