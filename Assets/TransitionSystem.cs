using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;


public class TransitionSystem : MonoBehaviour
{
    [SerializeField]
    private float finalDaysPerSecond = 60;
    [SerializeField]
    private float TRANSITION_DURATION = 5f;
    [SerializeField]
    private float POWER = 22;
    [SerializeField]
    private lb_BirdController birds;
    [SerializeField]
    private AudioSource soundtrack;
    private float rotationSpeedST;
    private float rotationSpeedRT;
    private float actualRotationSpeed;
    private bool startTransition = false;
    private bool transitioning = false;
    private float lapsed = 0f;
    private bool RTtoST = true;
    
    private float A;
    // Start is called before the first frame update
    void Start()
    {
        rotationSpeedRT = 360f/(24*60*60);
        rotationSpeedST = 360f * finalDaysPerSecond;
        actualRotationSpeed = rotationSpeedRT;
        A = 31557599;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeedRT * Time.deltaTime, 0f, 0f);
        if (startTransition){
            transitioning = true;
            startTransition = false;
            lapsed = 0f;
        }
        if (transitioning){
            lapsed += Time.deltaTime / Time.timeScale;
            if (RTtoST){                
                Time.timeScale = (float)(A*Math.Pow(lapsed/TRANSITION_DURATION,POWER) + 1);
                if (birds.gameObject.activeSelf && Time.timeScale >= 7948800){
                    soundtrack.Pause();
                    birds.AllPause();
                    birds.gameObject.SetActive(false);
                }
                if(Time.timeScale >= A){
                    lapsed = TRANSITION_DURATION;
                    transform.rotation = Quaternion.Euler(60f, 0f, 0f);
                    transitioning = false;
                }
            }
            else {
                Time.timeScale = (float)(A*Math.Pow(1 - lapsed/TRANSITION_DURATION,POWER) + 1);
                if (Time.timeScale < 7948800 && !birds.gameObject.activeSelf){
                        birds.gameObject.SetActive(true);
                        birds.AllUnPause();
                        birds.AllFlee();
                        soundtrack.Play();
                }
                if(Time.timeScale == 1){
                    lapsed = TRANSITION_DURATION;
                    actualRotationSpeed = rotationSpeedRT;
                    transitioning = false;
                }
            }
        }
        
    }
    public bool simulationToRealTime(){
        if (transitioning){
            lapsed = TRANSITION_DURATION - lapsed;
            Debug.Log("Lapsed = "+lapsed);
        }
        else 
            startTransition = true;
        actualRotationSpeed = actualRotationSpeed == 0? rotationSpeedST: actualRotationSpeed;
        RTtoST = false;
        return true;
    }
    public bool realTimeToSimulation(){
        if (transitioning){
            lapsed = TRANSITION_DURATION - lapsed;
            Debug.Log("Restante para play = "+(int) (1001*(TRANSITION_DURATION - lapsed)));
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
