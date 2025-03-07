using System;

namespace Patterns.FSM
{
    /// <summary>
    /// FSM state transition
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Transition<T>
    {
        public Func<bool> Condition { get; }
        public BaseState<T> To { get; }

        /// <summary>
        /// Creates a transition to another state given a condition.
        /// </summary>
        /// <param name="to">The state to transition to.</param>
        /// <param name="condition">The condition to check for transition.</param>
        public Transition(BaseState<T> to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }
}

