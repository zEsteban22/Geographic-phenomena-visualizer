
using UnityEngine;
public class TimeInterface : MonoBehaviour
{
    private static TimeInterface _instance = null;
    private float timeScale = 1f;
    private float speedUp = 1;
    private float speedDown = 1;
    public static bool exists { get { return _instance != null; } } 
    void Start()
    {
        _instance = this;
    }
    public static float deltaTime
    {
        get
        {
            return Time.deltaTime * _instance.timeScale * _instance.speedUp * _instance.speedDown;
        }
    }
    public static float TimeScale
    {
        get
        {
            return _instance.timeScale;
        }
        set
        {
            _instance.timeScale = value;
        }
    }
    public static float time
    {
        get
        {
            return Time.time * _instance.timeScale;
        }
    }
    public static float SpeedUp
    {
        get
        {
            return _instance.speedUp;
        }
        set
        {
            _instance.speedUp = value;
        }
    }
    public static float SpeedDown
    {
        get
        {
            return _instance.speedDown;
        }
        set
        {
            _instance.speedDown = value;
        }
    }
}
