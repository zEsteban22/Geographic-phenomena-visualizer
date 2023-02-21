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
        A = (float)(NormalTimeSpeed *(1-ScaleModifier));
        K = (float)(100/Sqrt(NormalTimeSpeed));
    }

    void Update()
    {
        t = System.Math.Abs(TimeInterface.TimeScale);
        audioSource.pitch = (float)((A + ScaleModifier * t) / NormalTimeSpeed);
        mixerGroup.audioMixer.SetFloat("Pitch", (float)(NormalTimeSpeed / (A + ScaleModifier * t)));
        mixerGroup.audioMixer.SetFloat("Volume", K*Sqrt(t)-30>=0?0:(float)(K * Sqrt(t) - 30));
        //mixerGroup.audioMixer.SetFloat("Volume", (float)((System.Math.Abs(TimeInterface.TimeScale) / NormalTimeSpeed) * 100 - 80));
    }
}
