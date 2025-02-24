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

        [Header("Animation")][SerializeField] private Animator animator;

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

        private static readonly int CharacterState = Animator.StringToHash("CharacterState");
        
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
            attackStartState = new AttackStartState<EnemyStates>(EnemyStates.AttackStart, "AttackStart", EnemySettings.attackStartSettings,
                outline);
            attackEndState =
                new AttackEndState<EnemyStates>(EnemyStates.AttackEnd, "AttackState", hitsManager.gameObject);
            damagedState = new DamagedState<EnemyStates>(EnemyStates.Damaged, "DamagedState", EnemyStates.Block,
                EnemySettings.damagedSettings, rb);
            parriedState = new DamagedState<EnemyStates>(EnemyStates.Parried, "ParriedState", EnemyStates.Block, EnemySettings.parriedSettings, rb);
            blockState = new ParryState<EnemyStates>(EnemyStates.Block, "BlockState", damageable, EnemySettings.parrySettings);
            impulseState = new DamagedState<EnemyStates>(EnemyStates.Impulsed, "ImpulseState", EnemyStates.Alert, EnemySettings.damagedSettings, rb);

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
                () => detectedPlayer != null && Vector3.Distance(trans.position, detectedPlayer.position) <
                    EnemySettings.alertSettings.alertAttackDistance);
            fsm.AddTransition(alertState, patrolState, () => detectedPlayer == null && !turnedAggro);
            fsm.AddTransition(attackStartState, attackEndState,
                () => !attackStartState.Active && detectedPlayer != null);
            fsm.AddTransition(attackEndState, alertState,
                () => !hitsManager.gameObject.activeSelf && detectedPlayer != null);

            blockState.onEnter += () =>
            {
                StartCoroutine(ParryTimer());
            };


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
            if (fsm.GetCurrentState().ID == EnemyStates.AttackStart)
                attackStartState.Release();
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

            suspectMeter = Mathf.Clamp(suspectMeter, EnemySettings.suspectMeterMinimum, EnemySettings.suspectMeterMaximum);


            var normalizedSuspectMeter = (suspectMeter - (EnemySettings.suspectMeterMinimum)) /
                                         ((EnemySettings.suspectMeterMaximum) - (EnemySettings.suspectMeterMinimum));

            suspectMeterMask.transform.localPosition = new Vector3(0.0f,
                Mathf.Lerp(-0.798f, 0.078f, (0.078f - (-0.798f)) * normalizedSuspectMeter), 0.0f);
        }

        private void CheckAttackDir()
        {
            if (detectedPlayer)
            {
                Vector3 targetPos = detectedPlayer.position;

                if (targetPos.x > transform.position.x - bodyCollider.size.x / 2.0f &&
                targetPos.x < transform.position.x + bodyCollider.size.x / 2.0f)
                {
                    Debug.Log("He is above me isn't he");
                }
            }
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

        private void OnBlockHandler(Vector2 dir)
        {
            if (dir.x > transform.position.x && !facingRight)
                Flip();
            else if (dir.x < transform.position.x && facingRight)
                Flip();
        }

        private void OnTimerEndedHandler(EnemyStates nextId)
        {
            fsm.SetCurrentState(fsm.GetState(nextId));
        }

        private void OnParriedHandler()
        {
            parriedState.SetDirection(facingRight ? Vector2.left : Vector2.right);
            fsm.SetCurrentState(parriedState);
        }

        private void OnParryHandler(Vector2 dir)
        {
            impulseState.SetDirection(dir.x > transform.position.x ? Vector2.left : Vector2.right);
            fsm.SetCurrentState(impulseState);
        }

        public void OnDeathHandler()
        {
            fsm.SetCurrentState(deathState);
        }
    }
}