using Code.Scripts.SOs.Animator;
using UnityEngine;

namespace Code.Scripts.Door
{
    public class DoorAnimatorController : MonoBehaviour
    {
        [SerializeField] private bool openDoor;
        [SerializeField] private bool closeDoor;
                
        [SerializeField] private Animator animator;
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
        
        public void OpenDoor()
        {
            animatorStateSetter.AnimatorSetValue(OpenState);
        }
        
        public void CloseDoor()
        {
            animatorStateSetter.AnimatorSetValue(CloseState);
        }
    }
}
