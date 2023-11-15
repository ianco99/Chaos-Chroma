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
        
        private string closeState = "Close";
        private string openState = "Open";
        
        AnimatorStateSetter<string, int> animatorStateSetter;
        
        private void Awake()
        {
            animatorStateSetter = new AnimatorStateSetter<string, int>(parameterName, animator);
            
            animatorStateSetter.AddState(closeState, 0);
            animatorStateSetter.AddState(openState, 1);
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
            animatorStateSetter.AnimatorSetValue(openState);
        }
        
        public void CloseDoor()
        {
            animatorStateSetter.AnimatorSetValue(closeState);
        }
    }
}
