using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CompassManager : MonoBehaviour
{
    public TMP_Text yaw;
    public TMP_Text comp;
    public TMP_Text compAvg;
    public TMP_Text camRotY;
    public TMP_Text originRotAmount;

    public LineRenderer lineCamera;
    public LineRenderer lineOrigin;
    public int lineLength;

    [Range(10, 60)]
    public int maxCompassInitChecks;

    private int compassIter = 0;
    private float[] lastCompassReads;
    private float lastRaw = 0;
    private float lastAvg = 0;
    private bool originAnchored = false;
    private float originRotatedAmount = 0;

    public float GetCompassAverage()
    {
        return lastAvg;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Enable compass
        Input.gyro.enabled = true;
        Input.compass.enabled = true;

        // Initialize
        lastCompassReads = new float[maxCompassInitChecks];
        InitDebugLines();
    }

    private void InitDebugLines()
    {
        lineOrigin.startColor = Color.yellow;
        lineOrigin.endColor = Color.yellow;
        lineOrigin.startWidth = 50f;
        lineOrigin.endWidth = 50f;
        lineOrigin.useWorldSpace = true;

        lineCamera.startColor = Color.blue;
        lineCamera.endColor = Color.blue;
        lineCamera.startWidth = 50f;
        lineCamera.endWidth = 50f;
        lineCamera.useWorldSpace = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCompassList();
        UpdateCompassAverage();
        AnchorSessionOrigin();
        UpdateDebugMenuInfo();
        UpdateDebugLines();
    }

    private void UpdateDebugLines()
    {
        // Move the line's starting point away a little bit so it's visible
        Vector3 originInit = transform.parent.forward * 1000;
        Vector3 cameraInit = transform.forward * 1000;
        Vector3 originLookingAt = transform.parent.forward * lineLength;
        Vector3 cameraLookingAt = transform.forward * lineLength;

        // Apply a slight angle so the line is visible pointing directly
        // away from the camera
        originLookingAt.y += lineLength * (float) 0.05;
        cameraLookingAt.y += lineLength * (float) 0.05;

        lineOrigin.SetPosition(0, originInit);
        lineOrigin.SetPosition(1, originLookingAt);
        lineCamera.SetPosition(0, cameraInit);
        lineCamera.SetPosition(1, cameraLookingAt);
    }

    private void UpdateDebugMenuInfo()
    {
        comp.text = "Compass: " + lastRaw;
        yaw.text = "Gyro (Yaw): " + Input.gyro.attitude.eulerAngles.x;
        compAvg.text = "Comp Avg: " + lastAvg;
        camRotY.text = "Cam Rot (Y): " + transform.parent.rotation.y;
        originRotAmount.text = "Origin Rot Amount: " + originRotatedAmount;
    }

    private void AnchorSessionOrigin()
    {
        // Rotation of the parent transform (AR Session Origin) will rotate the
        // camera successfully. Call it once after the averages come in.
        // It will anchor the "origin camera"
        // to the correct position, which anchors all airports and airplanes to
        // the correct world orientation position. Only do this once.
        //
        // The compass may need a small amount of time to initialize. Wait until
        // the last compass reading is not the default value.
        if (!originAnchored && lastRaw != 0)
        {
            transform.parent.rotation = Quaternion.Euler(new Vector3(0f, 180 + lastRaw, 0f));
            originAnchored = true;
            originRotatedAmount = lastRaw;
        }
    }

    private void UpdateCompassList()
    {
        // Update the compass readings list
        lastRaw = Input.compass.trueHeading;
        lastCompassReads[compassIter % maxCompassInitChecks] = lastRaw;
        compassIter += 1;
        // If compassIter is too large, reset it
        if (compassIter >= int.MaxValue) { compassIter = 0; }
    }

    private void UpdateCompassAverage()
    {
        // Get the average compass reading
        float sum = 0;
        for (int i = 0; i < lastCompassReads.Length; i++) { sum += lastCompassReads[i]; }
        lastAvg = sum / lastCompassReads.Length;
    }
}
