using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCacheManager : MonoBehaviour
{
    public CacheManager cache;
    public bool testingCache;

    // Start is called before the first frame update
    void Start()
    {
        if (testingCache)
        {
            cache.saveCurrentAirportState("bruh", 10, -10);

            Debug.Log("Cached Data Validity Status 1:" + cache.checkAirportValidityStatus(15, 15));
            cache.getCurrentlyCachedAirportData();
            Debug.Log("Cached Data Validity Status 2:" + cache.checkAirportValidityStatus(-15, -15));
            cache.getCurrentlyCachedAirportData();
            Debug.Log("Cached Data Validity Status 3:" + cache.checkAirportValidityStatus(-15, 15));
            cache.getCurrentlyCachedAirportData();
            Debug.Log("Cached Data Validity Status 4:" + cache.checkAirportValidityStatus(15, -15));
            cache.getCurrentlyCachedAirportData();
            Debug.Log("Cached Data Validity Status 5:" + cache.checkAirportValidityStatus(10, 10));
            cache.getCurrentlyCachedAirportData();
            Debug.Log("Cached Data Validity Status 6 (Need to refresh cache):" + cache.checkAirportValidityStatus(10, 10));

            cache.saveCurrentAirportState("bruh", 10, -10);
            Debug.Log("Cached Data Validity Status 7 (Cache refreshed):" + cache.checkAirportValidityStatus(10, 10));
        }
    }

   
}
