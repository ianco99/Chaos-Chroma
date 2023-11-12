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
        
        [Header("Detection Area:")]
        [SerializeField] private BossAreaManager areaManager;
        
        [Header("Punch:")]
        [SerializeField] private FirePunch leftPunch;
        [SerializeField] private FirePunch rightPunch;
        [SerializeField] private RetrievePunch leftRetrieve;
        [SerializeField] private RetrievePunch rightRetrieve;
       
        [Header("State Settings:")]
        [SerializeField] private TimerSettings timerSettings;
        
        // States
        private IdleState<string> idleState;
        private PunchState<string> punchState;
        private RetrieveState<string> retrieveState; 
        private CooldownState<string> cooldownState;

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

            fsm.AddState(idleState);
            fsm.AddState(punchState);
            fsm.AddState(retrieveState);
            fsm.AddState(cooldownState);

            AddTransitions();

            fsm.Init();

            fsm.SetCurrentState(fsm.GetState(initialState));
        }

        /// <summary>
        /// Add FSM transitions
        /// </summary>
        private void AddTransitions()
        {
            fsm.AddTransition(idleState, punchState, areaManager.IsPlayerInArea);
            fsm.AddTransition(punchState, retrieveState, () => punchState.Ended);
            fsm.AddTransition(retrieveState, cooldownState, () => retrieveState.Ended);
            fsm.AddTransition(cooldownState, idleState, () => !cooldownState.Active);
        }

        private void OnEnable()
        {
            punchState.onEnter += OnEnterPunchHandler;
        }

        private void OnDisable()
        {
            punchState.onEnter -= OnEnterPunchHandler;
        }

        private void Update()
        {
            fsm.Update();
        }

        /// <summary>
        /// Update target position on enter punch state
        /// </summary>
        private void OnEnterPunchHandler()
        {
            punchState.SetTargetDistance(areaManager.GetPositionDifference());
        }
    }
}
