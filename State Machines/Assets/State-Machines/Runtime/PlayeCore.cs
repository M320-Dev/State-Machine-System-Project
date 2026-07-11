using UnityEngine;

namespace M320.StateMachines
{
    public sealed class PlayeCore : MonoBehaviour
    {
        public struct Context 
        {
            public Transform transform;
        }

        private sealed class StateMachineCore : StateMachineCore<Context> 
        {
            protected override void OnUpdate(float deltaTime)
            {

            }
        }

        private StateMachineCore _core = new();

        private void Update()
        {
            _core.Update(Time.deltaTime);
        }
    }
}
