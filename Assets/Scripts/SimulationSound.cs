using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SimulationSound : MonoBehaviour
{
    [SerializeField]
    private double NormalTimeSpeed = 86400;
    [SerializeField]
    private float ScaleModifier = 1f / 8f;
    private float A;
    private AudioSource audioSource;
    private AudioMixerGroup mixerGroup;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mixerGroup = audioSource.outputAudioMixerGroup;
        A = (float)(NormalTimeSpeed *(1-ScaleModifier));
    }

    void Update()
    {
        audioSource.pitch = (float)((A + ScaleModifier * TimeInterface.TimeScale) / NormalTimeSpeed);
        mixerGroup.audioMixer.SetFloat("Pitch", (float)(NormalTimeSpeed / (A + ScaleModifier * TimeInterface.TimeScale)));
    }
}
