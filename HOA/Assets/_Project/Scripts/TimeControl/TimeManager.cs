using System;
using System.Collections;
using UnityEngine;


namespace antoinegleisberg.HOA
{
    public enum Season
    {
        Spring,
        Summer, 
        Fall,
        Winter
    }

    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }

        public Season CurrentSeason { get; private set; }

        public event Action<Season> OnSeasonChanged;
        public event Action OnDayChanged;

        [SerializeField] private readonly int _dayDurationInMinutes = 10;
        [SerializeField] private readonly int _daysPerSeason = 9;

        private int _currentDayNumber;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _currentDayNumber = 0;
        }

        private void Start()
        {
            CurrentSeason = Season.Spring;

            StartCoroutine(AdvanceTime());
        }
        
        private IEnumerator AdvanceTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(_dayDurationInMinutes * 60);
                _currentDayNumber += 1;
                OnDayChanged?.Invoke();
                if (_currentDayNumber % _daysPerSeason == 0)
                {
                    NextSeason();
                }
            }
        }

        private void NextSeason()
        {
            switch (CurrentSeason)
            {
                case Season.Spring:
                    CurrentSeason = Season.Summer;
                    break;
                case Season.Summer:
                    CurrentSeason = Season.Fall;
                    break;
                case Season.Fall:
                    CurrentSeason = Season.Winter;
                    break;
                case Season.Winter:
                    CurrentSeason = Season.Spring;
                    break;
            }
            OnSeasonChanged?.Invoke(CurrentSeason);
        }
    }
}
