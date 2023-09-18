        using System;
using UnityEngine;
using Patterns.FSM;
using Code.FOV;

namespace Code.SOs.Enemy
{

    [Serializable]
    public enum EnemyStates
    {
        Patrol,
        Attack,
        Block,
        Parry,
        Fall
    }

    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemyStates startingState;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private EnemySettings settings;
        [SerializeField] private FieldOfView fov;
        [SerializeField] private SpriteMask fovMask;
        [SerializeField] private float suspectMeter = 0.0f;
        [SerializeField] private float suspectUnit = 0.5f;

        private FiniteStateMachine<EnemyStates> fsm;
        private PatrolState<EnemyStates> patrolState;

        private void Awake()
        {
            fsm = new FiniteStateMachine<EnemyStates>();

            patrolState = new PatrolState<EnemyStates>(rb, EnemyStates.Patrol, "PatrolState", transform, settings);
            fsm = new FiniteStateMachine<EnemyStates>();

            fsm.AddState(patrolState);

            fsm.SetCurrentState(fsm.GetState(startingState));

            fsm.Init();

            fov.ToggleFindingTargets(true);
        }

        private void Update()
        {
            fsm.Update();

            CheckFieldOfView();
            CheckTransitions();
        }

        private void FixedUpdate()
        {
            fsm.FixedUpdate();
        }

        private void CheckFieldOfView()
        {
            if(fov.visibleTargets.Count > 0)
            {
                suspectMeter += suspectUnit * Mathf.Clamp(fov.viewRadius - Vector3.Distance(fov.visibleTargets[0].transform.position, transform.position), 0, fov.viewRadius) * Time.deltaTime;

                suspectMeter = Mathf.Clamp(suspectMeter, settings.suspectMeterMinimum, settings.suspectMeterMaximum);

                float normalizedSuspectMeter = (suspectMeter - (settings.suspectMeterMinimum)) / ((settings.suspectMeterMaximum) - (settings.suspectMeterMinimum));

                float totalUnits = (0.078f - (-0.798f));
                float yValue = totalUnits * normalizedSuspectMeter; //Max * 0-1 value

                fovMask.transform.localPosition = new Vector3(0.0f, Mathf.Lerp(-0.798f, 0.078f, yValue), 0.0f);
            }

            
        }

        private void CheckTransitions()
        {

        }
    }
}