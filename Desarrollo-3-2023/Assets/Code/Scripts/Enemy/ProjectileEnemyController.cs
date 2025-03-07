using Code.FOV;
using Code.Scripts.Abstracts;
using Code.Scripts.Attack;
using Code.Scripts.States;
using Code.SOs.Enemy;
using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.Enemy
{
    /// <summary>
    /// Controller for projectile enemy
    /// </summary>
    public class ProjectileEnemyController : BaseEnemyController
    {
        private FiniteStateMachine<int> fsm;

        private ProjectileEnemySettings ProjectileEnemySettings => settings as ProjectileEnemySettings;

        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private UnityEngine.Animator animator;
        [SerializeField] private FieldOfView fov;
        [SerializeField] private SpriteRenderer outline;
        [SerializeField] private ProjectileLauncher shooter;
        [SerializeField] private Damageable damageable;

        [Header("Suspect")] [SerializeField] private float suspectMeter;
        [SerializeField] private float suspectUnit = 0.5f;
        [SerializeField] private float attackDelay = 0.2f;
        [SerializeField] private float damagedTime = 2.0f;
        [SerializeField] private SpriteRenderer suspectMeterSprite;
        [SerializeField] private SpriteMask suspectMeterMask;

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

        // States
        private PatrolState<int> patrolState;
        private AlertState<int> alertState;
        private AttackStartState<int> attackStartState;
        private ShootState<int> shootState;
        private DamagedState<int> damagedState;
        private DeathState<int> deathState;

        private static readonly int CharacterState = UnityEngine.Animator.StringToHash("CharacterState");

        private void Awake()
        {
            InitFsm();

            fov.ToggleFindingTargets(true);

            damageable.OnTakeDamage += OnTakeDamageHandler;
            damagedState.onTimerEnded += OnTimerStateEndedHandler;
            deathState.onTimerEnded += () => Destroy(gameObject);
        }

        /// <summary>
        /// Initialize the Finite State Machine with all states and transitions.
        /// </summary>
        private void InitFsm()
        {
            fsm = new FiniteStateMachine<int>();

            patrolState = new PatrolState<int>(rb, 0, "PatrolState", groundCheckPoint, this, transform,
                ProjectileEnemySettings.patrolSettings);
            alertState = new AlertState<int>(rb, 1, "AlertState", this, transform,
                ProjectileEnemySettings.alertSettings, groundCheckPoint);
            attackStartState =
                new AttackStartState<int>(2, "AttackStart", ProjectileEnemySettings.attackStartSettings, outline);
            shootState = new ShootState<int>(3, "ShootState", alertState.ID, ProjectileEnemySettings.shootTimerSettings,
                shooter);
            damagedState = new DamagedState<int>(4, "DamagedState", alertState.ID,
                ProjectileEnemySettings.damagedSettings, rb);
            deathState = new DeathState<int>(5, "DeathState", ProjectileEnemySettings.deathTimerSettings);

            fsm.AddState(patrolState);
            fsm.AddState(alertState);
            fsm.AddState(attackStartState);
            fsm.AddState(shootState);
            fsm.AddState(damagedState);
            fsm.AddState(deathState);

            fsm.AddTransition(patrolState, alertState, () => suspectMeter > ProjectileEnemySettings.alertValue);

            fsm.AddTransition(alertState, attackStartState, IsAttackTransitionable);
            fsm.AddTransition(alertState, patrolState, () => !DetectedPlayer);

            fsm.AddTransition(attackStartState, shootState, () => !attackStartState.Active && DetectedPlayer);
            fsm.AddTransition(attackStartState, patrolState, () => !attackStartState.Active);

            fsm.SetCurrentState(patrolState);

            fsm.Init();
        }

        private void OnEnable()
        {
            shootState.onTimerEnded += OnTimerStateEndedHandler;
        }

        /// <summary>
        /// Handles the timer ending event for the shoot state.
        /// </summary>
        /// <param name="nextId">The ID of the next state to transition to.</param>
        private void OnTimerStateEndedHandler(int nextId)
        {
            fsm.SetCurrentState(fsm.GetState(nextId));
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
        /// Sets the parameter for the animator states
        /// </summary>
        private void UpdateAnimationState()
        {
            animator.SetInteger(CharacterState, fsm.GetCurrentState().ID);
        }

        /// <summary>
        /// Checks the field of view and updates the suspect meter accordingly.
        /// </summary>
        private void CheckFieldOfView()
        {
            if (fov.visibleTargets.Count <= 0)
            {
                suspectMeter -= suspectUnit * Time.deltaTime;
            }
            else
            {
                DetectedPlayer = fov.visibleTargets[0];
                alertState.SetTarget(DetectedPlayer);

                suspectMeter += suspectUnit *
                                Mathf.Clamp(
                                    fov.viewRadius - Vector3.Distance(fov.visibleTargets[0].transform.position,
                                        transform.position), 0, fov.viewRadius) * Time.deltaTime;

                if (suspectMeter < ProjectileEnemySettings.alertValue)
                {
                    DetectedPlayer = null;
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

            suspectMeter = Mathf.Clamp(suspectMeter, ProjectileEnemySettings.suspectMeterMinimum,
                ProjectileEnemySettings.suspectMeterMaximum);


            float normalizedSuspectMeter = (suspectMeter - (ProjectileEnemySettings.suspectMeterMinimum)) /
                                           ((ProjectileEnemySettings.suspectMeterMaximum) -
                                            (ProjectileEnemySettings.suspectMeterMinimum));

            suspectMeterMask.transform.localPosition = new Vector3(0.0f,
                Mathf.Lerp(-0.798f, 0.078f, (0.078f - (-0.798f)) * normalizedSuspectMeter), 0.0f);
        }

        /// <summary>
        /// Adjusts the enemy's orientation based on its current state and direction.
        /// </summary>
        /// <remarks>
        /// - In the patrol state, the enemy flips to match the patrol direction.
        /// - In the alert state, the enemy flips based on the direction of the alert target.
        /// - In the attack end state (commented out), if there are visible targets, the enemy flips to face the first visible target.
        /// </remarks>
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
            else if (fsm.GetCurrentState() == attackStartState)
            {
                if (fov.visibleTargets.Count > 0)
                {
                    var targetPos = fov.visibleTargets[0].transform.position;
            
                    if (targetPos.x > transform.position.x && !facingRight)
                        Flip();
                    else if (targetPos.x < transform.position.x && facingRight)
                        Flip();
                }
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
            if (DetectedPlayer != null)
                return Vector3.Distance(transform.position, DetectedPlayer.position) <
                       ProjectileEnemySettings.alertSettings.alertAttackUpDistance;

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

        /// <summary>
        /// Handles damage being taken by the enemy.
        /// </summary>
        /// <param name="origin">The origin of the damage.</param>
        /// <remarks>
        /// - If the enemy's life reaches zero, calls <see cref="OnDeathHandler"/>.
        /// - If the enemy is in the <see cref="shootState"/> state, stops the state.
        /// - If the enemy is not in the <see cref="damagedState"/> state, sets the direction of the <see cref="damagedState"/> based on the direction of the origin and
        ///   sets the current state to the <see cref="damagedState"/>.
        /// </remarks>
        private void OnTakeDamageHandler(Vector2 origin)
        {
            if (damageable.GetLife() <= 0)
            {
                OnDeathHandler();
                return;
            }

            // if (fsm.GetCurrentState() == shootState)
            //     shootState.Stop();

            if (origin.x > transform.position.x && !facingRight)
                Flip();
            else if (origin.x < transform.position.x && facingRight)
                Flip();

            if (fsm.GetCurrentState() != damagedState)
            {
                damagedState.SetDirection(facingRight ? Vector2.left : Vector2.right);
                fsm.SetCurrentState(damagedState);
            }
        }

        /// <summary>
        /// Handles the death event of the enemy.
        /// </summary>
        /// <remarks>
        /// Sets the current state of the Finite State Machine to the <see cref="deathState"/>.
        /// </remarks>
        private void OnDeathHandler()
        {
            fsm.SetCurrentState(deathState);
        }
    }
}