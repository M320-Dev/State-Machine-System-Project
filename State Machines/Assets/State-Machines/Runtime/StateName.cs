using System;

namespace M320.StateMachines
{
    public readonly struct StateName
    {
        private readonly string _value;

        private StateName(string value) 
        {
            _value = value;
        }

        public bool IsNullOrWhiteSpace => string.IsNullOrWhiteSpace(_value);
        
        public static StateName Empty = new(string.Empty);

        public static implicit operator string(StateName name) => name._value;
        public static implicit operator StateName(string value) => new(value);
        public static implicit operator StateName(Enum value) => new(value.ToString());
    }
}
