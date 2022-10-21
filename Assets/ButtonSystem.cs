using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ButtonSystem : MonoBehaviour
{
    public float threshold=0.1f;
    public float deadZone=0.025f;
    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;
    public UnityEvent onPressed, onReleased;
    void Start(){
        _startPos = transform.localPosition;
        _joint=GetComponent<ConfigurableJoint>();
    }
    void Update(){
        if (!_isPressed && GetValue() + threshold>=1)
            Pressed();
        else if (_isPressed && GetValue() - threshold <= 0)
            Released();
    }
    private float GetValue(){
        float value = Vector3.Distance(_startPos, transform.localPosition);
        if (Math.Abs(value) < deadZone)
            value=0f;
        return Mathf.Clamp(value, -1f, 1f);
    }
    private void Pressed(){
        _isPressed = true;
        onPressed.Invoke();
    }
    private void Released(){
        _isPressed = false;
        onReleased.Invoke();
    }
}
