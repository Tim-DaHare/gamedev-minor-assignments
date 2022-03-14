using UnityEngine;
using UnityEngine.PlayerLoop;

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

        public PlayerMovementBaseState GetActiveState()
        {
            var state = _currSubState ?? this;
            while (state._currSubState != null)
                state = state._currSubState;

            return state;
        }

        protected void SwitchState(PlayerMovementBaseState newState)
        {
            if (newState.GetType() == _ctx.CurrentMovementState.GetActiveState().GetType())
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
            _currSubState?.Enter();
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
            _currSubState?.Exit();
        }
    }
    
    public class PlayerIdleState : PlayerMovementBaseState
    {
        public PlayerIdleState(PlayerMovement currCtx, PlayerMovementStateFactory factory, bool isRootState = false) 
            : base(currCtx, factory, isRootState) {}
    
        public override void Enter()
        {
            // Debug.Log("Enter Idle");
        }

        public override void Exit()
        {
            // Debug.Log("Exit Idle");
        }
    }

    public class PlayerMovingState : PlayerMovementBaseState
    {
        public PlayerMovingState(PlayerMovement currCtx, PlayerMovementStateFactory factory, bool isRootState = false) 
            : base(currCtx, factory, isRootState) {}

        public override void Enter()
        {
            // Debug.Log("Enter Moving");
        }

        public override void Update()
        {
            _ctx.transform.forward = _ctx.InputDir;
        }

        public override void FixedUpdate()
        {
            _ctx.Rigidbody.MovePosition(_ctx.transform.position + (_ctx.InputDir * _ctx.Speed) * Time.fixedDeltaTime);
            
        }
        
        public override void Exit()
        {
            // Debug.Log("Exit Moving");
        }
    }
    
    public class PlayerRunningState : PlayerMovementBaseState
    {
        public PlayerRunningState(PlayerMovement currCtx, PlayerMovementStateFactory factory, bool isRootState = false) 
            : base(currCtx, factory, isRootState) {}

        public override void Enter()
        {
            // Debug.Log("Enter Running");
        }

        public override void Update()
        {
            _ctx.transform.forward = _ctx.InputDir;
            if (Input.GetKeyDown(KeyCode.C))
            {
                SwitchState(_factory.Sliding());
            }
        }

        public override void FixedUpdate()
        {
            _ctx.Rigidbody.MovePosition(_ctx.transform.position + (_ctx.InputDir * _ctx.RunSpeed) * Time.fixedDeltaTime);
        }
        
        public override void Exit()
        {
            // Debug.Log("Exit Running");
        }
    }
    
    public class PlayerSlidingState : PlayerMovementBaseState
    {
        private Vector3 _slideVel;
        
        public PlayerSlidingState(PlayerMovement currCtx, PlayerMovementStateFactory factory, bool isRootState = false) 
            : base(currCtx, factory, isRootState) {}

        public override void Enter()
        {
            Debug.Log("Enter Slide");
            _ctx.transform.localScale = new Vector3(1, 0.5F, 1);
            
            _slideVel = _ctx.InputDir * _ctx.RunSpeed;
        }

        public override void Update()
        {
            _ctx.transform.forward = _ctx.InputDir;
        }

        public override void FixedUpdate()
        {
            if (_slideVel.magnitude <= _ctx.Speed)
                SwitchState(_factory.Grounded());
            
            _ctx.Rigidbody.MovePosition(_ctx.transform.position + (_slideVel * Time.fixedDeltaTime));
            
            _slideVel = Vector3.ClampMagnitude(
                (_slideVel + _ctx.InputDir).normalized * (_slideVel.magnitude - 5 * Time.fixedDeltaTime), 
                _ctx.RunSpeed
            );
        }
        
        public override void Exit()
        {
            _ctx.transform.localScale = new Vector3(1, 1, 1);

            Debug.Log("Exit Slide");
        }
    }
}
