using Code.Scripts.Abstracts.Character;
using Code.SOs.Enemy;
namespace Code.Scripts.Enemy
{
    /// <summary>
    /// Base class for all enemies
    /// </summary>
    public abstract class BaseEnemyController : Character
    {
        public BaseEnemySettings settings;
    }
}