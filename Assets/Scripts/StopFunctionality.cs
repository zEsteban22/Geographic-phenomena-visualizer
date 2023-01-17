using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFunctionality : MonoBehaviour
{
    public void makeStop(){
        PlayPauseFunctionality.putPlay();
        GameSystem.stop();
    }
}
