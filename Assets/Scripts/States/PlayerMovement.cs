using UnityEngine;

namespace States
{
    public abstract class PlayerMovementBaseState
    {
        protected PlayerMovement _ctx;
        protected PlayerMovementStateFactory _factory;

        public PlayerMovementBaseState(PlayerMovement currCtx, PlayerMovementStateFactory factory)
        {
            _ctx = currCtx;
            _factory = factory;
        }

        public virtual void Enter() {}

        public virtual void Update() {}
        
        public virtual void FixedUpdate() {}

        public virtual void Exit() {}

        protected void SwitchState(PlayerMovementBaseState newState)
        {
            Exit();
            _ctx.CurrentMovementState = newState;
            newState.Enter();
        }
    }
    
    public class PlayerIdleState : PlayerMovementBaseState
    {
        public PlayerIdleState(PlayerMovement currCtx, PlayerMovementStateFactory factory) : base(currCtx, factory) {}

        public override void Update()
        {
            if (_ctx.InputDir != Vector3.zero)
            {
                SwitchState(_factory.Moving());
            }
        }
    }

    public class PlayerMovingState : PlayerMovementBaseState
    {
        public PlayerMovingState(PlayerMovement currCtx, PlayerMovementStateFactory factory) : base(currCtx, factory) {}

        public override void FixedUpdate()
        {
            _ctx.Rigidbody.MovePosition(_ctx.transform.position + (_ctx.InputDir * _ctx.Speed) * Time.deltaTime);
            
            if (_ctx.InputDir == Vector3.zero)
            {
                SwitchState(_factory.Idle());
            }
        }
    }
}
