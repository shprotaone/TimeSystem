using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogClock : MonoBehaviour
{
    private const float hoursToDegrees = 360f / 12f;
    private const float minutesToDegrees = 360f / 60f;
    private const float secondsToDegrees = 360f / 60f;

    [SerializeField] private ClockEngine _clockEngine;

    [SerializeField] private GameObject _pointerHour;
    [SerializeField] private GameObject _pointerMinutes;
    [SerializeField] private GameObject _pointerSeconds;
    [SerializeField] private GameObject _alarmArrow;

    private float _msecs = 0;

    private void Update()
    {
        ClockMove();
    }

    private void ClockMove()
    {
        float rotationSeconds = (secondsToDegrees) * _clockEngine.Seconds;
        float rotationMinutes = (minutesToDegrees) * _clockEngine.Minutes;
        float rotationHours = ToHours(_clockEngine.Hour, _clockEngine.Minutes);

        _pointerHour.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -rotationHours);      //почему отрицательное значение? 
        _pointerMinutes.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -rotationMinutes);
        _pointerSeconds.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -rotationSeconds); //возможно сделать через iTween?
    }   

    public float ToHours(float hour,float minutes)
    {
        return ((hoursToDegrees) * hour) + ((360.0f / (60.0f * 12.0f)) * minutes);
    }
}
