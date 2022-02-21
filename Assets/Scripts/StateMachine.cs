using UnityEngine;

public abstract class StateMachineState
{
    public virtual void Enter()
    {
    }
    
    public virtual StateMachineState Update()
    {
        return null;
    }
    
    public virtual void Exit()
    {
    }
}

public abstract class StateMachine
{
    public StateMachineState CurrentState { get; private set; }
    
    public virtual void Update(object subject)
    {
        // update current state
        var newState = CurrentState.Update();
        if (newState == null)
            return;
        
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
