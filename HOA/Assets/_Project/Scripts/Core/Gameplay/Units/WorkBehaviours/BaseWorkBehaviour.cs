using UnityEngine;
using System.Collections;

namespace antoinegleisberg.HOA.Core
{
    public abstract class BaseWorkBehaviour
    {
        protected Citizen _citizen;
        public BaseWorkBehaviour(Citizen citizen)
        {
            _citizen = citizen;
        }

        public abstract IEnumerator ExecuteWork();
    }
}
