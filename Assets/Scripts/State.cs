using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class State
    {
        public String StateName { get; set; }
        public List<State> OutgoingStates;

        public State()
        {
            OutgoingStates = new List<State>();
        }
    }
}