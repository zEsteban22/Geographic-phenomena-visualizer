using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CameraTool : MonoBehaviour
{
    public GameObject TeleportObject;
    public GameObject MinimapDisplay;
    public void TakePicture(){
        //Create an empty gameObject to use as reference for the teleportation anchor of the minimap object
        GameObject teleportDestination = new GameObject("Teleport destination");
        teleportDestination.transform.position = transform.position;
        teleportDestination.transform.rotation = transform.rotation;
        Debug.Log(transform.rotation);
        Debug.Log(gameObject.transform.rotation);
        
        //Create a child of MinimapDisplay 
        GameObject newMinimapIcon = Instantiate(TeleportObject, MinimapDisplay.transform);
        //move it to the correct position according to CameraTool's MinimapCamera view position at the picture time.
        // Para mapear: x_mapeado = x_origenMapa + (x_foto) /x_maxima * mapeo_máximo

        newMinimapIcon.transform.rotation = Quaternion.Euler(0, 0, 0);
        newMinimapIcon.transform.Translate(transform.position /50 *0.275f);
        //Put the teleportDestination as objective of the minimapIcon's teleportation anchor component
        newMinimapIcon.GetComponent<TeleportationAnchor>().teleportAnchorTransform = teleportDestination.transform;
        
        //Put the picture time in the name of the object to also save the time of the interesting point
        newMinimapIcon.gameObject.name = string.Format("TpAnchor at t={0:F2}, XYZ={1:F2} {2:F2} {3:F2}", GameSystem.timeStep, transform.position.x, transform.position.y, transform.position.z);
    }
}
