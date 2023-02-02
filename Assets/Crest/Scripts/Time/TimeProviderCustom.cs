// Crest Ocean System

// This file is subject to the MIT License as seen in the root of this folder structure (LICENSE)

using UnityEditor;
using UnityEngine;

namespace Crest
{
    /// <summary>
    /// This time provider fixes the ocean time at a custom value which is usable for testing/debugging.
    /// </summary>
    [AddComponentMenu(Internal.Constants.MENU_PREFIX_SCRIPTS + "Custom Time Provider")]
    [HelpURL(Internal.Constants.HELP_URL_BASE_USER + "time-providers.html" + Internal.Constants.HELP_URL_RP + "#supporting-pause")]
    public class TimeProviderCustom : TimeProviderBase
    {
        /// <summary>
        /// The version of this asset. Can be used to migrate across versions. This value should
        /// only be changed when the editor upgrades the version.
        /// </summary>
        [SerializeField, HideInInspector]
#pragma warning disable 414
        int _version = 0;
#pragma warning restore 414

        /// <summary>
        /// Freezes the time
        /// </summary>
        [Tooltip("Freeze progression of time. Only works properly in Play mode.")]
        public bool _paused = false;

        [Tooltip("Override time used for ocean simulation to value below.")]
        public bool _overrideTime = false;
        [Predicated("_overrideTime"), DecoratedField]
        public float _time = 0f;

        [Tooltip("Override delta time used for ocean simulation to value below. This in particular affects dynamic elements of the simulation like the foam simulation and the ripple simulation.")]
        public bool _overrideDeltaTime = false;
        [Predicated("_overrideDeltaTime"), DecoratedField]
        public float _deltaTime = 0f;

        TimeProviderDefault _tpDefault = new TimeProviderDefault();

        float _timeInternal = 0f;
        [SerializeField]
        int MaximumSpeed = 40;
        private void Start()
        {
            // May as well start on the same time value as unity
            _timeInternal = Time.time;
        }

        private void Update()
        {
            if (!_paused)
            {
                _timeInternal += _getTime();
            }
        }
        private float _getTime()
        {
            if (TimeInterface.exists)
                return TimeInterface.TimeScale < MaximumSpeed ? TimeInterface.deltaTime : TimeInterface.deltaTime / TimeInterface.TimeScale * MaximumSpeed;
            else
                return Time.deltaTime;
        }
        public override float CurrentTime
        {
            get
            {
                // Override means override
                if (_overrideTime)
                {
                    return _time;
                }

                // In edit mode, update is seldom called, so rely on the default TP
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying && !_paused)
                {
                    return _tpDefault.CurrentTime;
                }
#endif

                // Otherwise use our accumulated time
                return _timeInternal;
            }
        }

        public override float DeltaTime
        {
            get
            {
#if UNITY_EDITOR
                if (UnityEditor.EditorApplication.isPlaying)
                {
                    return _getTime();
                }
                else
                {
                    return 1f / 20f;
                }
#else
                return _getTime();
#endif
                ;
            }

        }


        public override float DeltaTimeDynamics => DeltaTime;
    }
}
