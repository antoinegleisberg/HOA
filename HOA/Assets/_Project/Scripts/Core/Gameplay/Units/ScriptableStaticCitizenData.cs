using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    [CreateAssetMenu(fileName = "CitizenData", menuName = "ScriptableObjects/CitizenData")]
    public class ScriptableStaticCitizenData : ScriptableObject
    {
        [field: SerializeField] public float WanderingDistance { get; private set; } = 10;
        [field: SerializeField] public float TimeAtWork { get; private set; } = 10;
        [field: SerializeField] public float TimeAtHome { get; private set; } = 10;
        [field: SerializeField] public float TimeWandering { get; private set; } = 10;

        [field: SerializeField] public float ProbabilityToSpawnBaby { get; private set; } = 0.1f;
        [field: SerializeField] public int CooldownInDaysBeforeNextBaby { get; private set; } = 5;

        [field: SerializeField] public float Speed { get; private set; } = 0.5f;

        [field: SerializeField] public int InventorySize { get; private set; } = 25;
    }
}
