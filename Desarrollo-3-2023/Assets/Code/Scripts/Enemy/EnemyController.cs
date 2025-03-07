using System;
using System.Collections;
using Code.FOV;
using Code.Scripts.Attack;
using Code.Scripts.States;
using Code.SOs.Enemy;
using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.Enemy
{
    [Serializable]
    public enum EnemyStates
    {
        Patrol,
        Alert,
        AttackStart = 4,
        AttackEnd = 5,
        Block,
        Parry,
        Damaged,
        Fall,
        Impulsed,
        Parried,
        Death
    }

    /// <summary>
    /// Controls the enemy
    /// </summary>
    public class EnemyController : BaseEnemyController
    {
        [SerializeField] private EnemyStates startingState;

        [SerializeField] private HitsManager hitsManager;
        [SerializeField] private FieldOfView fov;
        [SerializeField] private SpriteMask suspectMeterMask;
        [SerializeField] private SpriteRenderer suspectMeterSprite;
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private Damageable damageable;
        [SerializeField] private SpriteRenderer outline;
        [SerializeField] private BoxCollider2D bodyCollider;
        [SerializeField] private Color hitOutlineColor;

        [SerializeField] private float suspectMeter;
        [SerializeField] private float suspectUnit = 0.5f;
        [SerializeField] private float hitDistance = 5f;
        [SerializeField] private float attackDelay = 0.2f;
        [SerializeField] private float damagedTime = 2.0f;

        [Header("Animation")] [SerializeField] private UnityEngine.Animator animator;

        private Transform detectedPlayer;
        private bool turnedAggro;
        private bool blocking;

        private FiniteStateMachine<EnemyStates> fsm;
        private PatrolState<EnemyStates> patrolState;
        private AlertState<EnemyStates> alertState;
        private AttackStartState<EnemyStates> attackStartState;
        private AttackEndState<EnemyStates> attackEndState;
        private DamagedState<EnemyStates> damagedState;
        private DamagedState<EnemyStates> parriedState;
        private ParryState<EnemyStates> blockState;
        private DamagedState<EnemyStates> impulseState;
        private DeathState<EnemyStates> deathState;

        private static readonly int CharacterState = UnityEngine.Animator.StringToHash("CharacterState");

        private EnemySettings EnemySettings => settings as EnemySettings;

        private void Awake()
        {
            InitFSM();

            damageable.OnTakeDamage += OnTakeDamageHandler;
            damageable.OnBlock += OnBlockHandler;
            damageable.OnParry += OnParryHandler;
            damagedState.onTimerEnded += OnTimerEndedHandler;
            parriedState.onTimerEnded += OnTimerEndedHandler;
            deathState.onTimerEnded += () => Destroy(gameObject);

            fov.ToggleFindingTargets(true);

            damageable.OnDeath = EnemySettings.deathEvent;
        }

        private void OnEnable()
        {
            hitsManager.OnParried += OnParriedHandler;
        }

        private void OnDisable()
        {
            hitsManager.OnParried -= OnParriedHandler;
        }

        private void InitFSM()
        {
            fsm = new FiniteStateMachine<EnemyStates>();

            Transform trans = transform;
            patrolState = new PatrolState<EnemyStates>(rb, EnemyStates.Patrol, "PatrolState", groundCheckPoint, this,
                trans, EnemySettings.patrolSettings);
            patrolState.SetDirection(1.0f);
            alertState = new AlertState<EnemyStates>(rb, EnemyStates.Alert, "AlertState", this, trans,
                EnemySettings.alertSettings, groundCheckPoint);
            attackStartState = new AttackStartState<EnemyStates>(EnemyStates.AttackStart, "AttackStart",
                EnemySettings.attackStartSettings,
                outline);
            attackEndState =
                new AttackEndState<EnemyStates>(EnemyStates.AttackEnd, "AttackState", hitsManager.gameObject, this);
            damagedState = new DamagedState<EnemyStates>(EnemyStates.Damaged, "DamagedState", EnemyStates.Block,
                EnemySettings.damagedSettings, rb);
            parriedState = new DamagedState<EnemyStates>(EnemyStates.Parried, "ParriedState", EnemyStates.Block,
                EnemySettings.parriedSettings, rb);
            blockState = new ParryState<EnemyStates>(EnemyStates.Block, "BlockState", damageable,
                EnemySettings.parrySettings);
            impulseState = new DamagedState<EnemyStates>(EnemyStates.Impulsed, "ImpulseState", EnemyStates.Alert,
                EnemySettings.damagedSettings, rb);

            deathState = new DeathState<EnemyStates>(EnemyStates.Death, EnemySettings.deathSettings);

            fsm = new FiniteStateMachine<EnemyStates>();

            fsm.AddState(patrolState);
            fsm.AddState(alertState);
            fsm.AddState(attackEndState);
            fsm.AddState(damagedState);
            fsm.AddState(parriedState);
            fsm.AddState(blockState);
            fsm.AddState(impulseState);

            fsm.AddTransition(patrolState, alertState, () => suspectMeter > EnemySettings.alertValue);
            fsm.AddTransition(alertState, attackStartState,
                () => IsAttackTransitionable());
            fsm.AddTransition(alertState, patrolState, () => detectedPlayer == null && !turnedAggro);
            fsm.AddTransition(attackStartState, attackEndState,
                () => !attackStartState.Active && detectedPlayer != null);
            fsm.AddTransition(attackEndState, alertState,
                () => !hitsManager.gameObject.activeSelf && detectedPlayer != null);

            blockState.onEnter += () => { StartCoroutine(ParryTimer()); };


            fsm.SetCurrentState(fsm.GetState(startingState));

            fsm.Init();
        }

        private void Update()
        {
            fsm.Update();

            CheckRotation();
            CheckFieldOfView();
            CheckAttackDir();
            UpdateAnimationState();
            ReleaseAttack();
        }

        /// <summary>
        /// Timer for parry state duration.
        /// Waits for the duration set in <see cref="EnemySettings.parrySettings.duration"/>,
        /// then sets the current state to <see cref="EnemyStates.Alert"/>.
        /// </summary>
        /// <returns>Yield instruction for waiting.</returns>
        private IEnumerator ParryTimer()
        {
            yield return new WaitForSeconds(EnemySettings.parrySettings.duration);
            fsm.SetCurrentState(alertState);
        }

        /// <summary>
        /// Check if enemy is preparing an attack and release it
        /// </summary>
        private void ReleaseAttack()
        {
            if (fsm.GetCurrentState().ID == EnemyStates.AttackStart && attackStartState.Active)
            {
                attackStartState.Release();
            }
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
            animator.SetInteger(CharacterState, (int)fsm.GetCurrentState().ID);
        }

        /// <summary>
        /// Checks the field of view and updates the suspect meter accordingly.
        /// </summary>
        /// <remarks>
        /// If the field of view has no visible targets, the suspect meter decreases over time.
        /// If the field of view has visible targets, the suspect meter increases based on the distance
        /// between the closest target and the enemy.
        /// The suspect meter is then clamped to the specified minimum and maximum values.
        /// </remarks>
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

                if (suspectMeter < EnemySettings.alertValue)
                {
                    detectedPlayer = null;
                    suspectMeterSprite.color = Color.white;
                }
            }

            if (suspectMeter >= EnemySettings.alertValue)
            {
                suspectMeterSprite.color = Color.yellow;
            }
            else if (suspectMeter < EnemySettings.suspectMeterMaximum)
            {
                suspectMeterSprite.color = Color.white;
            }

            suspectMeter = Mathf.Clamp(suspectMeter, EnemySettings.suspectMeterMinimum,
                EnemySettings.suspectMeterMaximum);


            var normalizedSuspectMeter = (suspectMeter - (EnemySettings.suspectMeterMinimum)) /
                                         ((EnemySettings.suspectMeterMaximum) - (EnemySettings.suspectMeterMinimum));

            suspectMeterMask.transform.localPosition = new Vector3(0.0f,
                Mathf.Lerp(-0.798f, 0.078f, (0.078f - (-0.798f)) * normalizedSuspectMeter), 0.0f);
        }

        /// <summary>
        /// Define attack direction between up and side orientations
        /// </summary>
        private void CheckAttackDir()
        {
            if (detectedPlayer)
            {
                Vector3 targetPos = detectedPlayer.position;

                if (targetPos.x > transform.position.x - bodyCollider.size.x / 0.2f &&
                    targetPos.x < transform.position.x + bodyCollider.size.x / 0.2f &&
                    targetPos.y > hitsManager.transform.position.y + 1.0f)
                {
                    hitsManager.SetDir(Vector2.up);
                }
                else
                {
                    hitsManager.SetDir(Vector2.zero);
                }
            }
        }

        /// <summary>
        /// Adjusts the character's orientation based on its current state and direction.
        /// </summary>
        /// <remarks>
        /// - In the patrol state, the character flips its orientation to match the direction of movement.
        /// - In the alert state, the character flips its orientation based on the direction of the alert target.
        /// - In the attack end state, if there are visible targets, the character flips to face the first visible target.
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
            else if (fsm.GetCurrentState() == attackEndState)
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
        /// Handles damage being taken by the enemy.
        /// </summary>
        /// <param name="origin">The origin of the damage.</param>
        /// <remarks>
        /// - If the enemy's life reaches zero, calls <see cref="OnDeathHandler"/>.
        /// - If the enemy is in the <see cref="EnemyStates.AttackEnd"/> state, stops the state.
        /// - If the enemy is not in the <see cref="EnemyStates.Block"/> or <see cref="EnemyStates.Damaged"/> states, 
        ///   sets the direction of the <see cref="damagedState"/> based on the direction of the origin and 
        ///   sets the current state to the <see cref="damagedState"/>.
        /// </remarks>
        private void OnTakeDamageHandler(Vector2 origin)
        {
            if (damageable.GetLife() <= 0)
            {
                OnDeathHandler();
                return;
            }

            if (fsm.GetCurrentState().ID == EnemyStates.AttackEnd)
                attackEndState.Stop();

            if (origin.x > transform.position.x && !facingRight)
                Flip();
            else if (origin.x < transform.position.x && facingRight)
                Flip();

            if (fsm.GetCurrentState() != blockState && fsm.GetCurrentState() != damagedState)
            {
                damagedState.SetDirection(facingRight ? Vector2.left : Vector2.right);
                fsm.SetCurrentState(damagedState);
            }
        }

        /// <summary>
        /// Handles the character's behavior when a block action is triggered.
        /// </summary>
        /// <param name="dir">The direction vector indicating the source of the block.</param>
        /// <remarks>
        /// Flips the character's orientation based on the direction of the block relative to the character's position.
        /// </remarks>
        private void OnBlockHandler(Vector2 dir)
        {
            if (dir.x > transform.position.x && !facingRight)
                Flip();
            else if (dir.x < transform.position.x && facingRight)
                Flip();
        }

        /// <summary>
        /// Handles the timer ending event for states that transition to another state upon completion.
        /// </summary>
        /// <param name="nextId">The ID of the next state to transition to.</param>
        /// <remarks>
        /// Sets the current state of the Finite State Machine to the state with the given ID.
        /// </remarks>
        private void OnTimerEndedHandler(EnemyStates nextId)
        {
            fsm.SetCurrentState(fsm.GetState(nextId));
        }

        /// <summary>
        /// Handles parried event.
        /// </summary>
        /// <remarks>
        /// Sets the direction of the <see cref="parriedState"/> based on the character's orientation and
        /// sets the current state to the <see cref="parriedState"/>.
        /// </remarks>
        private void OnParriedHandler()
        {
            parriedState.SetDirection(facingRight ? Vector2.left : Vector2.right);
            fsm.SetCurrentState(parriedState);
        }

        /// <summary>
        /// Handles the parry action triggered on the character.
        /// </summary>
        /// <param name="dir">The direction vector indicating the source of the parry.</param>
        /// <remarks>
        /// Sets the direction of the <see cref="impulseState"/> based on the direction of the parry
        /// relative to the character's position and sets the current state to the <see cref="impulseState"/>.
        /// </remarks>
        private void OnParryHandler(Vector2 dir)
        {
            impulseState.SetDirection(dir.x > transform.position.x ? Vector2.left : Vector2.right);
            fsm.SetCurrentState(impulseState);
        }

        /// <summary>
        /// Handles the death event of the enemy.
        /// </summary>
        /// <remarks>
        /// Sets the current state of the Finite State Machine to the <see cref="deathState"/>.
        /// </remarks>
        public void OnDeathHandler()
        {
            fsm.SetCurrentState(deathState);
        }

        /// <summary>
        /// Checks if the enemy can transition to an attack state from its current state.
        /// </summary>
        /// <returns>
        /// True if the enemy can transition to an attack state, false otherwise.
        /// </returns>
        /// <remarks>
        /// An attack state can be transitioned to if the enemy has a detected player and the distance
        /// between the enemy and the detected player is within the alert attack distance.
        /// </remarks>
        private bool IsAttackTransitionable()
        {
            if (detectedPlayer != null)
            {
                if (hitsManager.GetDir() == 2)
                    return Vector3.Distance(transform.position, detectedPlayer.position) <
                           EnemySettings.alertSettings.alertAttackUpDistance;
                else
                    return Vector3.Distance(transform.position, detectedPlayer.position) <
                           EnemySettings.alertSettings.alertAttackSideDistance;
            }

            return false;
        }
    }
}