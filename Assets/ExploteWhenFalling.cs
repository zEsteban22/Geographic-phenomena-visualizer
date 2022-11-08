using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploteWhenFalling : MonoBehaviour
{
    private float threshold= 0.1f;
    void Update()
    {
        if (GetComponent<Rigidbody>().angularVelocity.magnitude > threshold){
            Destroy(gameObject);
        } 
    }
}
