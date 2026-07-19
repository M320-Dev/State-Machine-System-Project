using System.Collections.Generic;

namespace M320.StateMachineSystem
{
    public delegate void ChangeStateBranchHandler(
        IReadOnlyList<State> unchanged, 
        IReadOnlyList<State> previous, 
        IReadOnlyList<State> current);
}
