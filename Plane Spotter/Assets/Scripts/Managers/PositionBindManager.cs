using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionBindManager : MonoBehaviour
{

    public int bindDistance;
    public GameObject cameraObject;
    public GeoConverter converter;

    // attached to the game object using this script. Store its initial scale here
    private Vector3 objectScale;
    private bool objectScaleInitialized = false;

    public Vector3 GetRawPosition(float longitude1, float altitude1, float latitude1,
                                   float longitude2, float altitude2, float latitude2)
    {
        Vector3 loc1 = converter.GeoToCartesian(longitude1, altitude1, latitude1);
        Vector3 loc2 = converter.GeoToCartesian(longitude2, altitude2, latitude2);
        Vector3 posNew = loc2 - loc1;
        return posNew;
    }

    public Vector3 NormalizePosition(Vector3 posNew)
    {
        // Since values are given in feet, they are massive, and the objects
        // will be too tiny to see after their distance and scale is bound.
        // Scale them down here.
        posNew.x *= 0.001f;
        posNew.y *= 0.001f;
        posNew.z *= 0.001f;
        return posNew;
    }

    private Vector3 GetSphereBoundPosition(Vector3 targetPos)
    {
        // camera's position
        Vector3 playerPos = cameraObject.transform.position;
        // distance of target position from player
        Vector3 objDistance = targetPos - playerPos;
        
        // pythagorean theorem to get the raw distance as a float
        float rawDistance = Mathf.Sqrt(
            Mathf.Pow(objDistance.x, 2) +
            Mathf.Pow(objDistance.y, 2) +
            Mathf.Pow(objDistance.z, 2)
        );

        // lock the object's distance to "bindDistance" units away from the player
        float bindDistancePercentLength = bindDistance / rawDistance;
        objDistance.x = objDistance.x * bindDistancePercentLength;
        objDistance.y = objDistance.y * bindDistancePercentLength;
        objDistance.z = objDistance.z * bindDistancePercentLength;

        return objDistance;
    }

    private Vector3 GetSphereBoundScale(Vector3 targetPos)
    {
        // camera's position
        Vector3 playerPos = cameraObject.transform.position;
        // distance of target position from player
        Vector3 objDistance = targetPos - playerPos;
        // pythagorean theorem to get the raw distance as a float
        float rawDistance = Mathf.Sqrt(
            Mathf.Pow(objDistance.x, 2) +
            Mathf.Pow(objDistance.y, 2) +
            Mathf.Pow(objDistance.z, 2)
        );

        // return the scale (based on a percentage of the distance)
        Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);

        float bindDistancePercentLength = bindDistance / rawDistance;
        scale.x = (objectScale.x * bindDistancePercentLength);
        scale.y = (objectScale.y * bindDistancePercentLength);
        scale.z = (objectScale.z * bindDistancePercentLength);


        return scale;
    }

    public void SetBoundPosAndScale(GameObject obj, Vector3 targetPos)
    {
        // position
        obj.transform.position = GetSphereBoundPosition(targetPos);

        // scale
        //
        // store object's scale so it can be scaled correctly each frame.
        // (its scale is multiplied by a factor of its scale)
        if (!objectScaleInitialized)
        {
            objectScaleInitialized = true;
            objectScale = obj.transform.localScale;
        }
        obj.transform.localScale = GetSphereBoundScale(targetPos);
    }

}
