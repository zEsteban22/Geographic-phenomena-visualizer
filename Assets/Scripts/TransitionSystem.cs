using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;


public class TransitionSystem : MonoBehaviour
{
    [SerializeField]
    float finalTimeSpeed = 60 * 60 * 24 * 60.875f;
    [SerializeField]
    float initialTimeSpeed = 1;
    [SerializeField]
    float TRANSITION_DURATION = 5f;
    [SerializeField]
    float POWER = 22;
    [SerializeField]
    private float SunThreshold = 60 * 60 * 24 * 60.875f;
    [SerializeField]
    private AudioSource soundtrack;
    [SerializeField]
    private float P = 0.01f;
    private float K;
    private float rotationSpeedST;
    private float rotationSpeedRT = 360f / (24 * 60 * 60);
    float actualRotationSpeed;
    private bool startTransition = false;
    private bool transitioning = false;
    private float lapsed = 0f;
    private bool RTtoST = true; 

    void CalculateInternalParameters()
    {
        rotationSpeedST = 360f * finalTimeSpeed;
        actualRotationSpeed = rotationSpeedRT;
        K = (float)(Math.Log(finalTimeSpeed / P - 1) / (TRANSITION_DURATION / 2));
    }
    // Start is called before the first frame update
    void Start()
    {
        CalculateInternalParameters();
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(actualRotationSpeed * (TimeInterface.TimeScale <= SunThreshold? TimeInterface.deltaTime: 0), 0f, 0f);
        if (startTransition){
            transitioning = true;
            startTransition = false;
            lapsed = 0f;
        }
        if (transitioning){
            lapsed += Time.deltaTime;
            if (RTtoST){                
                TimeInterface.TimeScale = (float)(finalTimeSpeed/(1+Math.Pow(Math.E,-K*(lapsed - TRANSITION_DURATION/2))));
                if(lapsed >= TRANSITION_DURATION)
                {
                    lapsed = TRANSITION_DURATION;
                    transform.rotation = Quaternion.Euler(60f, 0f, 0f);
                    transitioning = false;
                }
            }
            else{
                TimeInterface.TimeScale = finalTimeSpeed * (float)(1 - 1 / (1 + Math.Pow(Math.E, -K * (lapsed - TRANSITION_DURATION / 2))));
                if(lapsed >= TRANSITION_DURATION){
                    lapsed = TRANSITION_DURATION;
                    transitioning = false;
                    transform.rotation = Quaternion.Euler(60f, 0f, 0f);
                }
            }
        }
        
    }

    public bool simulationToRealTime(){
        if (transitioning)
        {
            lapsed = TRANSITION_DURATION - lapsed;
        }
        else
        {
            startTransition = true;
            lapsed = 0f;
        }
        actualRotationSpeed = actualRotationSpeed == 0? rotationSpeedST: actualRotationSpeed;
        RTtoST = false;
        return true;
    }
    public bool realTimeToSimulation(){
        if (transitioning){
            lapsed = TRANSITION_DURATION - lapsed;
        }
        else {
            startTransition = true;
            lapsed = 0f;
        }
        RTtoST = true;
        Thread.Sleep((int) (1000*(TRANSITION_DURATION - lapsed)));
        return RTtoST;
    }
}
