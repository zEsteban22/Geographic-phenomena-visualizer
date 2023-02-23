using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpeedTester : MonoBehaviour
{
    private bool future = true;
    private void putTimeSpeed(int timeSpeed)
    {
        TimeInterface.TimeScale = (float)timeSpeed*(future?1:-1);
    }
    public void changeTimeDirection() { future = future == false; }
    public void x1()
    {
        putTimeSpeed(1);
    }
    public void x32768()
    {
        putTimeSpeed(32768);
    }
    public void x262144()
    {
        putTimeSpeed(262144);
    }
    public void x8()
    {
        putTimeSpeed(8);
    }
    public void x2097152()
    {
        putTimeSpeed(2097152);
    }
    public void x16777216()
    {
        putTimeSpeed(16777216);
    }
    public void x64()
    {
        putTimeSpeed(64);
    }
    public void x8388608()
    {
        putTimeSpeed(8388608);
    }
    public void x256()
    {
        putTimeSpeed(256);
    }
    public void x512()
    {
        putTimeSpeed(512);
    }
    public void x32()
    {
        putTimeSpeed(32);
    }
    public void x2048()
    {
        putTimeSpeed(2048);
    }
    public void x4096()
    {
        putTimeSpeed(4096);
    }
    public void minutes()
    {
        putTimeSpeed(60);
    }
    public void hours()
    {
        putTimeSpeed(3600);
    }
    public void days()
    {
        putTimeSpeed(86400);
    }
    public void weeks()
    {
        putTimeSpeed(604800);
    }
    public void months()
    {
        putTimeSpeed(2629800);
    }
    public void semesters()
    {
        putTimeSpeed(15778800);
    }
    public void years()
    {
        putTimeSpeed(220903200);// ~2^27.71883
    }

}
