using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpeedTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameSystem.timeSpeedDown=1/GameSystem.SECONDS_PER_YEAR;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void putTimeSpeed(int timeSpeed){
        GameSystem.timeSpeedUp = (float) timeSpeed;
    }
    public void x1(){
        putTimeSpeed(1);
    }
    public void x2(){
        putTimeSpeed(2);
    }
    public void x4(){
        putTimeSpeed(4);
    }
    public void x8(){
        putTimeSpeed(8);
    }
    public void x16(){
        putTimeSpeed(16);
    }
    public void x32(){
        putTimeSpeed(32);
    }
    public void x64(){
        putTimeSpeed(64);
    }
    public void x128(){
        putTimeSpeed(128);
    }
    public void x256(){
        putTimeSpeed(256);
    }
    public void x512(){
        putTimeSpeed(512);
    }
    public void x1024(){
        putTimeSpeed(1024);
    }
    public void x2048(){
        putTimeSpeed(2048);
    }
    public void x4096(){
        putTimeSpeed(4096);
    }
    public void minutes(){
        putTimeSpeed(60);
    }
    public void hours(){
        putTimeSpeed(3600);
    }
    public void days(){
        putTimeSpeed(86400);
    }
    public void weeks(){
        putTimeSpeed(604800);
    }
    public void months(){
        putTimeSpeed(2629800);
    }
    public void semesters(){
        putTimeSpeed(15778800);
    }
    public void years(){
        putTimeSpeed(220903200);// ~2^27.71883
    }

}
