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
    // todo possible feature:
    // also show outgoing planes

    public struct arrivals
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
    }
}


public class AirportFlights : MonoBehaviour
{
    // API to use to display flights to and from an airport:
    // https://flightaware.com/aeroapi/portal/documentation#get-/airports/-id-/flights

    public string ApiKey;
    
    private string httpResult;

    private List<AirportFlightData> airportFlightList = new List<AirportFlightData>();

    [Range(0, 7)]
    public int daysUntilNowToGrabAirportFlights;
    [Range(0, 2)]
    public int daysFromNowToGrabAirportFlights;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    int GetNumAirportFlights()
    {
        return airportFlightList.Count;
    }

    public IEnumerator GetAirportFlightsFromFA()
    {
        Debug.Log("Function was called.");
        // get airport code from Airport.cs, attached to this script's GameObject
        string airportCode = gameObject.GetComponent<Airport>().Code;

        // get start/end dates to grab airport flight info
        string dateStart = DateTime.Now.AddDays(-daysUntilNowToGrabAirportFlights).ToString("yyyy'-'MM'-'dd");
        string dateEnd = DateTime.Now.AddDays(daysFromNowToGrabAirportFlights).ToString("yyyy'-'MM'-'dd");
        Debug.Log(dateStart);
        Debug.Log(dateEnd);

        string call = "https://aeroapi.flightaware.com/aeroapi/airports/" + "BWI" + "/flights?" +
                        "type=General_Aviation" +
                        "&start=" + dateStart + // todo nasty hardcode :(
                        "&end=" + dateEnd;
        Debug.Log(call);

        // grab API stuff to put into AirportFlightData struct
        using (UnityWebRequest request = UnityWebRequest.Get(call))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("x-apikey", ApiKey);
            yield return request.SendWebRequest();
            /*request.SendWebRequest();*/
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("AIRPORTFLIGHTS: Successfully got text");
                var text = request.downloadHandler.text;
                Debug.Log(text);

                // todo add tmptexts for this?
                // airportFlightList = JsonUtility.FromJson<List<AirportFlightData>>(text);
                Newtonsoft.Json.JsonConvert.DeserializeObject(text);

                Debug.Log("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------Number of airport flights for "
                    + airportCode
                    + ": " + GetNumAirportFlights());

                Debug.Log(text);
                yield return airportFlightList;
            }
        }
    }
}