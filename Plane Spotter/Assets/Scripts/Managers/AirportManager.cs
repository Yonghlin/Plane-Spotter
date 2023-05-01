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
using TMPro;

//public class AirportData
//{
//    public string Code { get; set; }
//    public string Name { get; set; }
//    public double Elevation { get; set; }
//    public double Longitude { get; set; }
//    public double Latitude { get; set; }

//    public AirportData(string code, string name, double elevation, double longitude, double latitude)
//    {
//        this.Code = code;
//        this.Name = name;
//        this.Elevation = elevation;
//        this.Longitude = longitude;
//        this.Latitude = latitude;
//    }
//}

[Serializable]
public struct AirportData
{
    public string airport_code;
    public string alternate_ident;
    public string name;
    public double elevation;
    public string city;
    public string state;
    public double latitude;
    public double longitude;
    public string timezone;
    public string country_code;
    public string wiki_url;
    public string airport_flights_url;
    public double distance;
    public double heading;
    public string direction;
}

[Serializable]
public class AirportSet
{
    public List<AirportData> airports = new List<AirportData>();
}

public class AirportManager : MonoBehaviour
{
    private AirportSet airports = new AirportSet();
    private List<GameObject> airportPointers = new List<GameObject>();
    private string httpResult;

    private double lat;
    private double lon;

    public GameObject arrow;
    public GameObject AirportBaseObject;
    public GPS gps;
    public string ApiKey;
    public double Radius;

    public CacheManager cacheManager; 
    public TMP_Text dataMethod; 

    // Start is called before the first frame update
    void Start()
    {
        lat = gps.getLatitude();
        lon = gps.getLongitude();

        if(cacheManager.hasCachedData() && cacheManager.checkAirportValidityStatus((float)lat, (float)lon))
        {
            dataMethod.text = "Using Cached Data";
            StartCoroutine(getCachedAirportData());
        }
        else {
            dataMethod.text = "Using API";
            StartCoroutine(GetAirportsFromFA());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AirportSet GetAirports()
    {
        return airports;
    }

    public int GetNumAirports()
    {
        return airports.airports.Count;
    }

    IEnumerator GetAirportsFromFA()
    {
        string call = "https://aeroapi.flightaware.com/aeroapi/airports/nearby?" +
                        "latitude=" + lat.ToString() +
                        "&longitude=" + lon.ToString() +
                        "&radius=" + Radius;
        using (UnityWebRequest request = UnityWebRequest.Get(call))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("x-apikey", ApiKey);
            yield return request.SendWebRequest();
            if(request.isHttpError || request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("AIRPORTMANAGER: Successfully got text");
                var text = request.downloadHandler.text;
                Debug.Log($"{text}");
                httpResult = text;
                cacheManager.saveCurrentAirportState(text, (float)lat, (float)lon);
                airports = JsonUtility.FromJson<AirportSet>(text);
                Debug.Log(GetNumAirports());
                SpawnAirports();
                yield return airports;
            }
        }
    }

    private IEnumerator getCachedAirportData()
    {
        String CachedJSON = cacheManager.getCurrentlyCachedAirportData();
        airports = JsonUtility.FromJson<AirportSet>(CachedJSON);
        Debug.Log(GetNumAirports());
        SpawnAirports();

        yield return airports;
    }

    public void SpawnAirports()
    {
        foreach (AirportData airport in airports.airports)
        {
            Debug.Log("Spawning airport: " + airport.name);
            GameObject na = Instantiate(AirportBaseObject);
            Airport ap = na.GetComponent<Airport>();
            ap.Elevation = airport.elevation;
            ap.Latitude = airport.latitude;
            ap.Code = airport.airport_code;
            ap.Longitude = airport.longitude;
            ap.Name = airport.name;

            na.SetActive(true);
        }
    }
}
