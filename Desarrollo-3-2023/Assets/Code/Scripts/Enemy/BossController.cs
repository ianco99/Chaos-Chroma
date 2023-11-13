using Code.Scripts.Abstracts.Character;
using Code.Scripts.Attack;
using Code.Scripts.States;
using Code.SOs.States;
using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.Enemy
{
    public class BossController : Character
    {
        private FiniteStateMachine<string> fsm;

        [Header("FSM:")]
        [SerializeField] private string initialState;
        
        [Header("Areas:")]
        [SerializeField] private AreaDetector detectionArea;
        [SerializeField] private AreaDetector attackArea;
        
        [Header("Punch:")]
        [SerializeField] private FirePunch leftPunch;
        [SerializeField] private FirePunch rightPunch;
        [SerializeField] private RetrievePunch leftRetrieve;
        [SerializeField] private RetrievePunch rightRetrieve;
        
        [Header("Hits:")] 
        [SerializeField] private HitController leftHit;
        [SerializeField] private HitController rightHit;
       
        [Header("State Settings:")]
        [SerializeField] private TimerSettings timerSettings;
        [SerializeField] private MoveSettings moveSettings;
        
        // States
        private IdleState<string> idleState;
        private PunchState<string> punchState;
        private RetrieveState<string> retrieveState; 
        private CooldownState<string> cooldownState;
        private MovementState<string> movementState;

        private void Awake()
        {
            InitFsm();
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
            fsm.AddTransition(idleState, movementState, detectionArea.IsPlayerInArea);
            fsm.AddTransition(idleState, punchState, attackArea.IsPlayerInArea);
            
            fsm.AddTransition(movementState, punchState, attackArea.IsPlayerInArea);
            
            fsm.AddTransition(punchState, retrieveState, () => punchState.Ended);
            
            fsm.AddTransition(retrieveState, cooldownState, () => retrieveState.Ended);
            
            fsm.AddTransition(cooldownState, idleState, () => !cooldownState.Active);
        }

        private void OnEnable()
        {
            punchState.onEnter += OnEnterPunchHandler;
            cooldownState.onEnter += OnEnterCooldownHandler;
            retrieveState.onExit += OnExitRetrieveHandler;
        }

        private void OnDisable()
        {
            punchState.onEnter -= OnEnterPunchHandler;
            cooldownState.onEnter -= OnEnterCooldownHandler;
            retrieveState.onExit -= OnExitRetrieveHandler;
        }

        private void Update()
        {
            fsm.Update();
            
            movementState.dir.x = Mathf.Clamp(detectionArea.GetPositionDifference(), -1, 1);
        }
        
        private void FixedUpdate()
        {
            fsm.FixedUpdate();
        }

        /// <summary>
        /// Update target position on enter punch state
        /// </summary>
        private void OnEnterPunchHandler()
        {
            punchState.SetTargetDistance(detectionArea.GetPositionDifference());
        }

        private void OnEnterCooldownHandler()
        {
            cooldownState.Enter();
        }

        private void OnExitRetrieveHandler()
        {
            leftHit.Stop();
            rightHit.Stop();
        }
    }
}
