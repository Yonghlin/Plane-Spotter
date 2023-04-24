using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompassManager : MonoBehaviour
{
    public TMP_Text yaw;
    public TMP_Text comp;
    public TMP_Text compAvg;

    [Range(10, 60)]
    public int maxCompassInitChecks;

    private int compassIter = 0;
    private float[] lastCompassReads;
    private float lastAvg = 0;

    public float GetCompassAverage()
    {
        return lastAvg;
    }

    // Start is called before the first frame update
    void Start()
    {
        Input.gyro.enabled = true;
        Input.compass.enabled = true;

        lastCompassReads = new float[maxCompassInitChecks];
    }

    // Update is called once per frame
    void Update()
    {
        yaw.text = "Cam Rot (Yaw): " + Input.gyro.attitude.eulerAngles.x.ToString();
        comp.text = "Compass: " + Input.compass.magneticHeading.ToString(); 

        // Update the compass readings list
        float heading = Input.compass.magneticHeading;
        lastCompassReads[compassIter % maxCompassInitChecks] = Input.compass.magneticHeading;
        compassIter += 1;
        if (compassIter >= int.MaxValue) { compassIter = 0; }

        // Get the average compass reading
        float sum = 0;
        for (int i = 0; i < lastCompassReads.Length; i++) { sum += lastCompassReads[i]; }
        lastAvg = sum / lastCompassReads.Length;
        compAvg.text = "Comp Avg: " + lastAvg.ToString();
    }
}
