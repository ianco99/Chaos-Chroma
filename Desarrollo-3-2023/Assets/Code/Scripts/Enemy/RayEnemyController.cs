using Code.FOV;
using Code.Scripts.Attack;
using Code.Scripts.States;
using Code.SOs.Enemy;
using Patterns.FSM;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Enemy
{
    /// <summary>
    /// Controller for ray enemy
    /// </summary>
    public class RayEnemyController : BaseEnemyController
    {
        private FiniteStateMachine<int> fsm;

        private RayEnemySettings rayEnemySettings => settings as RayEnemySettings;

        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private FieldOfView fov;
        [SerializeField] private Animator animator;

        [Header("Suspect")]
        [SerializeField] private float suspectMeter;
        [SerializeField] private float suspectUnit = 0.5f;
        [SerializeField] private float attackDelay = 0.2f;
        [SerializeField] private float damagedTime = 2.0f;
        [SerializeField] private RayLauncher rayLauncher;
        [SerializeField] private SpriteRenderer headSprite;
        [SerializeField] private SpriteRenderer suspectMeterSprite;
        [SerializeField] private SpriteMask suspectMeterMask;
        [SerializeField] private SpriteRenderer outline;

        private Transform detectedPlayer;

        private Transform DetectedPlayer
        {
            get => detectedPlayer;
            set
            {
                detectedPlayer = value;
                shootState.SetTarget(value);
            }
        }

        private static readonly int CharacterState = Animator.StringToHash("CharacterState");

        private PatrolState<int> patrolState;
        private AlertState<int> alertState;
        private AttackStartState<int> attackStartState;
        private ShootState<int> shootState;


        private void Awake()
        {
            InitFSM();

            fov.ToggleFindingTargets(true);
        }

        private void OnEnable()
        {
            shootState.onTimerEnded += OnTimerStateEndedHandler;
        }

        private void InitFSM()
        {
            fsm = new FiniteStateMachine<int>();

            patrolState = new PatrolState<int>(rb, 0, groundCheckPoint, this, transform, rayEnemySettings.patrolSettings);
            alertState = new AlertState<int>(rb, 1, "AlertState", this, transform, rayEnemySettings.alertSettings, groundCheckPoint);
            attackStartState = new AttackStartState<int>(2, "AttackStartState", rayEnemySettings.attackStartSettings, outline);
            shootState = new ShootState<int>(3, "ShootState", alertState.ID, rayEnemySettings.shootTimerSettings, rayLauncher);

            fsm.AddState(patrolState);
            fsm.AddState(alertState);

            fsm.AddTransition(patrolState, alertState, () => suspectMeter > rayEnemySettings.alertValue);

            fsm.AddTransition(alertState, attackStartState, IsAttackTransitionable);
            fsm.AddTransition(alertState, patrolState, () => !DetectedPlayer);

            fsm.AddTransition(attackStartState, shootState, () => !attackStartState.Active && DetectedPlayer);

            fsm.SetCurrentState(patrolState);

            fsm.Init();
        }

        private void Update()
        {
            fsm.Update();

            CheckRotation();
            CheckFieldOfView();
            UpdateAnimationState();
            ReleaseAttack();
        }

        private void FixedUpdate()
        {
            fsm.FixedUpdate();
        }

        /// <summary>
        /// Handles the timer ending event for the shoot state.
        /// </summary>
        /// <param name="nextId">The ID of the next state to transition to.</param>
        private void OnTimerStateEndedHandler(int nextId)
        {
            fsm.SetCurrentState(fsm.GetState(nextId));
        }

        private void CheckRotation()
        {
            if (fsm.GetCurrentState() == patrolState)
            {
                switch (patrolState.dir.x)
                {
                    case > 0:
                        {
                            if (!facingRight)
                            {
                                Flip();
                                headSprite.flipX = true;
                            }

                            break;
                        }
                    case < 0:
                        {
                            if (facingRight)
                            {
                                Flip();
                                headSprite.flipX = false;
                            }

                            break;
                        }
                }
            }
            else if (fsm.GetCurrentState() == alertState)
            {
                switch (alertState.dir.x)
                {
                    case > 0:
                        {
                            if (!facingRight)
                            {
                                Flip();
                            }

                            break;
                        }
                    case < 0:
                        {
                            if (facingRight)
                            {
                                Flip();
                            }

                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Sets the parameter for the animator states
        /// </summary>
        private void UpdateAnimationState()
        {
            animator.SetInteger(CharacterState, fsm.GetCurrentState().ID);
        }

        private void CheckFieldOfView()
        {
            if (fov.visibleTargets.Count <= 0)
            {
                suspectMeter -= suspectUnit * Time.deltaTime;
            }
            else
            {
                DetectedPlayer = fov.visibleTargets[0];
                alertState.SetTarget(detectedPlayer);

                suspectMeter += suspectUnit *
                                Mathf.Clamp(
                                    fov.viewRadius - Vector3.Distance(fov.visibleTargets[0].transform.position,
                                        transform.position), 0, fov.viewRadius) * Time.deltaTime;

                if (suspectMeter < rayEnemySettings.alertValue)
                {

                    DetectedPlayer = null;
                    suspectMeterSprite.color = Color.white;
                }
            }

            if (suspectMeter >= rayEnemySettings.alertValue)
            {
                suspectMeterSprite.color = Color.yellow;
            }

            else if (suspectMeter < rayEnemySettings.suspectMeterMaximum)
            {
                suspectMeterSprite.color = Color.white;
            }

            suspectMeter = Mathf.Clamp(suspectMeter, rayEnemySettings.suspectMeterMinimum, rayEnemySettings.suspectMeterMaximum);


            var normalizedSuspectMeter = (suspectMeter - (rayEnemySettings.suspectMeterMinimum)) /
                                         ((rayEnemySettings.suspectMeterMaximum) - (rayEnemySettings.suspectMeterMinimum));

            suspectMeterMask.transform.localPosition = new Vector3(0.0f,
                Mathf.Lerp(-0.798f, 0.078f, (0.078f - (-0.798f)) * normalizedSuspectMeter), 0.0f);
        }

        /// <summary>
        /// Check if enemy is preparing an attack and release it
        /// </summary>
        private void ReleaseAttack()
        {
            if (fsm.GetCurrentState().ID == attackStartState.ID && attackStartState.Active)
            {
                attackStartState.Release();
            }
        }

        /// <summary>
        /// Checks if the enemy can transition to an attack state from its current state.
        /// </summary>
        /// <returns>True if the enemy can transition to an attack state, false otherwise.</returns>
        /// <remarks>
        /// An attack state can be transitioned to if the enemy has a detected player and the distance
        /// between the enemy and the detected player is within the alert attack distance.
        /// </remarks>
        private bool IsAttackTransitionable()
        {
            if (Vector3.Distance(transform.position, DetectedPlayer.position) < rayEnemySettings.alertSettings.alertAttackUpDistance)
            {
                Debug.Log("ojo eh");
            }
            else
            {
                Debug.Log("que mierda pasa viejo");
            }

            if (DetectedPlayer != null)
                return Vector3.Distance(transform.position, DetectedPlayer.position) < rayEnemySettings.alertSettings.alertAttackUpDistance;

            return false;
        }
    }
}