namespace Code.Scripts.States.Settings.Interfaces
{
    /// <summary>
    /// Settings for the knockback block
    /// </summary>
    public interface IKnockbackBlockSettings
    {
        public float knockbackForce { get => knockbackForce; set => knockbackForce = value; }
    }
}
