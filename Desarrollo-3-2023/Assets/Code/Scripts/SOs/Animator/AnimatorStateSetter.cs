using System.Collections.Generic;

namespace Code.Scripts.SOs.Animator
{
    /// <summary>
    /// Class to manage a parameter of an animator
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class AnimatorStateSetter<TKey, TValue>
    {
        private readonly string animatorStateParameter;
        private readonly UnityEngine.Animator animator;
        private readonly Dictionary<TKey, TValue> states;
        
        public AnimatorStateSetter(string animatorStateParameter, UnityEngine.Animator animator)
        {
            this.animatorStateParameter = animatorStateParameter;
            this.animator = animator;
            states = new Dictionary<TKey, TValue>();
        }
        
        /// <summary>
        /// Add given state to dictionary
        /// </summary>
        /// <param name="id">Key of the state</param>
        /// <param name="state">Value of the state</param>
        public void AddState(TKey id, TValue state)
        {
            if (states.ContainsKey(id))
                return;
            
            states.Add(id, state);
        }

        /// <summary>
        /// Remove given state from dictionary
        /// </summary>
        /// <param name="id">key of the state to remove</param>
        public void RemoveState(TKey id)
        {
            if (!states.ContainsKey(id))
                return;
            
            states.Remove(id);
        }

        /// <summary>
        /// Set the state by key
        /// </summary>
        /// <param name="id">key of the state to set</param>
        public void AnimatorSetValue(TKey id)
        {
            if (!states.ContainsKey(id))
                return;
            
            if (states[id] is int intValue)
                animator.SetInteger(animatorStateParameter, intValue);
            
            if (states[id] is bool boolValue)
                animator.SetBool(animatorStateParameter, boolValue);
            
            if (states[id] is float floatValue)
                animator.SetFloat(animatorStateParameter, floatValue);
        }
    }
}
