
using UnityEngine;
    public class TimeInterface : MonoBehaviour
    {
        private static TimeInterface _instance;
        private float speedUp = 1;
        private float speedDown = 1;
        void Start()
        {
            _instance = this;
        }
        public static float deltaTime
        {
            get
            {
                return Time.deltaTime * _instance.speedUp * _instance.speedDown;
            }
        }
        public static float TimeScale
        {
            get
            {
                return Time.timeScale * _instance.speedUp * _instance.speedDown;
            }
            set
            {
                Time.timeScale = value;
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
