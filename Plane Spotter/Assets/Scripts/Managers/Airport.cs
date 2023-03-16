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
    public GPS gps;

    private float ElevationFloat;
    private float LongitudeFloat; 
    private float LatitudeFloat;

    // Start is called before the first frame update
    void Start()
    {
        // set initial location here
        ElevationFloat = (float)Elevation;
        LongitudeFloat = (float)Longitude;
        LatitudeFloat = (float)Latitude;

        //obj.transform.position.Set(LatitudeFloat, ElevationFloat, LongitudeFloat);
    }

    // Update is called once per frame
    void Update()
    {
        double PlayerLatitude = gps.getLatitude();
        double PlayerLongitude = gps.getLongitude();
        double PlayerAltitude = gps.getAltitude();

        float x = (float) (Latitude - PlayerLatitude);
        float y = (float) (PlayerAltitude - PlayerAltitude);
        float z = (float) (Longitude - PlayerLongitude);

        transform.position = new Vector3(x * 1000, y, z * 1000);

      //  transform.localScale = new Vector3(
      //      1000 * Mathf.Abs(x*z * transform.localScale.x),
      //      1000 * Mathf.Abs(z*x * transform.localScale.y),
      //      1000 * Mathf.Abs(z*x * transform.localScale.z)
      //  );



    }
}
