using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class Speedup : MonoBehaviour
    {
        [Range(0.2f, 10f)]
        [SerializeField] private float _timeScale;

        private void Awake()
        {
            Time.timeScale = _timeScale;
        }

        private void OnValidate()
        {
            Time.timeScale = _timeScale;
        }
    }
}
