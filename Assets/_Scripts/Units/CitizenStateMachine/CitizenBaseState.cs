using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class CitizenBaseState
{
    public abstract void Init(Citizen citizen);

    public abstract void EnterState(Citizen citizen);

    public abstract void UpdateState(Citizen citizen);

    public abstract void ExitState(Citizen citizen);

    public abstract void OnDestroy(Citizen citizen);
}