using Code.FOV;
using Code.Scripts.Abstracts.Character;
using Code.SOs.Enemy;
using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.Enemy
{
    public class ProjectileEnemyController : Character
    {
        private FiniteStateMachine<int> fsm;
        
        // States
        private PatrolState<int> patrolState;

        private ProjectileEnemySettings settings;
        
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private Animator animator;
        [SerializeField] private FieldOfView fov;
        
        [Header("Suspect")]
        [SerializeField] private float suspectMeter;
        [SerializeField] private float suspectUnit = 0.5f;
        [SerializeField] private float attackDelay = 0.2f;
        [SerializeField] private float damagedTime = 2.0f;
        [SerializeField] private SpriteRenderer suspectMeterSprite;
        [SerializeField] private SpriteMask suspectMeterMask;

        private Transform detectedPlayer;
        
        private static readonly int CharacterState = Animator.StringToHash("CharacterState");

        private void Awake()
        {
            InitFsm();
            
            fov.ToggleFindingTargets(true);
        }

        private void InitFsm()
        {
            fsm = new FiniteStateMachine<int>();
            
            patrolState = new PatrolState<int>(rb, 0, groundCheckPoint, this, transform, settings.patrolSettings);
            
            fsm.AddState(patrolState);
            
            // fsm.AddTransition(patrolState, alertState, () => suspectMeter > settings.alertValue);
            
            fsm.SetCurrentState(patrolState);
            
            fsm.Init();
        }
        
        private void Update()
        {
            fsm.Update();
            
            CheckRotation();
            CheckFieldOfView();
            UpdateAnimationState();
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
                // alertState.SetTarget(detectedPlayer);

                suspectMeter += suspectUnit *
                                Mathf.Clamp(
                                    fov.viewRadius - Vector3.Distance(fov.visibleTargets[0].transform.position,
                                        transform.position), 0, fov.viewRadius) * Time.deltaTime;

                if (suspectMeter < settings.alertValue)
                {
                    detectedPlayer = null;
                    suspectMeterSprite.color = Color.white;
                }
            }

            if (suspectMeter >= settings.alertValue)
            {
                suspectMeterSprite.color = Color.yellow;
            }
            else if (suspectMeter < settings.suspectMeterMaximum)
            {
                suspectMeterSprite.color = Color.white;
            }

            suspectMeter = Mathf.Clamp(suspectMeter, settings.suspectMeterMinimum, settings.suspectMeterMaximum);


            var normalizedSuspectMeter = (suspectMeter - (settings.suspectMeterMinimum)) /
                                         ((settings.suspectMeterMaximum) - (settings.suspectMeterMinimum));

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
            // else if (fsm.GetCurrentState() == alertState)
            // {
            //     switch (alertState.dir.x)
            //     {
            //         case > 0:
            //             {
            //                 if (!facingRight)
            //                 {
            //                     Flip();
            //                 }
            //
            //                 break;
            //             }
            //         case < 0:
            //             {
            //                 if (facingRight)
            //                 {
            //                     Flip();
            //                 }
            //
            //                 break;
            //             }
            //     }
            // }
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

    }
}