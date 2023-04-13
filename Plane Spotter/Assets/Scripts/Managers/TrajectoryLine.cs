using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform airportTransform;
    private List<Transform> flightTransforms = new List<Transform>();
    private float range = 10000.0f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    private void Update()
    {
        if (airportTransform == null || flightTransforms.Count == 0)
        {
            lineRenderer.enabled = false;
            return;
        }

        // Find the closest flight to the airport
        Transform closestFlight = null;
        float closestDistance = float.MaxValue;
        foreach (Transform flightTransform in flightTransforms)
        {
            float distance = Vector3.Distance(flightTransform.position, airportTransform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestFlight = flightTransform;
            }
        }

        // Draw the line between the airport and the closest flight
        if (closestFlight != null && closestDistance < range)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, airportTransform.position);
            lineRenderer.SetPosition(1, closestFlight.position);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    public void SetAirportTransform(Transform airportTransform)
    {
        this.airportTransform = airportTransform;
    }

    public void AddFlightTransform(Transform flightTransform)
    {
        flightTransforms.Add(flightTransform);
    }
}