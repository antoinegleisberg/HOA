using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [CreateAssetMenu(fileName = "New Citizen Need", menuName = "ScriptableObjects/Citizen Need")]
    public class ScriptableCitizenNeed : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
    }
}
