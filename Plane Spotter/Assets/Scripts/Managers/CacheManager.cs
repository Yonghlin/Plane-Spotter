using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CacheManager : MonoBehaviour
{

    public int validLimit;
    public int latRange;
    public int lonRange;
    //Saves the current airport data along with the users current position
    public void saveCurrentAirportState(string airportListJSON, float curLat, float curLong)
    {
        PlayerPrefs.SetString("AirportDataJSON", airportListJSON);
        PlayerPrefs.SetString("ContainsData", "True");
        PlayerPrefs.SetFloat("PlayerLastLatitude", curLat);
        PlayerPrefs.SetFloat("PlayerLastLongitude", curLong);
        PlayerPrefs.SetInt("currentUse", 0);
    }

    //Checks to see if the recorded airport data is valid based on whether the user has moved too much or
    //
    public bool checkAirportValidityStatus(float curLat, float curLong)
    {
        Debug.Log("Current Use Index: " + PlayerPrefs.GetInt("currentUse", validLimit));
        if (PlayerPrefs.GetInt("currentUse", validLimit)  >=validLimit)
        {
            
            return false;
        }

        float lastLat = PlayerPrefs.GetFloat("PlayerLastLatitude", 0.0f);
        float lastLong = PlayerPrefs.GetFloat("PlayerLastLongitude", 0.0f);

        float latDifference = Mathf.Abs(lastLat) - Mathf.Abs(curLat);
        float longDifference = Mathf.Abs(lastLong) - Mathf.Abs(curLong);
        Debug.Log("Lat Difference: " + latDifference + " Lon difference: " + longDifference);

        if(latDifference <= latRange && longDifference <= lonRange)
        {
            return true;
        }


        PlayerPrefs.SetString("ContainsData", "False");
        return false;

    }

    public string getCurrentlyCachedAirportData()
    {
        int currentUse = PlayerPrefs.GetInt("currentUse", validLimit);
        currentUse++;
        PlayerPrefs.SetInt("currentUse", currentUse);
        return PlayerPrefs.GetString("AirportDataJSON", "No Data Cached");
    }

    public bool hasCachedData()
    {
        return (PlayerPrefs.GetString("ContainsData", "False") == "True");
    }
}
