using UnityEngine;

namespace States
{
    public abstract class PlayerMovementBaseState
    {
        protected PlayerMovement _ctx;
        protected PlayerMovementStateFactory _factory;
        
        private bool _isRootState;
        protected PlayerMovementBaseState _currSuperState;
        protected PlayerMovementBaseState _currSubState;

        public PlayerMovementBaseState(PlayerMovement currCtx, PlayerMovementStateFactory factory, bool isRootState = false)
        {
            _ctx = currCtx;
            _factory = factory;
            _isRootState = isRootState;
        }

        public virtual void Enter() {}

        public virtual void Update() {}
        
        public virtual void FixedUpdate() {}

        public virtual void Exit() {}

        public PlayerMovementBaseState getActiveState()
        {
            var state = _currSubState ?? this;
            while (state._currSubState != null)
                state = state._currSubState;

            return state;
        }

        protected void SwitchState(PlayerMovementBaseState newState)
        {
            if (newState.GetType() == _ctx.CurrentMovementState.getActiveState().GetType())
                return;

            Exit();

            if (newState._isRootState)
            {
                _ctx.CurrentMovementState = newState;
            }
            else
            {
                _currSubState = newState;
                newState._currSuperState = this;
            }
            
            newState.Enter();
        }
    }

    public class PlayerGroundedState : PlayerMovementBaseState
    {
        public PlayerGroundedState(PlayerMovement currCtx, PlayerMovementStateFactory factory, bool isRootState = false) 
            : base(currCtx, factory, isRootState) {}

        public override void Enter()
        {
            _currSubState?.Enter();;
        }

        public override void Update()
        {
            if (_ctx.InputDir != Vector3.zero)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    SwitchState(_factory.Running());
                }
                else
                {
                    SwitchState(_factory.Moving());
                }
            }
            else
            {
                SwitchState(_factory.Idle());
            }

            _currSubState?.Update();
        }

        public override void FixedUpdate()
        {
            _currSubState?.FixedUpdate();
        }

        public override void Exit()
        {
            _currSubState?.Exit();;
        }
    }
    
    public class PlayerIdleState : PlayerMovementBaseState
    {
        public PlayerIdleState(PlayerMovement currCtx, PlayerMovementStateFactory factory, bool isRootState = false) 
            : base(currCtx, factory, isRootState) {}
    
        public override void Enter()
        {
            Debug.Log("Enter Idle");
        }

        public override void Exit()
        {
            Debug.Log("Exit Idle");
        }
    }

    public class PlayerMovingState : PlayerMovementBaseState
    {
        public PlayerMovingState(PlayerMovement currCtx, PlayerMovementStateFactory factory, bool isRootState = false) 
            : base(currCtx, factory, isRootState) {}

        public override void Enter()
        {
            Debug.Log("Enter Moving");
        }

        public override void FixedUpdate()
        {
            _ctx.Rigidbody.MovePosition(_ctx.transform.position + (_ctx.InputDir * _ctx.Speed) * Time.deltaTime);
        }
        
        public override void Exit()
        {
            Debug.Log("Exit Moving");
        }
    }
    
    public class PlayerRunningState : PlayerMovementBaseState
    {
        private float runSpeed = 10;
        public PlayerRunningState(PlayerMovement currCtx, PlayerMovementStateFactory factory, bool isRootState = false) 
            : base(currCtx, factory, isRootState) {}

        public override void Enter()
        {
            Debug.Log("Enter Running");
        }

        public override void FixedUpdate()
        {
            _ctx.Rigidbody.MovePosition(_ctx.transform.position + (_ctx.InputDir * _ctx.RunSpeed) * Time.deltaTime);
        }
        
        public override void Exit()
        {
            Debug.Log("Exit Running");
        }
    }
}
