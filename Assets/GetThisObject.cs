using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetThisObject : MonoBehaviour
{
    public void Activate(){
        GameSystem.changeTime(gameObject);
        //mover todos los objetos de la base junto con el usuario
    }
}
