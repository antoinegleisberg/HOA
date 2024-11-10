using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace antoinegleisberg.HOA.Core
{
    public class ScriptableCitizenNeedDB : MonoBehaviour
    {
        private static Dictionary<string, ScriptableCitizenNeed> _needs;

        private static readonly string _path = "Needs";

        public static ScriptableCitizenNeed GetNeedByName(string name)
        {
            if (_needs == null)
            {
                Init();
            }

            if (!_needs.ContainsKey(name))
            {
                Debug.LogError("NeedsDB does not contain a need with the name " + name);
                return null;
            }

            return _needs[name];
        }

        public static IEnumerable<ScriptableCitizenNeed> GetAllNeeds()
        {
            if (_needs == null)
            {
                Init();
            }
            return _needs.Values;
        }
        
        private static void Init()
        {
            _needs = new Dictionary<string, ScriptableCitizenNeed>();

            ScriptableCitizenNeed[] needs = Resources.LoadAll<ScriptableCitizenNeed>(_path);

            foreach (ScriptableCitizenNeed need in needs)
            {
                _needs.Add(need.Name, need);
            }
        }
    }
}
