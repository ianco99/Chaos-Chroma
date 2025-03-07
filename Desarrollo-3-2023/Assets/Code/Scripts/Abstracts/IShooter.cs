using System;
using UnityEngine;

namespace Code.Scripts.Abstracts
{
    /// <summary>
    /// Interface for shooter
    /// </summary>
    public interface IShooter
    {
        public void SetAim(Vector2 aim);
        public void Shoot();
        public Transform GetTransform();
    }
}
