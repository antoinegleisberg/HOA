using System;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class UIEvents : MonoBehaviour
    {
        public static UIEvents Instance { get; private set; }

        public event Action OnBuildMenuOpen;
        public event Action OnBuildMenuClose;
        public event Action OnSettingsMenuOpen;
        public event Action OnSettingsMenuClose;
        public event Action<string> OnBuildBuildingSelected;
        public event Action OnCancelPreview;

        private void Awake()
        {
            Instance = this;
        }

        public void OpenBuildMenu()
        {
            OnBuildMenuOpen?.Invoke();
        }

        public void CloseBuildMenu()
        {
            OnBuildMenuClose?.Invoke();
        }

        public void OpenSettingsMenu()
        {
            OnSettingsMenuOpen?.Invoke();
        }

        public void CloseSettingsMenu()
        {
            OnSettingsMenuClose?.Invoke();
        }

        public void SelectBuildingToBuild(string name)
        {
            OnBuildBuildingSelected?.Invoke(name);
        }

        public void CancelPreview()
        {
            OnCancelPreview?.Invoke();
        }
    }
}
