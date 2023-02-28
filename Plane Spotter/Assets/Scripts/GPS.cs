using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class GPS : MonoBehaviour
{
    public  double longitude{ get; protected set;}
    public  double latitude{ get;protected set;}
    public double altitude {get; protected set;}

    // Start is called before the first frame update
    IEnumerator Start()
    {
         // Starts the location service.
        if(Permission.HasUserAuthorizedPermission(Permission.FineLocation)){
            Debug.Log("Location permission has been granted");
        }
      
        Input.location.Start();

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and stores it in local variables
            longitude = Input.location.lastData.longitude;
            latitude = Input.location.lastData.latitude;
            altitude = Input.location.lastData.altitude;
        }
    }


    //Run on each frame. Checks to see if user still has permission for FineLocation and runs a coroutine to update all of the stored data
    void Update(){
        if(Permission.HasUserAuthorizedPermission(Permission.FineLocation)){
            StartCoroutine(updateCurrentLocation());
        }
    }
    
    //Stores current location data 
    string updateCurrentLocation(){
        longitude = Input.location.lastData.longitude;
        latitude = Input.location.lastData.latitude;
        altitude = Input.location.lastData.altitude;
        return null;
    }
}
