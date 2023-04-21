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
        timeStart = (float)((DateTime.Now.Ticks) / TimeSpan.TicksPerMillisecond);
        SetPosition();
        //StartCoroutine(LateStart());
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
        transform.RotateAround(gps.transform.position, Vector3.up, -camYaw);
    }

    // Update is called once per frame
    void Update()
    {
        if ((((float)(DateTime.Now.Ticks) / TimeSpan.TicksPerMillisecond)) - timeStart >= timeToWaitVelocity) {
            pos2_longitude = Longitude;
            pos2_latitude = Latitude;
            pos2_elevation = Elevation;
        }

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
     

        //Debug.Log("distance from player to airport (x): " + transform.position.x);
        //Debug.Log("distance from player to airport (y): " + transform.position.y);
        //Debug.Log("distance from player to airport (z): " + transform.position.z);

    }

}
