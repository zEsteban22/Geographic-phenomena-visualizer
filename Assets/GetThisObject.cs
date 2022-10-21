using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GetThisObject : MonoBehaviour
{
    public GameObject _base;
    public void Activate(){
        GameSystem.changeTime(gameObject);
        _base.transform.position = GetComponent<TeleportationAnchor>().teleportAnchorTransform.position;
        //mover todos los objetos de la base junto con el usuario
    }
}
