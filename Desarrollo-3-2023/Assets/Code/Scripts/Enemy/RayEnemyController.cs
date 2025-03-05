using Code.SOs.Enemy;
using Patterns.FSM;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

        private PatrolState<int> patrolState;

        private void Awake()
        {
            InitFSM();
        }

        private void InitFSM()
        {
            fsm = new FiniteStateMachine<int>();

            patrolState = new PatrolState<int>(rb, 0, groundCheckPoint, this, transform, rayEnemySettings.patrolSettings);

            fsm.AddState(patrolState);

            fsm.SetCurrentState(patrolState);

            fsm.Init();
        }

        private void Update()
        {
            fsm.Update();

            CheckRotation();
        }

        private void FixedUpdate()
        {
            fsm.FixedUpdate();
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
        }
    }
}