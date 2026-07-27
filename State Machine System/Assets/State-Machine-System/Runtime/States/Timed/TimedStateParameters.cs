namespace M320.StateMachineSystem
{
    public class TimedStateParameters : StateParameters
    {
        public float duration;
        public UpdateMode mode;

        public TimedStateParameters(StateName name, float duration = 0f, UpdateMode mode = UpdateMode.Update) : base(name) 
        {
            this.duration = duration;
            this.mode = mode;
        }
    }
}
