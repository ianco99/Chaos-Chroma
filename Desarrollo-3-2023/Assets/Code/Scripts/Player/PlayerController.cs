using System;
using Code.Scripts.Abstracts;
using Code.Scripts.Abstracts.Character;
using Code.Scripts.Attack;
using Code.Scripts.Enemy;
using Code.Scripts.Input;
using Code.Scripts.States;
using Code.SOs.States;
using Patterns.FSM;
using UnityEngine;
using Event = AK.Wwise.Event;

namespace Code.Scripts.Player
{
    [Serializable]
    public enum PlayerStates
    {
        Idle,
        Move,
        JumpStart,
        JumpEnd,
        AttackStart,
        AttackEnd,
        Block,
        Parry,
        Damaged,
        GodMode,
        Knockback
    }

    /// <summary>
    /// Manage all player actions
    /// </summary>
    public class PlayerController : Character
    {
        [Header("Player:")] [SerializeField] private PlayerStates startState = PlayerStates.Idle;
        [SerializeField] private GameObject hit;
        [SerializeField] private Damageable damageable;
        [SerializeField] private Collider2D feet;
        [SerializeField] private PhysicsMaterial2D feetMat;
        [SerializeField] private PhysicsMaterial2D bodyMat;
        [SerializeField] private SpriteRenderer outline;
        [SerializeField] private GameObject pauseCanvas;
        [SerializeField] private float burstStrength = 10f;
        [SerializeField] private float moveDeadzone = .5f;

        [Header("StateSettings")] [SerializeField]
        private MoveSettings moveSettings;

        [SerializeField] private JumpStartSettings jumpStartSettings;
        [SerializeField] private JumpEndSettings jumpEndSettings;
        [SerializeField] private GodSettings godSettings;
        [SerializeField] private AttackStartSettings attackStartSettings;
        [SerializeField] private ParrySettings parrySettings;
        [SerializeField] private DamagedSettings damagedSettings;
        [SerializeField] private DamagedSettings knockbackSettings;

        [Header("Animation")] [SerializeField] private UnityEngine.Animator animator;

        [Header("Audio Events")] [SerializeField]
        private Event playEspada;

        [SerializeField] private Event playJump;
        [SerializeField] private Event playDefense;
        [SerializeField] private Event playHit;
        [SerializeField] private Event playFootstep;
        [SerializeField] private Event stopFootstep;
        private bool isWalking;

        // States
        private MovementState<PlayerStates> movementState;
        private IdleState<PlayerStates> idleState;
        private AttackEndState<PlayerStates> attackEndState;
        private AttackStartState<PlayerStates> attackStartState;
        private ParryState<PlayerStates> parryState;
        private BlockState<PlayerStates> blockState;
        private JumpStartState<PlayerStates> jumpStartState;
        private JumpEndState<PlayerStates> jumpEndState;
        private DamagedState<PlayerStates> damagedState;
        private GodState<PlayerStates> godState;
        private DamagedState<PlayerStates> knockbackState;

        private FiniteStateMachine<PlayerStates> fsm;
        private static readonly int CharacterState = UnityEngine.Animator.StringToHash("CharacterState");
        private static readonly int Grounded = UnityEngine.Animator.StringToHash("OnGround");
        private static readonly int DirTag = UnityEngine.Animator.StringToHash("Dir");

