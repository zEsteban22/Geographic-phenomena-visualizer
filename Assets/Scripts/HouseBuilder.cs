using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HouseBuilder : MonoBehaviour
{
    /* For multiple buildings
    public List<GameObject> houses;
    public int actualHouse; */
    public GameObject buildingModel;
    public XRRayInteractor rayInteractor;
    public void buildHouse(){
        RaycastHit hit;
        if (rayInteractor.TryGetCurrent3DRaycastHit(out hit))
            if (hit.collider.gameObject.tag == "Terrain"){
                Vector3 point = hit.point;
                point.y += 4.6f;//This is to place the house right over the terrain
                Instantiate(buildingModel, point, Quaternion.identity).SetActive(true);

            }
    }
}
