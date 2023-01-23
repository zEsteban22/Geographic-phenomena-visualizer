using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPauseFunctionality : MonoBehaviour
{
    private bool isPlaying;
    public Sprite playSprite;
    public Sprite pauseSprite;
    public GameObject showingSprite;
    private static PlayPauseFunctionality instance;
    void Start(){
        instance = this;
    }
    public void ExecuteFunctionality(){
        if(isPlaying)
            GameSystem.pause();
        else
            GameSystem.resume();
        isPlaying = isPlaying == false;
    }
    public void changeSprite(){
        showingSprite.GetComponent<SpriteRenderer>().sprite = isPlaying?pauseSprite:playSprite;
    }
    public static void putPlay(){
        instance.isPlaying=false;
        instance.changeSprite();
    }
}
