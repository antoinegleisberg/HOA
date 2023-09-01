using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA
{
    public class Building : MonoBehaviour
    {
        [field: SerializeField] public ScriptableBuilding ScriptableBuilding { get; private set; }
    }
}
