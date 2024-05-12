using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [CreateAssetMenu(fileName = "NewHouse", menuName = "ScriptableObjects/Buildings/House")]
    public class ScriptableHouse : ScriptableObject
    {
        [field: SerializeField] public int MaxResidents { get; private set; }
    }
}
