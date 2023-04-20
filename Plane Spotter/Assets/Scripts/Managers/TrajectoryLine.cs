using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrajectoryLine : MonoBehaviour
{
    public Toggle showTrajectoryLine;

    private Transform airportTransform;
    private LineRenderer lineRenderer;
    private List<Transform> flightTransforms = new List<Transform>();
    private float range = 10000.0f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // hierarchy: AirportPrefab -> LineRenderer -> this script
        // get the AirportPrefab transform
        airportTransform = transform.parent;

        // start listening for when the user checks the box
        showTrajectoryLine.onValueChanged.AddListener(delegate
        {
            if (showTrajectoryLine.isOn)
            {
                // Tell the AirportFlights script to query for incoming/outgoing flight info
                AirportFlights flights = GetComponentInParent<AirportFlights>();
                StartCoroutine(flights.GetAirportFlightsFromFA());
            }
        });

    }

    private void Update()
    {
        // this script is attached to AirportPrefab, so the airport transform is just that
        if (airportTransform == null || flightTransforms.Count == 0)
        {
            lineRenderer.enabled = false;
            return;
        }

        // if the user toggled the trajectoryline on in the popup menu
        if (showTrajectoryLine.isOn)
        {
                        // Find the closest flight to the airport
            Transform closestFlight = null;
            float closestDistance = float.MaxValue;
            foreach (Transform flightTransform in flightTransforms)
            {
                // todo change to latitude / altitude
                double latAirport = transform.parent.GetComponent<Airport>().Latitude;
                double lonAirport = transform.parent.GetComponent<Airport>().Longitude;

                //
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
    }

    public void AddFlightTransform(Transform flightTransform)
    {
        flightTransforms.Add(flightTransform);
    }
}