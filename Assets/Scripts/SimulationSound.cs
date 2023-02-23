using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static System.Math;

public class SimulationSound : MonoBehaviour
{
    [SerializeField]
    private double NormalTimeSpeed = 86400;
    [SerializeField]
    private float ScaleModifier = 1f / 8f;
    [SerializeField]
    private float volumeIncreaseRate = 0.2f;
    private float A;
    private float K;
    private AudioSource audioSource;
    private AudioMixerGroup mixerGroup;
    private float t;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mixerGroup = audioSource.outputAudioMixerGroup;
    }

    void Update()
    {
        t = System.Math.Abs(TimeInterface.TimeScale);
        A = (float)(NormalTimeSpeed * (1 - ScaleModifier));
        audioSource.pitch = (float)((A + ScaleModifier * t) / NormalTimeSpeed);
        mixerGroup.audioMixer.SetFloat("Pitch", (float)(NormalTimeSpeed / (A + ScaleModifier * t)));
        if (t >= 32) 
            mixerGroup.audioMixer.SetFloat("Volume", 0);
        else
            mixerGroup.audioMixer.SetFloat("Volume", -80);
        //mixerGroup.audioMixer.SetFloat("Volume", (float)((System.Math.Abs(TimeInterface.TimeScale) / NormalTimeSpeed) * 100 - 80));
    }
}
