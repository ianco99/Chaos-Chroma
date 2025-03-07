namespace Code.Scripts.Abstracts
{
    /// <summary>
    /// Interface for publisher
    /// </summary>
    public abstract class Publisher : IPublisher
    {
        public abstract string Name { get; }
        public abstract void Publish();

        public abstract void Subscribe();
    }
}
