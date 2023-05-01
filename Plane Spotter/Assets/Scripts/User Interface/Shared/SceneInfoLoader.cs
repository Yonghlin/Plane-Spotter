using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class SceneInfoLoader : MonoBehaviour
{
    public TMP_Text AirportName;
    public TMP_Text AirportLocation;
    public TMP_Text AirportCode;
    public TMP_Text AirportCoordinates;
    public TMP_Text AirportTimezone;
    public Button wikiButton;
    public TMP_Text buttonLabel;
    public Airport ap;
    private Airport temp;

    public void Start()
    {
        wikiButton.GetComponent<Button>().enabled = false; // remove functionality

        wikiButton.GetComponent<Image>().enabled = false; // remove rendering
        buttonLabel.enabled = false;

        getCurrentData();
        Airport temp = ap.GetComponent<Airport>();  
        //Debug.Log("Test " + ap.GetComponent<Airport>().getName());

        AirportName.text = "Name: " + temp.getName();
        AirportLocation.text = "Location" + temp.getLocation();
        AirportCode.text = "Code: " + temp.getCode();
        AirportCoordinates.text = "Coordinates: (" + temp.getLongitude() + "," + temp.getLatitude() + ")";
        AirportTimezone.text = "Timezone: " + temp.getTimeZone();

        string link = ap.GetComponent<Airport>().getWikiLink();
        if (link.Length > 6)
        {
            wikiButton.GetComponent<Button>().enabled = true; // remove functionality
            wikiButton.GetComponent<Image>().enabled = true; // remove rendering
            buttonLabel.enabled = true;
        }

    }
    private void getCurrentData()
    {
        string objectJSON = PlayerPrefs.GetString("Airport", "No Object");
        Debug.Log(objectJSON);
        JsonUtility.FromJsonOverwrite(objectJSON, ap.GetComponent<Airport>());
        ap.AddComponent<GPS>();
        ap.AddComponent<GeoConverter>();
        ap.GetComponent<Airport>().gps = ap.GetComponent<GPS>();
        ap.GetComponent<Airport>().converter = ap.GetComponent<GeoConverter>();
    }

    public void openWikipediaLink()
    {
        Debug.Log("Clicked button\n");
        Application.OpenURL(ap.GetComponent<Airport>().getWikiLink());
        Debug.Log("Link Opened: " + ap.GetComponent<Airport>().getWikiLink());
    }
}
