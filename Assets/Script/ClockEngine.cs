using UnityEngine;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

public class ClockEngine : MonoBehaviour
{
    private const float hourInSeconds = 3600f;
    private const string completeText = "Соединение установлено";
    private const string errorText = "Ошибка соединения ";

    [SerializeField] private int _hour;
    [SerializeField] private int _minutes;
    [SerializeField] private int _seconds;

    private Notice _notice;
    private TimeSpan _currentTime;
    private float _msecs = 0;

    public int Hour => _hour;
    public int Minutes => _minutes;
    public int Seconds => _seconds;

    public TimeSpan CurrentTime { get { return new TimeSpan(_hour, _minutes, _seconds); } }
    
    private void Start()
    {
        _notice = GetComponent<Notice>();
        SyncTime();
        StartCoroutine(HourDelay());
    }

    private void Update()
    {
        CalculateTime();
    }

    private void CalculateTime()
    {
        _msecs += Time.deltaTime * 1;
        if (_msecs >= 1.0f)
        {
            _msecs -= 1.0f;
            _seconds++;
            if (_seconds >= 60)
            {
                _seconds = 0;
                _minutes++;
                if (_minutes > 60)
                {
                    _minutes = 0;
                    _hour++;
                    if (_hour >= 24)
                        _hour = 0;
                }
            }
        }
    }

    private void SyncTime()
    {
        TimeSpan netTime;

        try
        {
            netTime = NTPSync.GetTime(NTPSync.ntpServer1);
            StartCoroutine(_notice.SetNoticeRoutine(completeText,Color.green));
        }
        catch (SocketException)
        {
            StartCoroutine(_notice.SetNoticeRoutine(errorText + NTPSync.ntpServer1, Color.red));
            try
            {
                netTime = NTPSync.GetTime(NTPSync.ntpServer2);
                StartCoroutine(_notice.SetNoticeRoutine(completeText, Color.green));
            }
            catch (SocketException)
            {
                StartCoroutine(_notice.SetNoticeRoutine(errorText + NTPSync.ntpServer2, Color.red));
                _notice.SetNoticeRoutine("Устанавливаю локальное время", Color.yellow);                
                _currentTime = DateTime.Now.TimeOfDay;
            }
        }
        finally
        {
            if(_currentTime != netTime)
            {
                _currentTime = netTime;

                _hour = _currentTime.Hours;
                _minutes = _currentTime.Minutes;
                _seconds = _currentTime.Seconds;
            }           
        }
    }   

    private IEnumerator HourDelay()
    {
        yield return new WaitForSeconds(hourInSeconds);
        SyncTime();
        StartCoroutine(HourDelay());
    }
}
