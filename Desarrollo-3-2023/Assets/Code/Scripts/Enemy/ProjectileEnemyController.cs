using Code.FOV;
using Code.SOs.Enemy;
using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.Enemy
{
    public class ProjectileEnemyController : BaseEnemyController
    {
        private FiniteStateMachine<int> fsm;

        private ProjectileEnemySettings ProjectileEnemySettings => settings as ProjectileEnemySettings;
        
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private Animator animator;
        [SerializeField] private FieldOfView fov;
        [SerializeField] private SpriteRenderer outline;

        [Header("Suspect")]
        [SerializeField] private float suspectMeter;
        [SerializeField] private float suspectUnit = 0.5f;
        [SerializeField] private float attackDelay = 0.2f;
        [SerializeField] private float damagedTime = 2.0f;
        [SerializeField] private SpriteRenderer suspectMeterSprite;
        [SerializeField] private SpriteMask suspectMeterMask;

        private Transform detectedPlayer;
        
        // States
        private PatrolState<int> patrolState;
        private AlertState<int> alertState;
        private AttackStartState<int> attackStartState;
        
        private static readonly int CharacterState = Animator.StringToHash("CharacterState");

        private void Awake()
        {
            InitFsm();
            
            fov.ToggleFindingTargets(true);
        }

        private void InitFsm()
        {
            fsm = new FiniteStateMachine<int>();
            
            patrolState = new PatrolState<int>(rb, 0, "PatrolState", groundCheckPoint, this, transform, ProjectileEnemySettings.patrolSettings);
            alertState = new AlertState<int>(rb, 1, "AlertState", this, transform, ProjectileEnemySettings.alertSettings, groundCheckPoint);
            attackStartState = new AttackStartState<int>(2, "AttackStart", ProjectileEnemySettings.attackStartSettings, outline);
            
            fsm.AddState(patrolState);
            
            fsm.AddTransition(patrolState, alertState, () => suspectMeter > ProjectileEnemySettings.alertValue);
            
            fsm.AddTransition(alertState, attackStartState, () => IsAttackTransitionable());
            fsm.AddTransition(alertState, patrolState, () => detectedPlayer == null);
            fsm.AddTransition(attackStartState, patrolState, () => !attackStartState.Active && detectedPlayer != null);
            
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
            
            state = fsm.GetCurrentState().Name;
        }
        
        private void FixedUpdate()
        {
            fsm.FixedUpdate();
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
                detectedPlayer = fov.visibleTargets[0];
                alertState.SetTarget(detectedPlayer);

                suspectMeter += suspectUnit *
                                Mathf.Clamp(
                                    fov.viewRadius - Vector3.Distance(fov.visibleTargets[0].transform.position,
                                        transform.position), 0, fov.viewRadius) * Time.deltaTime;

                if (suspectMeter < ProjectileEnemySettings.alertValue)
                {
                    detectedPlayer = null;
                    suspectMeterSprite.color = Color.white;
                }
            }

            if (suspectMeter >= ProjectileEnemySettings.alertValue)
            {
                suspectMeterSprite.color = Color.yellow;
            }
            else if (suspectMeter < ProjectileEnemySettings.suspectMeterMaximum)
            {
                suspectMeterSprite.color = Color.white;
            }

            suspectMeter = Mathf.Clamp(suspectMeter, ProjectileEnemySettings.suspectMeterMinimum, ProjectileEnemySettings.suspectMeterMaximum);


            float normalizedSuspectMeter = (suspectMeter - (ProjectileEnemySettings.suspectMeterMinimum)) /
                                           ((ProjectileEnemySettings.suspectMeterMaximum) - (ProjectileEnemySettings.suspectMeterMinimum));

            suspectMeterMask.transform.localPosition = new Vector3(0.0f,
                Mathf.Lerp(-0.798f, 0.078f, (0.078f - (-0.798f)) * normalizedSuspectMeter), 0.0f);
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
            // else if (fsm.GetCurrentState() == attackEndState)
            // {
            //     if (fov.visibleTargets.Count > 0)
            //     {
            //         var targetPos = fov.visibleTargets[0].transform.position;
            //
            //         if (targetPos.x > transform.position.x && !facingRight)
            //             Flip();
            //         else if (targetPos.x < transform.position.x && facingRight)
            //             Flip();
            //     }
            // }
        }
        
        private bool IsAttackTransitionable()
        {
            if (detectedPlayer != null)
                    return Vector3.Distance(transform.position, detectedPlayer.position) < ProjectileEnemySettings.alertSettings.alertAttackUpDistance;

            return false;
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
    }
}