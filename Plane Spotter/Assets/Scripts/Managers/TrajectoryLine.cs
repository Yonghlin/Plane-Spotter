using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrajectoryLine : MonoBehaviour
{
    public TMPro.TMP_Text airportNameInPopupMenu;
    public Toggle showTrajectoryLineInPopupMenu;

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
        showTrajectoryLineInPopupMenu.onValueChanged.AddListener(delegate
        {
            if (showTrajectoryLineInPopupMenu.isOn)
            {
                // nasty.
                // showTrajectoryLine isn't unique, and there's a listener for every airport trajectoryline object.
                // we only want to call the API once, so check if the airport's name in the popup menu
                // matches the airport object's name
                if (airportNameInPopupMenu.text == GetComponentInParent<Airport>().Name)
                {
                    // Set the airport's toggled value
                    GetComponentInParent<Airport>().setShowingTrajectoryLine(true);

                    // Tell the AirportFlights script to query for incoming/outgoing flight info
                    AirportFlights flights = GetComponentInParent<AirportFlights>();
                    StartCoroutine(flights.GetAirportFlightsFromFA());

                    //This will grab all the Airplanes in the scene
                    var airplanesInScene = GameObject.FindGameObjectsWithTag("Airplane");
                    // This will compare all the airplanes in the scene with the incoming airplanes to the airport
                    // and check if airportFlightData has the airplane and it will add it to flightTransform
                    flightTransforms.Clear();
                    foreach (GameObject airplane in airplanesInScene)
                    {
                        foreach (AirportFlightArrival arrival in flights.airportFlightData.arrivals)
                        {
                            if (airplane.GetComponent<Airplane>().Name == arrival.name)
                            {
                                flightTransforms.Add(airplane.transform);
                            }
                        }
                    }
                }
            }
            else
            {
                // nasty.
                // showTrajectoryLine isn't unique, and there's a listener for every airport trajectoryline object.
                // we only want to call the API once, so check if the airport's name in the popup menu
                // matches the airport object's name
                if (airportNameInPopupMenu.text == GetComponentInParent<Airport>().Name)
                {
                    // Set the airport's toggled value
                    GetComponentInParent<Airport>().setShowingTrajectoryLine(false);
                }
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

        // if the airport trajectory line is enabled, which is set by the user checking the checkbox
        // in the popup menu
        if (GetComponentInParent<Airport>().isShowingTrajectoryLine())
        {
            // Find the closest flight to the airport
            // make starting value very high to allow for actual distances to compare as closer
            double closestDistance = 10000.0;
            Transform closestFlight = null;
            double latAirport = transform.parent.GetComponent<Airport>().Latitude;
            double lonAirport = transform.parent.GetComponent<Airport>().Longitude;
            foreach (Transform flightTransform in flightTransforms)
            {
                Airplane planeObj = flightTransform.gameObject.GetComponent<Airplane>();
                double currentDistance = Mathf.Sqrt(Mathf.Pow((float) (planeObj.Latitude - latAirport), 2) + Mathf.Pow((float) (planeObj.Longitude - lonAirport), 2));

                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
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
}
