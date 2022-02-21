using System;
using UnityEngine;


public class PlayerIdleState : StateMachineState
{
    public override StateMachineState Update()
    {
        if (InputDir != Vector3.zero)
        {
            return new PlayerWalkingState();
        }

        return null;
    }
}

public class PlayerWalkingState : StateMachineState
{
    public override StateMachineState Update()
    {
        if (InputDir == Vector3.zero)
        {
            return new PlayerIdleState();
        }
        
        return null;
    }
}

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// The current state of the player
    /// </summary>
    private StateMachineState CurrentMovementState { get; set; } = new PlayerIdleState();
    
    /// <summary>
    /// The speed at which the player can move around
    /// </summary>
    public float Speed = 5f;

    public string test;

    private void Update()
    {
        // update statemachine
        CurrentMovementState.Update();

        test = CurrentMovementState.GetType().ToString();
    }
}
