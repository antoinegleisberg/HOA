using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEvents : MonoBehaviour
{
    public static TimeEvents instance;

    public event Action onMorning;
    public event Action onMorningWork;
    public event Action onMidday;
    public event Action onAfternoonWork;
    public event Action onEvening;
    public event Action onNight;

    private void Awake() { instance = this; }

    public void Morning() { if (onMorning != null) onMorning(); }
    public void MorningWork() { if (onMorningWork != null) onMorningWork(); }
    public void Midday() { if (onMidday != null) onMidday(); }
    public void AfternoonWork() { if (onAfternoonWork != null) onAfternoonWork(); }
    public void Evening() { if (onEvening != null) onEvening(); }
    public void Night() { if (onNight != null) onNight(); }
}
