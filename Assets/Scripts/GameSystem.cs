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
    [SerializeField]
    private lb_BirdController birds;
    [SerializeField]
    private float birdsThreshold = 3600;
    private float timeStep = 0;
    public static int LAST_STEP = 12;
    private static double _lastTimeScale = 1;
    public static int SECONDS_PER_YEAR = 31557600; // 365.25 * 24 * 60 * 60
    private static GameSystem _instance;
    [SerializeField]
    private AudioMixerGroup mixer;
    [SerializeField]
    private int soundThreshold;
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
            TimeInterface.TimeScale = 1f;//pause();
            PlayPauseFunctionality.putPlay();
            timeStep = LAST_STEP;
        }
        GameObject mesh = _getElement(meshes);
        //The update of the terrain is a workaround due all the terrain mesh filter and colliders are precalculated
        terrain.GetComponent<MeshFilter>().sharedMesh = mesh.GetComponent<MeshFilter>().sharedMesh;
        terrain.GetComponent<MeshCollider>().sharedMesh = mesh.GetComponent<MeshCollider>().sharedMesh;
        //workaround due ocean crest hard to addapt.
        terrain.transform.parent.gameObject.GetComponent<MeshFilter>().sharedMesh = mesh.GetComponent<MeshFilter>().sharedMesh;
        
        text.text = String.Format("year = {0,5:f3}; months = {1,6:f3}; weeks = {2,7:f3}; days = {3,8:f3}; day time = {4,2:D2}:{5,2:D2}:{6,2:D2}", timeStep, timeStep * 12, timeStep * 52.17857, timeStep * 365.25, (int)(timeStep * SECONDS_PER_YEAR / 60 / 60 % 24), (int)(timeStep * SECONDS_PER_YEAR / 60 % 60), (int)(timeStep * SECONDS_PER_YEAR % 60));
        slider.value = timeStep / LAST_STEP;
        backgroundCityRenderer.sprite = _getElement(citySprites);
        oceanSound.pitch = TimeInterface.TimeScale;
        if (TimeInterface.TimeScale < soundThreshold)
        {
            mixer.audioMixer.SetFloat("Volume", (1 - TimeInterface.TimeScale / soundThreshold) * 100 - 100);
            mixer.audioMixer.SetFloat("Pitch", 1f / TimeInterface.TimeScale);
        }
        else if (TimeInterface.TimeScale >= soundThreshold)
            mixer.audioMixer.SetFloat("Volume", -80);
        if (TimeInterface.TimeScale <= birdsThreshold && !birds.gameObject.activeSelf)
        {
            birds.gameObject.SetActive(true);
            birds.AllUnPause();
            birds.AllFlee();
        }
        else if (TimeInterface.TimeScale > birdsThreshold && birds.gameObject.activeSelf)
        {
            birds.AllPause();
            birds.gameObject.SetActive(false);
        }
    }
    public static float TimeStep
    {
        get
        {
            return _instance.timeStep;
        }
    }
    public static Mesh TerrainMesh()
    {
        return _instance.terrain.GetComponent<MeshCollider>().sharedMesh; 
    }
    public static void stop()
    {
        _instance.timeStep = 0;
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
        _instance.timeStep = LAST_STEP * slider.value;
    }

    public static void changeTime(GameObject iconSelected)
    {
        Regex re = new Regex(@"t=([^,]*,\d{2}),");
        Match m = re.Match(iconSelected.name);
        _instance.timeStep = float.Parse(m.Groups[1].Value);
    }

    private T _getElement<T>(List<T> list) where T : UnityEngine.Object
    {
        int index = timeStep == 0 ? 0 : (int)Math.Ceiling(timeStep / LAST_STEP * list.Count) - 1;
        return list[index];
    }
}
