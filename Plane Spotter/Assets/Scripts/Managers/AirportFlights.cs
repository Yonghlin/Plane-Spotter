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


[Serializable]
public struct AirportFlightData
{
    public string flight_ident;
    public string flight_number;
    public string flight_registration;
    public string aircraft_type;

    // airport origin information
    public string code;
    public string timezone;
    public string name;
    public string city;

    // todo possible feature:
    // also show outgoing planes
}


public class AirportFlights : MonoBehaviour
{
    // API to use to display flights to and from an airport:
    // https://flightaware.com/aeroapi/portal/documentation#get-/airports/-id-/flights

    public string ApiKey;
    
    private string httpResult;
    private AirportFlightData flightsToAirport;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetAirportFlightsFromFA());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator GetAirportFlightsFromFA()
    {
        // get airport code from Airport.cs, attached to this script's GameObject
        string airportCode = gameObject.GetComponent<Airport>().Code;

        // TODO pass in a date

        // grab API stuff to put into AirportFlightData struct
        using (UnityWebRequest request = UnityWebRequest.Get("https://aeroapi.flightaware.com/aeroapi/airports/" + airportCode + "/flights?" +
                        "type=General_Aviation" +
                        "&start=2023-04-06" + // todo nasty hardcode :(
                        "&end=2023-04-07"))
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
                Debug.Log("AIRPORTFLIGHTS: Successfully got text");
                var text = request.downloadHandler.text;
                Debug.Log($"{text}");
                httpResult = text;

                // todo might need to make an AirportFlightSet that holds multiple AirportFlghtDatas
                flightsToAirport = JsonUtility.FromJson<AirportFlightData>(text);
                yield return flightsToAirport;
            }
        }
    }

}
