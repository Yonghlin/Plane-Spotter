using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionBindManager : MonoBehaviour
{

    public int bindDistance;
    public GameObject cameraObject;

    // attached to the game object using this script. Store its initial scale here
    private Vector3 objectScale;

    private Vector3 GetSphereBoundPosition(Vector3 targetPos)
    {
        // camera's position
        Vector3 playerPos = cameraObject.transform.position;
        // distance of target position from player
        Vector3 objDistance = targetPos - playerPos;
        
        // actually this pythagorean theorem stuff might not be needed. idk
        // pythagorean theorem to get the raw distance as a float
        float rawDistance = Mathf.Sqrt(
            Mathf.Pow(objDistance.x, 2) +
            Mathf.Pow(objDistance.y, 2) +
            Mathf.Pow(objDistance.z, 2)
        );


        // why doesnt this work :(
        // lock the object's distance to "bindDistance" units away from the player
        objDistance.x = (bindDistance / objDistance.x) * targetPos.x;
        objDistance.y = (bindDistance / objDistance.y) * targetPos.y;
        objDistance.z = (bindDistance / objDistance.z) * targetPos.z;

        Debug.Log("bound pos x: " + objDistance.x);
        Debug.Log("bound pos y: " + objDistance.y);
        Debug.Log("bound pos z: " + objDistance.z);

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
        Vector3 scale = new Vector3(1, 1, 1);
        scale.x = (bindDistance / rawDistance) * objectScale.x;
        scale.y = (bindDistance / rawDistance) * objectScale.y;
        scale.z = (bindDistance / rawDistance) * objectScale.z;

        // why doesnt this work :(
        Debug.Log("bound scale x: " + scale.x);
        Debug.Log("bound scale y: " + scale.y);
        Debug.Log("bound scale z: " + scale.z);

        return scale;
    }

    public void SetBoundPosAndScale(GameObject obj, Vector3 targetPos)
    {
        obj.transform.position = GetSphereBoundPosition(targetPos);
        // store object's scale so it can be scaled correctly each frame.
        // (its scale is multiplied by a factor of its scale)
        if (objectScale == null)
        {
            objectScale = obj.transform.localScale;
        }
        obj.transform.localScale = GetSphereBoundScale(targetPos);
    }

}
