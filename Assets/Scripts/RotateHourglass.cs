using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHourglass : MonoBehaviour
{
    public float rotationSpeed;
    private float rotation;
    void Start()
    { }
    void Update(){
        if (rotation != 0){
            transform.Rotate(rotationSpeed*rotation,0f,0f);
            GameSystem.changeSpeedDown(transform);
        }
    }

    public void RotateLeft(){
        rotation = 1;
    } 
    public void RotateRight(){
        rotation = -1;
    }
    public void stopRotation(){
        rotation = 0;
    }
}