        private void Awake()
        {
            Transform trans = transform;

            movementState =
                new MovementState<PlayerStates>(PlayerStates.Move, "MovementState", moveSettings, trans, rb,
                    playFootstep, stopFootstep);
            idleState = new IdleState<PlayerStates>(PlayerStates.Idle, "IdleState");
            attackStartState =
                new AttackStartState<PlayerStates>(PlayerStates.AttackStart, "AttackStartState", attackStartSettings,
                    outline);
            attackEndState =
                new AttackEndState<PlayerStates>(PlayerStates.AttackEnd, "AttackEndState", hit, this, playEspada);
            parryState = new ParryState<PlayerStates>(PlayerStates.Parry, "ParryState", damageable, parrySettings);
            blockState = new BlockState<PlayerStates>(PlayerStates.Block, "BlockState", damageable);
            jumpStartState = new JumpStartState<PlayerStates>(PlayerStates.JumpStart, this, "JumpStartState",
                jumpStartSettings, playJump, trans, rb);
            jumpEndState =
                new JumpEndState<PlayerStates>(PlayerStates.JumpEnd, "JumpEndState", jumpEndSettings, trans, rb);
            damagedState = new DamagedState<PlayerStates>(PlayerStates.Damaged, "DamagedState", PlayerStates.Block,
                damagedSettings, rb);
            godState = new GodState<PlayerStates>(PlayerStates.GodMode, "GodState", godSettings, trans, rb);
            knockbackState = new DamagedState<PlayerStates>(PlayerStates.Knockback, "KnockbackState",
                PlayerStates.Block, knockbackSettings, rb);

            fsm = new FiniteStateMachine<PlayerStates>();

            fsm.AddState(movementState);
            fsm.AddState(idleState);
            fsm.AddState(attackStartState);
            fsm.AddState(attackEndState);
            fsm.AddState(parryState);
            fsm.AddState(blockState);
            fsm.AddState(jumpStartState);
            fsm.AddState(jumpEndState);
            fsm.AddState(damagedState);
            fsm.AddState(godState);
            fsm.AddState(knockbackState);

            AddTransitions();

            fsm.SetCurrentState(fsm.GetState(startState));

            fsm.Init();
        }

        private void OnEnable()
        {
            InputManager.onMove += CheckMoving;
            InputManager.onAttackPressed += CheckAttackPressed;
            InputManager.onAttackReleased += CheckAttackReleased;
            InputManager.onBlockPressed += CheckParry;
            InputManager.onBlockReleased += CheckBlock;
            InputManager.onJump += CheckJumpStart;
            InputManager.onGodMode += CheckGodMode;
            InputManager.onPause += PauseHandler;

            damageable.OnTakeDamage += KnockBack;
            damageable.OnBlock += KnockBack;

            damagedState.onEnter += OnDamagedEnterHandler;

            hit.GetComponent<HitsManager>().OnParried += OnParriedHandler;
            speedPickup += OnSpeedPickUp;
            lifePickup += OnLifePickUp;

            BossController.OnBurst += OnBurstHandler;
        }

        private void OnDisable()
        {
            InputManager.onMove -= CheckMoving;
            InputManager.onAttackPressed -= CheckAttackPressed;
            InputManager.onAttackReleased -= CheckAttackReleased;
            InputManager.onBlockPressed -= CheckParry;
            InputManager.onBlockReleased -= CheckBlock;
            InputManager.onJump -= CheckJumpStart;
            InputManager.onGodMode -= CheckGodMode;
            InputManager.onPause -= PauseHandler;

            damageable.OnTakeDamage -= KnockBack;
            damageable.OnBlock -= KnockBack;

            damagedState.onEnter -= OnDamagedEnterHandler;

            hit.GetComponent<HitsManager>().OnParried -= OnParriedHandler;
            speedPickup -= OnSpeedPickUp;
            lifePickup -= OnLifePickUp;

            BossController.OnBurst -= OnBurstHandler;

            //TODO: Move to a better place
            moveSettings.speed = 300.0f;
        }

        private void Update()
        {
            CheckJumpEnd();
            CheckRotation();
            UpdateAnimationState();
            UpdateMaterial();

            fsm.Update();
        }

        private void FixedUpdate()
        {
            fsm.FixedUpdate();
        }

        /// <summary>
        /// Overrides the base character's Flip method to additionally update the damaged state's direction.
        /// </summary>
        /// <remarks>
        /// This method flips the character's orientation and updates the direction of the damaged state based on the current facing direction.
        /// </remarks>
        protected override void Flip()
        {
            base.Flip();

            damagedState.SetDirection(facingRight ? -transform.right : transform.right);
        }

