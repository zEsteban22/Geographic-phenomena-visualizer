using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBuilder : MonoBehaviour
{
    /* For multiple buildings
    public List<GameObject> houses;
    public int actualHouse; */
    public GameObject buildingModel;
    public void buildHouse(Transform handTransform){
        RaycastHit hit;
        if (Physics.Raycast(handTransform.transform.position, handTransform.forward, out hit))
            if (hit.collider.gameObject.tag == "Terrain"){
                Vector3 point = hit.point;
                point.y += 4.6f;//This is to place the house right over the terrain
                Instantiate(buildingModel, point, Quaternion.identity);
            }
    }
}
