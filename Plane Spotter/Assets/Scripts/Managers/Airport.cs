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
    [Range(1,5)]
    public float waitTimeBeforeInstantiation;
    [Range(10, 60)]
    public int maxCompassInitChecks;

    private int compassIter = 0;
    private float[] lastCompassReads;
    
    public GPS gps;
    public float distance_multiplier;
    public float elevation_multiplier;

    public string getCode()
    {
        return Code;
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
        // set the bound position
        //posManager.SetBoundPosAndScale(this.gameObject, unityCoords);
        // TODO set this back
        transform.position = unityCoords;
    }

    // Start is called before the first frame update
    void Start()
    {
        lastCompassReads = new float[maxCompassInitChecks];
        StartCoroutine(LateStart());
    }

    // https://answers.unity.com/questions/971957/how-to-initialize-after-start.html
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(waitTimeBeforeInstantiation);
        // any code here to be run AFTER other GameObject's start functions have run
        // without waiting a number of seconds, the objects won't display
        SetPosition();
        // object position in the real world is affected by the direction of
        // the camera, specifically when the app opens. so offset it here

        float camYaw = Input.gyro.attitude.eulerAngles.x;

        if(camYaw >= 180f)
        {
            //ex: 182 = 182 - 360 = -178
            camYaw -= 360;
        }
        transform.RotateAround(gps.transform.position, Vector3.up, -camYaw);

        transform.RotateAround(gps.transform.position, Vector3.up, -Input.compass.trueHeading);

        float sum = 0;
        for (int i = 0; i < lastCompassReads.Length; i++)
        {
            sum += lastCompassReads[i];
        }
        float avg = sum / lastCompassReads.Length;
        // use the average of multiple compass readings to improve accuracy
        transform.RotateAround(gps.transform.position, Vector3.up, -avg);
    }

    // Update is called once per frame
    void Update()
    {
        // ~ the number of iterations for waitTimeBeforeInstantiation seconds
        // to elapse, if the app runs at 60 updates per second
        if (compassIter <= 60 * waitTimeBeforeInstantiation)
        {
            // store multiple values of recent compass data to average them and spawn airports
            // more accurately

            // wraps back around to the beginning of the array, updating old values with new ones
            float compHeading = Input.compass.trueHeading;
            // if the phone is at around 360deg, it can wrap to 0, and both values close to
            // 360 and 0 will be added, skewing the results and putting the airport objects
            // in the wrong positions by 180deg. correct it here by discarding values too close
            if (compHeading < 340 && compHeading > 20)
            {
                lastCompassReads[compassIter % maxCompassInitChecks] = Input.compass.trueHeading;
                compassIter += 1;
            }
        }
        //Debug.Log("distance from player to airport (x): " + transform.position.x);
        //Debug.Log("distance from player to airport (y): " + transform.position.y);
        //Debug.Log("distance from player to airport (z): " + transform.position.z);
    }
}


