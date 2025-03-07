using Code.Scripts.Abstracts.Character;

namespace Code.Scripts.Pickup
{
    /// <summary>
    /// Interface for pickups
    /// </summary>
    public interface IPickup 
    {
        public void Pickup(Character character);
    }
}
