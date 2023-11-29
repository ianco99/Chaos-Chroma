using Code.SOs.States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns.FSM
{
    public class DeathState<T> : BaseState<T>
    {
        public Action onTimerEnded;

        protected TimerSettings timerSettings;

        protected float currentTimer;

        public DeathState(T id, TimerSettings settings) : base(id)
        {
            timerSettings = settings;
        }

        public DeathState(T id, string name, T nextStateID, TimerSettings settings) : base(id, name)
        {
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
            onTimerEnded?.Invoke();
            Exit();
        }
    }
}