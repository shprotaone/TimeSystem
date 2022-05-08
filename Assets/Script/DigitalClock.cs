using UnityEngine;
using TMPro;
using System;

public class DigitalClock : MonoBehaviour
{
    [SerializeField] private ClockEngine _clockEngine;

    [SerializeField] private TMP_Text _timeText;
    TimeSpan time;

    void Update()
    {
        time = _clockEngine.CurrentTime;
        _timeText.text = time.ToString(@"hh\:mm\:ss");
    }
}
