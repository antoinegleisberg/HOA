using System.Linq;
using UnityEngine;

namespace antoinegleisberg.HOA.UI
{
    [CreateAssetMenu(fileName = "BuildMenuCategory", menuName = "ScriptableObjects/BuildMenuCategory")]
    public class ScriptableBuildMenuCategory : ScriptableObject
    {
        private static readonly string _resourcesPath = "UI/BuildMenu/Categories";

        [field: SerializeField] public BuildMenuCategory BuildMenuCategory { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        public static ScriptableBuildMenuCategory GetByCategory(BuildMenuCategory category)
        {
            return Resources.LoadAll<ScriptableBuildMenuCategory>(_resourcesPath)
                .Where(sbmc => sbmc.BuildMenuCategory == category)
                .FirstOrDefault();
        }
    }
}
