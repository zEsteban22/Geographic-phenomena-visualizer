using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

public class GameSystem : MonoBehaviour
{
    //public Text text;
    public Slider slider;
    public GameObject growingTree;
    public List<GameObject> meshes;
    public GameObject terrain;
    
    public static float timeStep = 0;
    public static float timeSpeedUp = 1;
    public static float timeSpeedDown = 1;
    private static float _lastTimeSpeedDown = 0;
    private static int LAST_STEP = 60;
    void Start()
    {
        //text.text = timeStep.ToString("F2");
    }

    void Update()
    {
        timeStep += Time.deltaTime * timeSpeedUp * timeSpeedDown;
        if (timeStep < 0f){
            PlayPauseFunctionality.putPlay();
            stop();
        }
        if (timeStep > LAST_STEP){
            PlayPauseFunctionality.putPlay();
            timeStep = LAST_STEP;
            pause();
        }
        float treeGrowthState = timeStep / LAST_STEP;
        growingTree.transform.localScale = Vector3.one * treeGrowthState;
        int index = timeStep == 0 ? 0 : (int) Math.Ceiling(timeStep / LAST_STEP * meshes.Count) - 1;
        terrain.GetComponent<MeshFilter>().sharedMesh = meshes[index].GetComponent<MeshFilter>().sharedMesh;
        terrain.GetComponent<MeshCollider>().sharedMesh = meshes[index].GetComponent<MeshCollider>().sharedMesh;
        //text.text = timeStep.ToString("F2");
        slider.value = timeStep/LAST_STEP;
    }

    

    public static void stop()
    {
        _lastTimeSpeedDown = timeSpeedDown;
        timeSpeedDown = 0;
        timeStep = 0;
    }

    public static void pause()
    {
        _lastTimeSpeedDown = timeSpeedDown;
        timeSpeedDown = 0;
    }

    public static void resume()
    {
        timeSpeedDown = _lastTimeSpeedDown;
    }

    public static void changeSpeedUp(Transform palanca)
    {
        timeSpeedUp = (1.0f + palanca.rotation.x) / 2.0f * 4.0f ; 
    }


    public static void changeSpeedDown(Transform reloj)
    {
        float angulo = reloj.rotation.x;
        if (angulo > 0.7)
            angulo = 0.5f + (angulo-0.7f)/0.3f/2f;
        else if (angulo <= 0.7)
            angulo = angulo/0.7f/2f;
        timeSpeedDown = (float) -Math.Cos(Math.PI * angulo);

    }

    public static void changeTime(Slider slider){
        timeStep = LAST_STEP*slider.value;
    }

    public static void changeTime(GameObject iconSelected){
        Regex re = new Regex(@"t=([^,]*,\d{2}),");
        Match m = re.Match(iconSelected.name);
        Debug.Log(m.Groups[1]);
        timeStep = float.Parse(m.Groups[1].Value);
    }

}
