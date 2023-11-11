using Code.Scripts.Attack;

namespace Patterns.FSM
{
    public class PunchState<T> : BaseState<T>
    {
        private readonly FirePunch leftPunch;
        private readonly FirePunch rightPunch;
        private float distance;
        
        public bool ended = false;
        
        public PunchState(T id, FirePunch leftPunch, FirePunch rightPunch) : base(id)
        {
            this.leftPunch = leftPunch;
            this.rightPunch = rightPunch;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            Punch();
        }
        
        private void Punch()
        {
            ended = false;
            
            if (distance > 0)
                leftPunch.Punch(distance);
            else
                rightPunch.Punch(distance);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (!leftPunch.Move && !rightPunch.Move)
                ended = true;
        }
    }
}
