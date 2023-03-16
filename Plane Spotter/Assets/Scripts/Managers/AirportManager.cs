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
    public string Code;
    public string Name;
    public double Elevation;
    public double Latitude;
    public double Longitude;
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

    public GameObject arrow;

    public gps Gps;

    public GameObject AirportBaseObject;

    public string ApiKey;

    public double Radius;


    public double lat;
    public double lon;
    public double altitude;

    private string httpResult;
    // Start is called before the first frame update
    void Start()
    {

        GetAirportsFromFA();

        
        
        foreach(AirportData airport in airports.airports)
        {
            GameObject na = Instantiate(AirportBaseObject);
            Airport ap = na.GetComponent<Airport>();
            ap.Elevation = airport.Elevation;
            ap.Latitude = airport.Latitude;
            ap.Code = airport.Code;
            ap.Longitude = airport.Longitude;
            ap.Name = airport.Name;

            na.SetActive(true);
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

    private AirportSet GetAirportsFromFA()
    {
        StartCoroutine(GetText());
        return JsonUtility.FromJson<AirportSet>(httpResult);
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://aeroapi.flightaware.com/aeroapi/airports/nearby?" +
                        "latitude=" + lat.ToString() +
                        "&longitude=" + lon.ToString() +
                        "&radius=" + Radius);
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("x-apikey", ApiKey);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
            httpResult = www.downloadHandler.text;
        }
    }
}
