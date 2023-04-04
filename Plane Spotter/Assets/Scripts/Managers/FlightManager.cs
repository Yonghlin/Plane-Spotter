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

    public int flightSearchOffset;

    public string ApiKey;

    private string httpResult;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetFlightsFromFA());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public double query;
    public double lat;
    public double lon;
    public double otherlat;
    public double otherlon;

    IEnumerator GetFlightsFromFA()
    {
        lat = GPS.getLatitude();
        lon = GPS.getLongitude();
        //otherlat = lat + flightSearchOffset;
        //otherlon = lon + flightSearchOffset; 
        using (UnityWebRequest request = UnityWebRequest.Get("https://aeroapi.flightaware.com/aeroapi/flights/search?" +
                    "query=-latlong+%22" +
                    lat.ToString() + "+" +
                    lon.ToString() + "+" +
                    otherlat.ToString() + "+" +
                    otherlon.ToString() + "%22"))
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
        }
    }
}


