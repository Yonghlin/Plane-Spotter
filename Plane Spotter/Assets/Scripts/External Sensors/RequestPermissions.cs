using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class RequestPermissions : MonoBehaviour
{
    internal void PermissionCallbacks_PermissionDeniedAndDontAskAgain(string permissionName)
    {
        Debug.Log($"{permissionName} PermissionDeniedAndDontAskAgain");
    }

    internal void PermissionCallbacks_PermissionGranted(string permissionName)
    {
        Debug.Log($"{permissionName} PermissionCallbacks_PermissionGranted");
    }

    internal void PermissionCallbacks_PermissionDenied(string permissionName)
    {
        Debug.Log($"{permissionName} PermissionCallbacks_PermissionDenied");
    }
    // Start is called before the first frame update
    void Start()
    {
        if(Permission.HasUserAuthorizedPermission(Permission.FineLocation)){
            Debug.Log("FineLocation permission has been granted.");
            
        }else{
            bool useCallbacks = false;
            if(!useCallbacks){
                Permission.RequestUserPermission(Permission.FineLocation);
            }else{
                 var callbacks = new PermissionCallbacks();
                callbacks.PermissionDenied += PermissionCallbacks_PermissionDenied;
                callbacks.PermissionGranted += PermissionCallbacks_PermissionGranted;
                callbacks.PermissionDeniedAndDontAskAgain += PermissionCallbacks_PermissionDeniedAndDontAskAgain;
                Permission.RequestUserPermission(Permission.FineLocation, callbacks);
            }
        }
        
    }
}
