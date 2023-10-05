using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns.FSM
{
    public class TimerTransitionState<T> : BaseState<T>
    {
        public Action<T> onTimerEnded;

        protected T nextStateID;

        protected float currentTimer;
        protected float maxTimer;

        public TimerTransitionState(T id, string name, T nextStateID, float maxTime) : base(id, name)
        {
            this.nextStateID = nextStateID;
            this.maxTimer = maxTime;
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

            if (!(currentTimer > maxTimer)) return;
            
            Exit();
            onTimerEnded?.Invoke(nextStateID);
        }
    }
}