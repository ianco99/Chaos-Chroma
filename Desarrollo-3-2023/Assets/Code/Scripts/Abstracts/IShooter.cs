using System;
using UnityEngine;

namespace Code.Scripts.Abstracts
{
    public interface IShooter
    {
        public void SetAim(Vector2 aim);
        public void Shoot();
        public Transform GetTransform();
    }
}
