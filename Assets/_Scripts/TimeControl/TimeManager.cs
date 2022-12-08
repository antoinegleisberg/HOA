using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float _timeScale = 144f; // 10 minutes = 1 ingame day
    [SerializeField] private float _timeInHours;
    
    [SerializeField] private int _morningHour = 6;
    [SerializeField] private int _morningWorkHour = 8;
    [SerializeField] private int _middayHour = 12;
    [SerializeField] private int _afternoonWorkHour = 14;
    [SerializeField] private int _eveningHour = 18;
    [SerializeField] private int _nightHour = 22;


    private void Update()
    {
        float newTime = _timeInHours + Time.deltaTime * _timeScale / 3600f;
        
        if (_timeInHours < _morningHour && newTime >= _morningHour) TimeEvents.instance.Morning();
        if (_timeInHours < _morningWorkHour && newTime >= _morningWorkHour) TimeEvents.instance.MorningWork();
        if (_timeInHours < _middayHour && newTime >= _middayHour) TimeEvents.instance.Midday();
        if (_timeInHours < _afternoonWorkHour && newTime >= _afternoonWorkHour) TimeEvents.instance.AfternoonWork();
        if (_timeInHours < _eveningHour && newTime >= _eveningHour) TimeEvents.instance.Evening();
        if (_timeInHours < _nightHour && newTime >= _nightHour) TimeEvents.instance.Night();

        _timeInHours = newTime;
    }
}
