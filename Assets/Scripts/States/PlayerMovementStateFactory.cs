namespace States
{
    public class PlayerMovementStateFactory
    {
        private PlayerMovement _currCtx;
        
        public PlayerMovementStateFactory(PlayerMovement currCtx)
        {
            _currCtx = currCtx;
        }
        
        public PlayerMovementBaseState Grounded()
        {
            return new PlayerGroundedState(_currCtx, this, true);
        }

        public PlayerMovementBaseState Idle()
        {
            return new PlayerIdleState(_currCtx, this);
        }

        public PlayerMovingState Moving()
        {
            return new PlayerMovingState(_currCtx, this);
        }
        
        public PlayerRunningState Running()
        {
            return new PlayerRunningState(_currCtx, this);
        }
    }
}
