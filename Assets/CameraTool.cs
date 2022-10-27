using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[ExecuteInEditMode]
public class CameraTool : MonoBehaviour
{
    public GameObject TeleportObject;
    public GameObject MinimapDisplay;
    public bool activator = false;
    private static float minimap_width = 0f;
    private static float terrain_width = 0f;
    public void Update(){
        if (terrain_width == 0f)
            terrain_width = GameObject.Find("Terrain").GetComponent<Collider>().bounds.size.x/2;
        if (minimap_width == 0f)
            minimap_width = MinimapDisplay.GetComponent<RectTransform>().rect.width/2*0.001f;
        Debug.Log("minimap_width: "+minimap_width);
        if (activator){
            activator = false;
            //Create an empty gameObject to use as reference for the teleportation anchor of the minimap object
            GameObject teleportDestination = new GameObject("Teleport destination");
            teleportDestination.transform.position = transform.position;
            teleportDestination.transform.rotation = transform.rotation;
            
            //Create a child of MinimapDisplay 
            GameObject newMinimapIcon = Instantiate(TeleportObject, MinimapDisplay.transform);
            newMinimapIcon.SetActive(true);
            //move it to the correct position according to CameraTool's MinimapCamera view position at the picture time.
            newMinimapIcon.transform.rotation = Quaternion.identity;
            Vector3 translation = transform.position /terrain_width * minimap_width;
            translation.y = 0.0225f;
            newMinimapIcon.transform.Translate(translation);
            //Put the teleportDestination as objective of the minimapIcon's teleportation anchor component
            newMinimapIcon.GetComponent<TeleportationAnchor>().teleportAnchorTransform = teleportDestination.transform;
            
            //Put the picture time in the name of the object to also save the time of the interesting point
            newMinimapIcon.gameObject.name = string.Format("TpAnchor at t={0:F2}, XYZ={1:F2} {2:F2} {3:F2}", GameSystem.timeStep, transform.position.x, transform.position.y, transform.position.z);
        }
    }
}
