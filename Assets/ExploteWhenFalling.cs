using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploteWhenFalling : MonoBehaviour
{
    private bool touchedFloor = false;
    private float timeMoving = 0;
    void Update()
    {
        if (timeMoving > 1f)
            Destroy(gameObject);
        else if (touchedFloor && GetComponent<Rigidbody>().velocity.magnitude > 0){
            timeMoving += Time.deltaTime;
        } else if (!touchedFloor && GetComponent<Rigidbody>().velocity.magnitude == 0) {
            touchedFloor = true;
        }
    }
}
