using System;
using Code.Scripts.Abstracts.Character;
using Code.Scripts.Input;
using Code.Scripts.States;
using Patterns.FSM;
using UnityEngine;

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
        GodMode
    }

    /// <summary>
    /// Manage all player actions
    /// </summary>
    public class PlayerController : Character
    {
        [Header("Player:")] [SerializeField] private PlayerStates startState = PlayerStates.Idle;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float acceleration = 5f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float parryDuration = 1f;
        [SerializeField] private float throwBackForce = 5f;
        [SerializeField] private GameObject hit;
        [SerializeField] private GameObject parryCapsule;
        [SerializeField] private GameObject blockCapsule;
        [SerializeField] private Damageable damageable;
        [SerializeField] private Collider2D feet;
        [SerializeField] private PhysicsMaterial2D feetMat;
        [SerializeField] private PhysicsMaterial2D bodyMat;

        [Header("Animation")] [SerializeField] private Animator animator;

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

        private FiniteStateMachine<PlayerStates> fsm;
        private static readonly int CharacterState = Animator.StringToHash("CharacterState");

        private void Awake()
        {
            Transform trans = transform;

            movementState =
                new MovementState<PlayerStates>(PlayerStates.Move, "MovementState", speed, acceleration, trans, rb);
            idleState = new IdleState<PlayerStates>(PlayerStates.Idle, "IdleState");
            attackStartState = new AttackStartState<PlayerStates>(PlayerStates.AttackStart, "AttackStartState");
            attackEndState = new AttackEndState<PlayerStates>(PlayerStates.AttackEnd, "AttackEndState", hit);
            parryState = new ParryState<PlayerStates>(PlayerStates.Parry, "ParryState", damageable, parryDuration);
            blockState = new BlockState<PlayerStates>(PlayerStates.Block, "BlockState", damageable);
            jumpStartState = new JumpStartState<PlayerStates>(PlayerStates.JumpStart, this, "JumpStartState", speed,
                acceleration, trans, rb, jumpForce);
            jumpEndState =
                new JumpEndState<PlayerStates>(PlayerStates.JumpEnd, "JumpEndState", speed, acceleration, trans, rb);
            damagedState =
                new DamagedState<PlayerStates>(PlayerStates.Damaged, "DamagedState", PlayerStates.Idle, .2f,
                    throwBackForce, rb);
            godState = new GodState<PlayerStates>(PlayerStates.GodMode, "GodState", 5f, acceleration, trans, rb);

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

            damageable.OnTakeDamage += KnockBack;
            damageable.OnBlock += KnockBack;
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

            damageable.OnTakeDamage -= KnockBack;
            damageable.OnBlock -= KnockBack;
        }

        private void Update()
        {
            CheckPlayerState();
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
        /// Sets player's current state
        /// </summary>
        private void CheckPlayerState()
        {
            blockCapsule.SetActive(false);
            parryCapsule.SetActive(false);

            switch (fsm.GetCurrentState().ID)
            {
                case PlayerStates.Idle:
                    IdleTransitions();
                    break;

                case PlayerStates.Move:
                    MoveTransitions();
                    break;

                case PlayerStates.JumpStart:
                    JumpStartTransitions();
                    break;

                case PlayerStates.AttackStart:
                    AttackStartTransitions();
                    break;

                case PlayerStates.AttackEnd:
                    AttackEndTransitions();
                    break;

                case PlayerStates.Block:
                    blockCapsule.SetActive(true);
                    BlockTransitions();
                    break;

                case PlayerStates.Parry:
                    parryCapsule.SetActive(true);
                    ParryTransitions();
                    break;

                case PlayerStates.JumpEnd:
                    JumpEndTransitions();
                    break;

                case PlayerStates.Damaged:
                    DamagedTransitions();
                    break;

                case PlayerStates.GodMode:
                    GodStateTransitions();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

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
                case > 0:
                {
                    if (!facingRight)
                        Flip();

                    break;
                }
                case < 0:
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
        }

        /// <summary>
        /// Changes the physics material when on air
        /// </summary>
        private void UpdateMaterial()
        {
            feet.sharedMaterial = movementState.IsGrounded() ? feetMat : bodyMat;
        }

        #region State activations

        /// <summary>
        /// Handle to move transition
        /// </summary>
        /// <param name="input"></param>
        private void CheckMoving(Vector2 input)
        {
            if (input.x != 0 || input.y != 0)
                movementState.Enter();

            movementState.dir = input;
            jumpStartState.dir = input;
            jumpEndState.dir = input;
            godState.dir = input;
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
            if (rb.velocity.y < 0)
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

            damagedState.Enter();
        }

        #endregion

        #region Transitions

        /// <summary>
        /// Idle state transitions manager
        /// </summary>
        private void IdleTransitions()
        {
            if (damagedState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Damaged));
            else if (godState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.GodMode));
            else if (movementState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Move));
            else if (attackStartState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.AttackStart));
            else if (parryState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Parry));
            else if (jumpStartState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.JumpStart));
        }

        /// <summary>
        /// Move state transitions manager
        /// </summary>
        private void MoveTransitions()
        {
            if (damagedState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Damaged));
            else if (godState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.GodMode));
            else if (!movementState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Idle));
            else if (jumpStartState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.JumpStart));
            else if (attackStartState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.AttackStart));
            else if (parryState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Parry));
        }

        /// <summary>
        /// Attack start state transitions manager
        /// </summary>
        private void AttackStartTransitions()
        {
            if (damagedState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Damaged));
            else if (!attackStartState.Active && attackEndState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.AttackEnd));
        }

        /// <summary>
        /// Attack end state transitions manager
        /// </summary>
        private void AttackEndTransitions()
        {
            if (damagedState.Active)
            {
                attackEndState.Stop();
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Damaged));
            }
            else if (!attackEndState.Active)
            {
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Idle));
            }
        }

        /// <summary>
        /// Parry state transitions manager
        /// </summary>
        private void ParryTransitions()
        {
            if (!parryState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Block));
        }

        /// <summary>
        /// Block state transitions manager
        /// </summary>
        private void BlockTransitions()
        {
            if (damagedState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Damaged));
            else if (godState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.GodMode));
            else if (!blockState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Idle));
        }

        /// <summary>
        /// Jump start state transitions manager
        /// </summary>
        private void JumpStartTransitions()
        {
            if (damagedState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Damaged));
            else if (godState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.GodMode));
            else if (!jumpStartState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.JumpEnd));
        }

        /// <summary>
        /// Jump end state transitions manager
        /// </summary>
        private void JumpEndTransitions()
        {
            if (damagedState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Damaged));
            else if (godState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.GodMode));
            else if (!jumpEndState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Idle));
        }

        /// <summary>
        /// Damaged state transitions manager
        /// </summary>
        private void DamagedTransitions()
        {
            if (!damagedState.Active && blockState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Block));
            else if (!damagedState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Idle));
        }

        /// <summary>
        /// God mode state transitions manager
        /// </summary>
        private void GodStateTransitions()
        {
            if (!godState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Idle));
        }

        #endregion
    }
}