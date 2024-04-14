using UnityEngine;

namespace antoinegleisberg.HOA
{

    [CreateAssetMenu(fileName = "NewWorkplace", menuName = "ScriptableObjects/Buildings/Workplace")]
    public class ScriptableWorkplace : ScriptableObject
    {
        public int MaxWorkers;
    }
}
