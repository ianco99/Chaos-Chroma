using System;
using Code.Scripts.Abstracts.Character;
using Code.Scripts.Attack;
using Code.Scripts.SOs.Animator;
using Code.Scripts.States;
using Code.SOs.States;
using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.Enemy
{
    public class BossController : Character
    {
        private FiniteStateMachine<string> fsm;

        [Header("FSM:")] [SerializeField] private string initialState;

        [Header("Areas:")] [SerializeField] private AreaDetector detectionArea;
        [SerializeField] private AreaDetector attackArea;

        [Header("Punch:")] [SerializeField] private FirePunch leftPunch;
        [SerializeField] private FirePunch rightPunch;
        [SerializeField] private RetrievePunch leftRetrieve;
        [SerializeField] private RetrievePunch rightRetrieve;

        [Header("Hits:")] [SerializeField] private HitController leftHit;
        [SerializeField] private HitController rightHit;

        [Header("State Settings:")] [SerializeField]
        private TimerSettings timerSettings;

        [SerializeField] private MoveSettings moveSettings;
        [SerializeField] private DamagedSettings damagedSettings;

        [Header("Life:")] [SerializeField] private Damageable damageable;

        [Header("Animations:")] 
        [SerializeField] private string animatorParameterName;
        [SerializeField] private Animator animator;
        
        public static event Action<Vector2> OnBurst;
        
        private AnimatorStateSetter<string, int> animatorStateSetter;

        // States
        private IdleState<string> idleState;
        private PunchState<string> punchState;
        private RetrieveState<string> retrieveState;
        private CooldownState<string> cooldownState;
        private MovementState<string> movementState;
        private DamagedState<string> damagedState;

        private void Awake()
        {
            InitFsm();
            InitStateSetter();
        }

        /// <summary>
        /// Initialize Finite State Machine
        /// </summary>
        private void InitFsm()
        {
            fsm = new FiniteStateMachine<string>();

            idleState = new IdleState<string>("Idle");
            punchState = new PunchState<string>("Punch", leftPunch, rightPunch);
            retrieveState = new RetrieveState<string>("Retrieve", leftRetrieve, rightRetrieve);
            cooldownState = new CooldownState<string>("Cooldown", idleState.ID, timerSettings);
            movementState = new MovementState<string>("Movement", moveSettings, transform, rb);
            damagedState = new DamagedState<string>("Damaged", idleState.ID, "Idle", damagedSettings, rb);

            fsm.AddState(idleState);
            fsm.AddState(punchState);
            fsm.AddState(retrieveState);
            fsm.AddState(cooldownState);
            fsm.AddState(movementState);

            AddTransitions();

            fsm.Init();

            fsm.SetCurrentState(fsm.GetState(initialState));
        }

        /// <summary>
        /// Add FSM transitions
        /// </summary>
        private void AddTransitions()
        {
            fsm.AddTransition(idleState, movementState, detectionArea.IsDetectableInArea);
            fsm.AddTransition(idleState, punchState, attackArea.IsDetectableInArea);

            fsm.AddTransition(movementState, punchState, attackArea.IsDetectableInArea);

            fsm.AddTransition(punchState, damagedState, () => damagedState.Active);
            fsm.AddTransition(punchState, retrieveState, () => punchState.Ended);

            fsm.AddTransition(retrieveState, damagedState, () => damagedState.Active);
            fsm.AddTransition(retrieveState, cooldownState, () => retrieveState.Ended);

            fsm.AddTransition(cooldownState, idleState, () => !cooldownState.Active);

            fsm.AddTransition(damagedState, idleState, () => !damagedState.Active);
        }

        private void OnEnable()
        {
            punchState.onEnter += OnEnterPunchHandler;
            cooldownState.onEnter += OnEnterCooldownHandler;
            damagedState.onEnter += OnEnterDamagedHandler;
            retrieveState.onExit += OnExitRetrieveHandler;
            damageable.OnTakeDamage += OnTakeDamageHandler;
            leftHit.OnParried += OnLeftParriedHandler;
            rightHit.OnParried += OnRightParriedHandler;
        }

        private void OnDisable()
        {
            punchState.onEnter -= OnEnterPunchHandler;
            cooldownState.onEnter -= OnEnterCooldownHandler;
            damagedState.onEnter -= OnEnterDamagedHandler;
            retrieveState.onExit -= OnExitRetrieveHandler;
            damageable.OnTakeDamage -= OnTakeDamageHandler;
            leftHit.OnParried -= OnLeftParriedHandler;
            rightHit.OnParried -= OnRightParriedHandler;
        }

        private void Update()
        {
            fsm.Update();
            
            animatorStateSetter.AnimatorSetValue(fsm.GetCurrentState().ID);

            movementState.dir = detectionArea.GetPositionDifference().normalized;
        }

        private void FixedUpdate()
        {
            fsm.FixedUpdate();
        }

        /// <summary>
        /// Initialize stateSetter and add states
        /// </summary>
        private void InitStateSetter()
        {
            animatorStateSetter = new AnimatorStateSetter<string, int>(animatorParameterName, animator);
            
            animatorStateSetter.AddState(idleState.ID, 0);
            animatorStateSetter.AddState(punchState.ID, 1);
            animatorStateSetter.AddState(retrieveState.ID, 2);
            animatorStateSetter.AddState(cooldownState.ID, 3);
            animatorStateSetter.AddState(movementState.ID, 4);
            animatorStateSetter.AddState(damagedState.ID, 5);
        }

        /// <summary>
        /// Update target position on enter punch state
        /// </summary>
        private void OnEnterPunchHandler()
        {
            leftHit.gameObject.SetActive(true);
            rightHit.gameObject.SetActive(true);
            
            punchState.SetTargetPos(detectionArea.GetPositionDifference());
        }

        /// <summary>
        /// Set cooldown state as active when entered
        /// </summary>
        private void OnEnterCooldownHandler()
        {
            cooldownState.Enter();
        }

        /// <summary>
        /// Set damaged state when entered
        /// </summary>
        private void OnEnterDamagedHandler()
        {
            damagedState.Enter();
        }

        /// <summary>
        /// Stop Hit when exited retrieve
        /// </summary>
        private void OnExitRetrieveHandler()
        {
            leftHit.Stop();
            rightHit.Stop();
        }

        /// <summary>
        /// Set damaged state on parried
        /// </summary>
        private void OnLeftParriedHandler()
        {
            ResetPunches();

            damagedState.SetDirection(Vector2.left);
            damagedState.Enter();
        }

        /// <summary>
        /// Set damaged state on parried
        /// </summary>
        private void OnRightParriedHandler()
        {
            ResetPunches();

            damagedState.SetDirection(Vector2.right);
            damagedState.Enter();
        }

        /// <summary>
        /// Set damaged state on take damage
        /// </summary>
        /// <param name="origin">Origin of damage</param>
        private void OnTakeDamageHandler(Vector2 origin)
        {
            ResetPunches();
            
            damagedState.SetDirection(origin.x > transform.position.x ? Vector2.left : Vector2.right);
            damagedState.Enter();
            
            OnBurst?.Invoke(transform.position);
        }

        /// <summary>
        /// Instantly reset punches to original state
        /// </summary>
        private void ResetPunches()
        {
            leftHit.Stop();
            rightHit.Stop();
            punchState.Stop();
            retrieveState.Reset();
        }
    }
}