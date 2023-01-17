using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;

public class GameSystem : MonoBehaviour
{
    public TMP_Text text;
    public Slider slider;
    public TransitionSystem transitionSystem;
    public GameObject growingTree;
    public List<GameObject> meshes;
    public GameObject terrain;
    public SpriteRenderer backgroundCityRenderer;
    public List<Sprite> citySprites; //at this moment there is 12 city sprites, one for year

    public static float timeStep = 0; 
    private static int LAST_STEP = 12; 
    public static float timeSpeedUp = 1;
    public static float timeSpeedDown = 0;
    private static float _lastTimeScale = 1;
    private static int SECONDS_PER_YEAR = 31557600; // 365.25 * 24 * 60 * 60
    private static GameSystem _instance;
    void Start()
    {
        _instance = this;
    }

    void Update()
    {
        //timeStep += Time.deltaTime /SECONDS_PER_YEAR * timeSpeedUp * timeSpeedDown;
        timeStep += Time.deltaTime /Time.timeScale * timeSpeedUp * timeSpeedDown;
        if (timeStep < 0f){
            PlayPauseFunctionality.putPlay();
            stop();
        }
        if (timeStep > LAST_STEP){
            pause();
            PlayPauseFunctionality.putPlay();
            timeStep = LAST_STEP;
        }
        float treeGrowthState = timeStep / LAST_STEP;
        growingTree.transform.localScale = Vector3.one * treeGrowthState;
        GameObject mesh = _getElement(meshes);
        //The update of the terrain is a workaround due all the terrain mesh filter and colliders are precalculated
        terrain.GetComponent<MeshFilter>().sharedMesh = mesh.GetComponent<MeshFilter>().sharedMesh;
        terrain.GetComponent<MeshCollider>().sharedMesh = mesh.GetComponent<MeshCollider>().sharedMesh;
        text.text = String.Format("timeStep = " + timeStep.ToString("F2") + "; time speed = {0}", timeSpeedUp * timeSpeedDown);
        slider.value = timeStep/LAST_STEP;
        backgroundCityRenderer.sprite = _getElement(citySprites);
        
    }
    

    public static void stop()
    {
        timeStep = 0;
        pause();
        
    }

    public async static void pause()
    {
        if(await Task.Run(()=>_instance.transitionSystem.simulationToRealTime())){
            //_lastTimeScale = Time.timeScale;
            if (timeSpeedDown != 0)
                _lastTimeScale = timeSpeedDown;
            timeSpeedDown = 0;
        } else {
            Debug.Log("transition stopped");
        }
    }

    public async static void resume()
    {
        if(await Task.Run(()=>_instance.transitionSystem.realTimeToSimulation())){
            //Time.timeScale = _lastTimeScale;  
            timeSpeedDown = _lastTimeScale;  
        } else {
            Debug.Log("play transition stopped");
        }
    }
    public static void changeSpeedUp(Transform palanca)
    {
        timeSpeedUp = (1.0f + palanca.rotation.x) / 2.0f * 10.0f ; 
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
        timeStep = float.Parse(m.Groups[1].Value);
    }

    private T _getElement<T>(List<T> list) where T:UnityEngine.Object {
        int index = timeStep == 0? 0 : (int) Math.Ceiling(timeStep/LAST_STEP * list.Count) - 1;
        return list[index];
    }
}
