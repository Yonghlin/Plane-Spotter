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

    public string FlightAwareKey;

    public double Radius;

    // Start is called before the first frame update
    void Start()
    {

        airports = GetAirportsFromFA(FlightAwareKey, 40.0506496, -77.5275351, 50);
        
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

    private AirportSet GetAirportsFromFA(string ApiKey, double lat, double lon, double radius)
    {
        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(
        //            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //        client.DefaultRequestHeaders.Add("x-apikey", ApiKey);

        //        var response = await client.GetAsync("https://aeroapi.flightaware.com/aeroapi/airports/nearby?" +
        //                "latitude=" + lat.ToString() +
        //                "&longitude=" + lon.ToString() +
        //                "&radius=" + radius);
        //        var contentStream = await response.Content.ReadAsStreamAsync();
        //        Debug.Log("RESPONSE CODE: " + response.StatusCode);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            using (StreamReader reader = new StreamReader(contentStream))
        //            {
        //                string jsonstr = reader.ReadToEnd();
        //                Debug.Log(jsonstr);
        //                return JsonUtility.FromJson<AirportSet>(jsonstr);
        //            }
        //        }
        //    }
        //    return null;
        //}
        UnityWebRequest www = UnityWebRequest.Get("https://aeroapi.flightaware.com/aeroapi/airports/nearby?" +
                        "latitude=" + lat.ToString() +
                        "&longitude=" + lon.ToString() +
                        "&radius=" + radius);
        www.SetRequestHeader("x-apikey", ApiKey);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.Send();
        Debug.Log(www.result); Debug.Log(www.error);
        return new AirportSet();
    }

    private IEnumerable getFromServer(string ApiKey, string url)
    {
        UnityWebRequest www = UnityWebRequest.Get("https://aeroapi.flightaware.com/aeroapi/airports/nearby?" +
                        "latitude=" + lat.ToString() +
                        "&longitude=" + lon.ToString() +
                        "&radius=" + radius);
        www.SetRequestHeader("x-apikey", ApiKey);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.Send();
    }
}
