using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;

public class Flight
{
    public string ident;
    public string fa_flight_id;
    public Origin origin;
    public Destination destination;
    public LastPosition last_position;
    public string ident_prefix;
}

public class Origin
{
    public string code;
    public string name;
    public string city;
    public string airport_info_url;
}

public class Destination
{
    public string code;
    public string name;
    public string city;
    public string airport_info_url;
}

public class LastPosition
{
    public string fa_flight_id;
    public int altitude;
    public int groundspeed;
    public int heading;
    public double latitude;
    public double longitude;
}

public class Links
{
    public string next;
}

public class RootObject
{
    public Links links;
    public int num_pages;
    public List<Flight> flights;
}



public class FlightManager : MonoBehaviour
{
    private List<Flight> flights;

    private List<GameObject> planePointers = new List<GameObject>();

    public GameObject planeBaseObject;

    public GPS GPS;

    public int flightSearchRadius;
    public int secondsPerUpdate;
    private long lastUpdatedMillis;

    public string ApiKey;

    private string httpResult;

    public List<Airplane> airplanes = new List<Airplane>();

    // Start is called before the first frame update
    void Start()
    {
        lastUpdatedMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        StartCoroutine(GetFlightsFromFA());
    }

    // Update is called once per frame
    void Update()
    {
        // if it's been enough seconds for the next update, update
        long currentTimeMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        long millisElapsed = currentTimeMillis - lastUpdatedMillis;
        long millisPerUpdate = secondsPerUpdate * 1000;

        if (millisElapsed > millisPerUpdate)
        {
            lastUpdatedMillis = currentTimeMillis;

            // every time we update the plane positions, we recreate the game objects
            // so we only need to do 1 API call
            // var airplanesInScene = GameObject.FindGameObjectsWithTag("Airplane");
            /*foreach (GameObject airplane in airplanesInScene)
            {
                Destroy(airplane);
            }*/
            // respawn the planes with updated positions
            StartCoroutine(GetFlightsFromFA());
        }
    }



    public double query;

    IEnumerator GetFlightsFromFA()
    {
        double minLat = /*GPS.getLatitude();//*/40.0506496 - flightSearchRadius;
        double minLon = /*GPS.getLongitude(); //*/-77.5275351 - flightSearchRadius;
        double maxLat = /*GPS.getLatitude();//*/40.0506496 + flightSearchRadius;
        double maxLon = /*GPS.getLongitude(); //*/-77.5275351 + flightSearchRadius;
        using (UnityWebRequest request = UnityWebRequest.Get("https://aeroapi.flightaware.com/aeroapi/flights/search?" +
                    "query=-latlong+%22" +
                    minLat.ToString() + "+" +
                    minLon.ToString() + "+" +
                    maxLat.ToString() + "+" +
                    maxLon.ToString() + "%22"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("x-apikey", ApiKey);
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("FLIGHTMANAGER: Successfully got text");
                var text = request.downloadHandler.text;
                // Assuming the JSON string is stored in a variable named jsonString
                Debug.Log("About to parse JSON");
                //RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(text);
                RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(text);
                Debug.Log("num planes: " + rootObject.flights.Count);
                flights = rootObject.flights;


                List<Airplane> planesToRemove = new List<Airplane>();
                foreach (Airplane a in airplanes)
                {
                    bool wasInNew = false;
                    List<Flight> toRemove = new List<Flight>();

                    foreach (Flight newFlight in flights)
                    {
                        if (a.Code == newFlight.origin.code)
                        {
                            a.UpdatePosition(newFlight.last_position.altitude, (float)newFlight.last_position.latitude, (float)newFlight.last_position.longitude);
                            wasInNew = true;
                            // "flights" is the list of NEW planes. if the plane already exists in our old planes, there's no need
                            // to keep it in the "flights" list, as there is no need to spawn it again, and everything in "flights"
                            // will be spawned after this.
                            //
                            // flights.Remove(newFlight);
                            toRemove.Add(newFlight);
                        }
                    }

                    foreach (Flight f in toRemove)
                    {
                        flights.Remove(f);
                    }

                    if (!wasInNew)
                    {
                        // remove from the scene and the list since it's not in the updated data
                        planesToRemove.Add(a);
                        Destroy(a.transform.gameObject);
                    }
                }

                foreach (Airplane a in planesToRemove)
                {
                    airplanes.Remove(a);
                }
                SpawnPlanes(flights);

                yield return flights;
            }
        }
    }

    public void SpawnPlanes(List<Flight> flights)
    {
        foreach (Flight plane in flights)
        {
            Debug.Log("Spawning plane: " + plane.ident);
            GameObject planeobject = Instantiate(planeBaseObject);
            Airplane ap = planeobject.GetComponent<Airplane>();
            ap.Elevation = plane.last_position.altitude;
            ap.Latitude = plane.last_position.latitude;
            ap.Longitude = plane.last_position.longitude;
            ap.Name = plane.origin.name;
            ap.Code = plane.origin.code;
            ap.PlaneId = plane.ident;

          
            planeobject.SetActive(true);
            // for each plane that needs to be spawned, add it to our current list of airplanes
            airplanes.Add(planeobject.GetComponent<Airplane>());
        }
    }
}


