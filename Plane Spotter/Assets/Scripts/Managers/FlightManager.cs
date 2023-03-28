using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

using gps = GPS;

public struct Last_Position
{
    public string fa_flight_id;
    public double altitude;
    public string altitude_change;
    public double groundspeed;
    public double heading;
    public double latitude;
    public double longitude;
    public string timestamp;
}

public struct Origin
{
    public string code;
    public string code_icao;
    public string code_iata;
    public string code_lid;
    public string timezome;
    public string name;
    public string city;
    public string airport_info_url;
}

public struct Destination
{
    public string code;
    public string code_icao;
    public string code_iata;
    public string code_lid;
    public string timezome;
    public string name;
    public string city;
    public string airport_info_url;
}

public struct FlightData
{
    public string ident;
    public string ident_icao;
    public string ident_iata;
    public string fa_flight_id;
    public string actual_off;
    public string actual_on;
    public bool foresight_predictions_available;

    public Last_Position last_position;
    public Origin origin;
    public string destination;
}

public class FlightSet
{
    public List<FlightData> planes = new List<FlightData>();
}

public class FlightManager : MonoBehaviour
{
    private FlightSet planes = new FlightSet();

    private List<GameObject> planePointers = new List<GameObject>();

    public GameObject planeBaseObject;

    public gps GPS;

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

    public int GetNumPlanes()
    {
        return planes.planes.Count;
    }

    public double query;
    public double lat;
    public double lon;
    public double otherlat;
    public double otherlon;

    IEnumerator GetFlightsFromFA()
    {
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
                Debug.Log($"{text}");
                httpResult = text;
                planes = JsonUtility.FromJson<FlightSet>(text);
                Debug.Log(GetNumPlanes());
                SpawnPlanes();
                yield return planes;
            }
        }
    }

    public void SpawnPlanes()
    {
        foreach (FlightData plane in planes.planes)
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


