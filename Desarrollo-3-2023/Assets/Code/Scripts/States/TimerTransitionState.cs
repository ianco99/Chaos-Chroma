using System;
using Code.SOs.States;
using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Timer transition base state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TimerTransitionState<T> : BaseState<T>
    {
        public Action<T> onTimerEnded;

        protected T nextStateID;
        protected TimerSettings timerSettings;

        protected float currentTimer;

        public TimerTransitionState(T id, T nextStateID, TimerSettings settings) : base(id)
        {
            this.nextStateID = nextStateID;
            timerSettings = settings;
        }
        
        public TimerTransitionState(T id, string name, T nextStateID, TimerSettings settings) : base(id, name)
        {
            this.nextStateID = nextStateID;
            timerSettings = settings;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            currentTimer = 0;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            currentTimer += Time.deltaTime;

            if (!(currentTimer > timerSettings.maxTime)) return;
            
            Exit();
            onTimerEnded?.Invoke(nextStateID);
        }
    }
}