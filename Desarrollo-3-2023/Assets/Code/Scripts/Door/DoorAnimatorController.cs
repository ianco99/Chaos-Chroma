using Code.Scripts.Animator;
using UnityEngine;

namespace Code.Scripts.Door
{
    /// <summary>
    /// Controls the animation of the door
    /// </summary>
    public class DoorAnimatorController : MonoBehaviour
    {
        [SerializeField] private bool openDoor;
        [SerializeField] private bool closeDoor;
                
        [SerializeField] private UnityEngine.Animator animator;
        [SerializeField] private string parameterName;

        private const string CloseState = "Close";
        private const string OpenState = "Open";

        AnimatorStateSetter<string, int> animatorStateSetter;
        
        private void Awake()
        {
            animatorStateSetter = new AnimatorStateSetter<string, int>(parameterName, animator);
            
            animatorStateSetter.AddState(CloseState, 0);
            animatorStateSetter.AddState(OpenState, 1);
        }
        
        private void Update()
        {
            if (openDoor)
            {
                OpenDoor();
                openDoor = false;
            }
            
            if (closeDoor)
            {
                CloseDoor();
                closeDoor = false;
            }
        }
        
        /// <summary>
        /// Opens the door by setting the animator to the open state
        /// </summary>
        public void OpenDoor()
        {
            animatorStateSetter.AnimatorSetValue(OpenState);
        }
        
        /// <summary>
        /// Closes the door by setting the animator to the close state.
        /// </summary>
        public void CloseDoor()
        {
            animatorStateSetter.AnimatorSetValue(CloseState);
        }
    }
}
