using UnityEngine;

namespace antoinegleisberg.HOA.Core
{

    [CreateAssetMenu(fileName = "NewWorkplace", menuName = "ScriptableObjects/Buildings/Workplace")]
    public class ScriptableWorkplace : ScriptableObject
    {
        [field: SerializeField] public int MaxWorkers { get; private set; }

        private void OnValidate()
        {
            if (MaxWorkers <= 0)
            {
                Debug.LogWarning("Max Workers is less than 0, setting to 1");
                MaxWorkers = 1;
            }
        }
    }
}
