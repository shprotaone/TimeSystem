using System;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Alarm : MonoBehaviour
{
    private const string alarmText = "Будильник сработал";
    private const string setAlarmText = "Будильник на ";
    private const string errorInputTime = "Указано время неверного формата, попробуйте еще раз";

    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private ClockEngine _clockEngine;
    [SerializeField] private AlarmArrow _alarmArrow;    

    private TimeSpan _alarmTime;
    private Button _setAlarmButton;
    private Notice _notice;

    private bool _enabled;

    void Start()
    {
        _setAlarmButton = GetComponentInChildren<Button>();
        _setAlarmButton.onClick.AddListener(SetAlarm);
        _notice = GetComponent<Notice>();
        _inputField = GetComponentInChildren<TMP_InputField>();       
    }

    private void Update()
    {
        CheckAlarm();
    }


    private void CheckAlarm()
    {
        if ( _enabled && _clockEngine.CurrentTime > _alarmTime)
        {
            StartCoroutine(_notice.SetNoticeRoutine(alarmText, Color.green));
            _enabled = false;
        }
    }

    public void FillInput(float angle, bool PM)
    {
        float currentTime;
        _inputField.placeholder.GetComponent<TMP_Text>().enabled = false;

        currentTime = (360-angle) / 0.5f;

        if (!PM)
        {
            _alarmTime = TimeSpan.FromMinutes(currentTime);
        }
        else
        {
            _alarmTime = TimeSpan.FromMinutes(currentTime + 720);
        }
                
        _inputField.text = _alarmTime.ToString(@"hh\:mm");       
    }

    public void SetArrowFromDigit()
    {
        double currentAngle;
        if (EventSystem.current.currentSelectedGameObject == _inputField.gameObject)
        {
            print("Select");
        }
        else
        {
            _alarmTime = TimeSpan.Parse(_inputField.text);
            currentAngle = 360 - (_alarmTime.TotalMinutes * 0.5f);
            _alarmArrow.ParseTimeToAngle((float)currentAngle);
        }
    }

    public void SetAlarm()
    {       
        try
        {            
            _alarmTime = TimeSpan.Parse(_inputField.text);
            SetArrowFromDigit();
            _enabled = true;
            StartCoroutine(_notice.SetNoticeRoutine(setAlarmText + _alarmTime.ToString(), Color.yellow));
        }
        catch (Exception)
        {
            StartCoroutine(_notice.SetNoticeRoutine(errorInputTime, Color.red));
            throw;
        }
    }
}
