using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.Audio;

public class GameSystem : MonoBehaviour
{
    public TMP_Text text;
    public Slider slider;
    public TransitionSystem transitionSystem;
    public List<GameObject> meshes;
    public GameObject terrain;
    public SpriteRenderer backgroundCityRenderer;
    public List<Sprite> citySprites; 
    public AudioSource oceanSound;
    public static float timeStep = 0;
    private static int LAST_STEP = 12;
    private static double _lastTimeScale = 1;
    public static int SECONDS_PER_YEAR = 31557600; // 365.25 * 24 * 60 * 60
    private static GameSystem _instance;
    [SerializeField]
    private AudioMixerGroup mixer;
    [SerializeField]
    private int MaximumSpeed = 24;
    void Start()
    {
        _instance = this;
    }

    void Update()
    {
        timeStep += (float)(TimeInterface.deltaTime / SECONDS_PER_YEAR);
        if (timeStep < 0f)
        {
            PlayPauseFunctionality.putPlay();
            stop();
        }
        if (timeStep > LAST_STEP)
        {
            pause();
            PlayPauseFunctionality.putPlay();
            timeStep = LAST_STEP;
        }
        float treeGrowthState = timeStep / LAST_STEP;
        foreach(GameObject growingTree in GameObject.FindGameObjectsWithTag("Tree"))
        {
            growingTree.transform.localScale = Vector3.one * treeGrowthState * float.Parse(growingTree.name.Split(" ")[0]);
        }
        
        GameObject mesh = _getElement(meshes);
        //The update of the terrain is a workaround due all the terrain mesh filter and colliders are precalculated
        terrain.GetComponent<MeshFilter>().sharedMesh = mesh.GetComponent<MeshFilter>().sharedMesh;
        terrain.GetComponent<MeshCollider>().sharedMesh = mesh.GetComponent<MeshCollider>().sharedMesh;
        text.text = String.Format("year = " + timeStep.ToString("F8") + "; time speed = "+ String.Format("{0:0.0000}", TimeInterface.TimeScale));
        slider.value = timeStep / LAST_STEP;
        backgroundCityRenderer.sprite = _getElement(citySprites);
        oceanSound.pitch = TimeInterface.TimeScale;
        if (TimeInterface.TimeScale <= MaximumSpeed)
            mixer.audioMixer.SetFloat("Volume", (1-TimeInterface.TimeScale/MaximumSpeed)*100-80);
        mixer.audioMixer.SetFloat("Pitch", 1f / TimeInterface.TimeScale);

    }


    public static void stop()
    {
        timeStep = 0;
        pause();

    }

    public async static void pause()
    {
        if (await Task.Run(() => _instance.transitionSystem.simulationToRealTime()))
        {
            //_lastTimeScale = Time.timeScale;
            if (TimeInterface.SpeedDown != 0)
                _lastTimeScale = TimeInterface.SpeedDown;
            TimeInterface.SpeedDown = 0;
        }
        else
        {
            Debug.Log("transition stopped");
        }
    }

    public async static void resume()
    {
        if (await Task.Run(() => _instance.transitionSystem.realTimeToSimulation()))
        {
            //Time.timeScale = _lastTimeScale;  
            TimeInterface.SpeedDown = (float)_lastTimeScale;
        }
        else
        {
            Debug.Log("play transition stopped");
        }
    }
    public static void changeSpeedUp(Transform palanca)
    {
        TimeInterface.SpeedUp = (1.0f + palanca.rotation.x) / 2.0f * 10.0f;
    }


    public static void changeSpeedDown(Transform reloj)
    {
        float angulo = reloj.rotation.x;
        if (angulo > 0.7)
            angulo = 0.5f + (angulo - 0.7f) / 0.3f / 2f;
        else if (angulo <= 0.7)
            angulo = angulo / 0.7f / 2f;
        TimeInterface.SpeedDown = (float)-Math.Cos(Math.PI * angulo);

    }

    public static void changeTime(Slider slider)
    {
        timeStep = LAST_STEP * slider.value;
    }

    public static void changeTime(GameObject iconSelected)
    {
        Regex re = new Regex(@"t=([^,]*,\d{2}),");
        Match m = re.Match(iconSelected.name);
        timeStep = float.Parse(m.Groups[1].Value);
    }

    private T _getElement<T>(List<T> list) where T : UnityEngine.Object
    {
        int index = timeStep == 0 ? 0 : (int)Math.Ceiling(timeStep / LAST_STEP * list.Count) - 1;
        return list[index];
    }
}
