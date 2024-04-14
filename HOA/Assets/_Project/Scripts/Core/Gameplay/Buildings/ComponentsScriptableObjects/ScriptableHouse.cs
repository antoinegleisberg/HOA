using UnityEngine;

namespace antoinegleisberg.HOA
{
    [CreateAssetMenu(fileName = "NewHouse", menuName = "ScriptableObjects/Buildings/House")]
    public class ScriptableHouse : ScriptableObject
    {
        public int MaxResidents;
    }
}
