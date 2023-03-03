using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

using gps = GPS;
public class Airport
{
    public string Code { get; set; }
    public string Name { get; set; }
    public double Elevation { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }

    public Airport(string code, string name, double elevation,  double longitude, double latitude)
    {
        this.Code = code;
        this.Name = name;
        this.Elevation = elevation;
        this.Longitude = longitude;
        this.Latitude = latitude;
    }
}

public class AirportSet
{
    public List<Airport> airports = new List<Airport>();
    
    public void addAirport(Airport airport)
    {
        airports.Add(airport);
    }

    public List<Airport> GetAirports() { return airports; }

}

public class AirportManager : MonoBehaviour
{
    private AirportSet airports = new AirportSet();

    private List<GameObject> airportPointers = new List<GameObject>();

    public GameObject arrow;

    public gps Gps;


    // Start is called before the first frame update
    void Start()
    {
        airports.addAirport(new Airport("N68", "Franklin County Rgnl", 687.9, -77.6432563, 39.9729617));
        airports.addAirport(new Airport("W05", "Gettysburg Rgnl", 553.2, -77.27465, 39.8413092));
        airports.addAirport(new Airport("N94", "Carlisle", 510.1, -77.1742736, 40.1879144));
        airports.addAirport(new Airport("W73", "Mid Atlantic Soaring Center", 573, -77.3513761, 39.75704));
        airports.addAirport(new Airport("KHGR", "Hagerstown Rgnl", 703.1, -77.7265, 39.7085));
        airports.addAirport(new Airport("07N", "Bermudian Valley Airpark", 470, -77.0038667, 40.0167617));
        airports.addAirport(new Airport("KTVH", "York", 494.7, -76.8730278, 39.917));
        airports.addAirport(new Airport("0P8", "Lazy B Ranch", 476, -76.8153, 40.0244236));
        airports.addAirport(new Airport("KCXY", "Capital City", 346.7, -76.8513611, 40.2171389));
        airports.addAirport(new Airport("P34", "Mifflintown", 545, -77.4056667, 40.5989444));
        airports.addAirport(new Airport("KDMW", "Carroll Co Rgnl", 789.2, -77.0076667, 39.6082778));
        airports.addAirport(new Airport("KMDT", "Harrisburg Intl", 310, -76.7626192, 40.1931917));
        airports.addAirport(new Airport("W35", "Potomac Airpark", 412.5, -78.1660833, 39.6926111));
        airports.addAirport(new Airport("P17", "Greater Breezewood Rgnl", 1345, -78.2977333, 39.8742681));

        foreach(Airport airport in airports.airports)
        {
            Instantiate(arrow, new Vector3((float)airport.Latitude - (float) Gps.getLatitude(), (float)airport.Elevation - (float) Gps.getAltitude(), (float)airport.Longitude - (float) Gps.getLongitude()), Quaternion.Euler(90, 0, 0));
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
}
