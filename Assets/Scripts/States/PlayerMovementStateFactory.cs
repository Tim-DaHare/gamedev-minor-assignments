namespace States
{
    public class PlayerMovementStateFactory
    {
        private PlayerMovement _currCtx;
        
        public PlayerMovementStateFactory(PlayerMovement currCtx)
        {
            _currCtx = currCtx;
        }

        public PlayerMovementBaseState Idle()
        {
            return new PlayerIdleState(_currCtx, this);
        }

        public PlayerMovingState Moving()
        {
            return new PlayerMovingState(_currCtx, this);
        }
    }
}
