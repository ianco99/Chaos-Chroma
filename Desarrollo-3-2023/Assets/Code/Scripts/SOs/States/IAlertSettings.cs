namespace Code.Scripts.SOs.States
{
    /// <summary>
    /// Settings for the alert state
    /// </summary>
    public interface IAlertSettings
    {
        public float alertSpeed { get => alertSpeed; set => alertSpeed = value; }
        public float alertAcceleration { get => alertAcceleration; set => alertAcceleration = value; }

    }
}