        /// <summary>
        /// Control player facing direction
        /// </summary>
        private void CheckRotation()
        {
            if (fsm.GetCurrentState().ID == PlayerStates.AttackStart)
                return;

            switch (movementState.dir.x)
            {
                case > 0.2f:
                {
                    if (!facingRight)
                        Flip();

                    break;
                }
                case < -0.2f:
                {
                    if (facingRight)
                        Flip();

                    break;
                }
            }
        }

        /// <summary>
        /// Sets the parameter for the animator states
        /// </summary>
        private void UpdateAnimationState()
        {
            animator.SetInteger(CharacterState, (int)fsm.GetCurrentState().ID);
            animator.SetBool(Grounded, movementState.IsGrounded());
            animator.SetInteger(DirTag, attackEndState.Dir);
        }

        /// <summary>
        /// Changes the physics material when on air
        /// </summary>
        private void UpdateMaterial()
        {
            feet.sharedMaterial = movementState.IsGrounded() ? feetMat : bodyMat;
        }

        /// <summary>
        /// Adds transitions between states in the state machine
        /// </summary>
        private void AddTransitions()
        {
            fsm.AddTransition(idleState, damagedState, () => damagedState.Active);
            fsm.AddTransition(idleState, godState, () => godState.Active);
            fsm.AddTransition(idleState, movementState, () => movementState.Active);
            fsm.AddTransition(idleState, attackStartState, () => attackStartState.Active);
            fsm.AddTransition(idleState, parryState, () => parryState.Active);
            fsm.AddTransition(idleState, jumpStartState, () => jumpStartState.Active);
            fsm.AddTransition(idleState, jumpEndState, () => jumpEndState.Active);

            fsm.AddTransition(movementState, damagedState, () => damagedState.Active);
            fsm.AddTransition(movementState, godState, () => godState.Active);
            fsm.AddTransition(movementState, jumpStartState, () => jumpStartState.Active);
            fsm.AddTransition(movementState, idleState, () => !movementState.Active);
            fsm.AddTransition(movementState, attackStartState, () => attackStartState.Active);
            fsm.AddTransition(movementState, parryState, () => parryState.Active);
            fsm.AddTransition(movementState, jumpEndState, () => jumpEndState.Active);

            fsm.AddTransition(attackStartState, damagedState, () => damagedState.Active);
            fsm.AddTransition(attackStartState, attackEndState, () => !attackStartState.Active);

            fsm.AddTransition(attackEndState, damagedState, () => damagedState.Active);
            fsm.AddTransition(attackEndState, idleState, () => !attackEndState.Active);

            fsm.AddTransition(parryState, blockState, () => !parryState.Active);

            fsm.AddTransition(blockState, damagedState, () => damagedState.Active);
            fsm.AddTransition(blockState, godState, () => godState.Active);
            fsm.AddTransition(blockState, idleState, () => !blockState.Active);

            fsm.AddTransition(jumpStartState, damagedState, () => damagedState.Active);
            fsm.AddTransition(jumpStartState, godState, () => godState.Active);
            fsm.AddTransition(jumpStartState, jumpEndState, () => !jumpStartState.Active);

            fsm.AddTransition(jumpEndState, damagedState, () => damagedState.Active);
            fsm.AddTransition(jumpEndState, godState, () => godState.Active);
            fsm.AddTransition(jumpEndState, idleState, () => !jumpEndState.Active);

            fsm.AddTransition(damagedState, blockState, () => blockState.Active && !damagedState.Active);
            fsm.AddTransition(damagedState, idleState, () => !damagedState.Active);

            fsm.AddTransition(godState, idleState, () => !godState.Active);
        }

        /// <summary>
        /// Handler for when player enters state "Damaged"
        /// </summary>
        /// <returns></returns>
        private void OnDamagedEnterHandler()
        {
            if (attackStartState.Active)
                attackStartState.Exit();
            playHit.Post(gameObject);

            if (attackEndState.Active)
                attackEndState.Stop();
        }

        /// <summary>
        /// Handles the speed pickup event by increasing the player's speed.
        /// </summary>
        /// <param name="speedBump">The amount to increase the player's speed.</param>
        private void OnSpeedPickUp(float speedBump)
        {
            moveSettings.speed += speedBump;
        }

        /// <summary>
        /// Handles the life pickup event by healing the player's damageable.
        /// </summary>
        /// <param name="healBump">The amount to heal the player's damageable.</param>
        private void OnLifePickUp(float healBump)
        {
            damageable.Heal(healBump);
        }

        /// <summary>
        /// Handler for enter pause
        /// </summary>
        private void PauseHandler()
        {
            if (pauseCanvas)
                pauseCanvas.SetActive(GameManager.Pause());
        }

        /// <summary>
        /// Handle boss burst
        /// </summary>
        /// <param name="burstOrigin">Center of the burst</param>
        private void OnBurstHandler(Vector2 burstOrigin)
        {
            Vector2 dir = (Vector2)transform.position - burstOrigin;
            dir.y = 0;
            dir.Normalize();

            rb.AddForce(dir * burstStrength, ForceMode2D.Impulse);
        }

        #region State activations

        /// <summary>
        /// Handle to move transition
        /// </summary>
        /// <param name="input"></param>
        private void CheckMoving(Vector2 input)
        {
            if (Mathf.Abs(input.x) > moveDeadzone)
            {
                movementState.Enter();
                if (!isWalking)
                    isWalking = true;
            }

            if (input.x == 0 && isWalking)
                isWalking = false;

            movementState.dir = input;
            jumpStartState.dir = input;
            jumpEndState.dir = input;
            godState.dir = input;

            attackEndState.SetDir(input);
        }

        /// <summary>
        /// Handle to attack start transition
        /// </summary>
        private void CheckAttackPressed()
        {
            if (fsm.GetCurrentState().ID == PlayerStates.Idle || fsm.GetCurrentState().ID == PlayerStates.Move)
                attackStartState.Enter();
        }

        /// <summary>
        /// Handle to attack end transition
        /// </summary>
        private void CheckAttackReleased()
        {
            if (fsm.GetCurrentState().ID == PlayerStates.AttackStart)
                attackStartState.Release();

            if (fsm.GetCurrentState().ID == PlayerStates.AttackStart)
                attackEndState.Enter();
        }

        /// <summary>
        /// Handle to parry transition
        /// </summary>
        private void CheckParry()
        {
            if (fsm.GetCurrentState().ID != PlayerStates.Idle && fsm.GetCurrentState().ID != PlayerStates.Move) return;

            parryState.Enter();
            blockState.Enter();
        }

        /// <summary>
        /// Handle to block transition
        /// </summary>
        private void CheckBlock()
        {
            blockState.Exit();
            playDefense.Post(gameObject);
        }

        /// <summary>
        /// Handle to jump start transition
        /// </summary>
        private void CheckJumpStart()
        {
            jumpStartState.Enter();
        }

        /// <summary>
        /// Handle to jump end transition
        /// </summary>
        private void CheckJumpEnd()
        {
            if (rb.velocity.y < -0.5f)
                jumpEndState.Enter();
        }

        /// <summary>
        /// Handle to god mode transition
        /// </summary>
        private void CheckGodMode()
        {
            godState.Toggle();
        }

        /// <summary>
        /// Handle to damaged transition
        /// </summary>
        private void KnockBack(Vector2 pos)
        {
            if (transform.position.x > pos.x && facingRight)
                Flip();
            else if (transform.position.x < pos.x && !facingRight)
                Flip();

            damagedState.SetDirection(facingRight ? Vector2.left : Vector2.right);
            damagedState.Enter();
        }

        private void OnParriedHandler()
        {
            knockbackState.SetDirection(facingRight ? Vector2.left : Vector2.right);
            fsm.SetCurrentState(knockbackState);
            //knockbackState.Enter();
        }

        #endregion

#if UNITY_EDITOR
        private void OnGUI()
        {
            GUI.backgroundColor = new Color(0.5f, 0.5f, 0.5f, 0.25f);

            float squareSize = 100f;
            float padding = 10f;
            Rect squareRect = new Rect(Screen.width - squareSize - padding, padding, squareSize, squareSize);

            GUI.Box(squareRect, "");

            GUI.TextField(squareRect, fsm.GetCurrentState().Name);
        }
#endif
    }
}