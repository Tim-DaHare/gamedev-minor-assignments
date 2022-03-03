using UnityEngine;
using States;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// The speed at which the player can move around
    /// </summary>
    public float Speed = 5f;
    
    /// <summary>
    /// The speed at which the player can run or "Sprint" around
    /// </summary>
    public float RunSpeed = 10f;
    
    /// <summary>
    /// The current movement state of the player
    /// </summary>
    private PlayerMovementBaseState _currentMovementState;
    public PlayerMovementBaseState CurrentMovementState
    {
        get => _currentMovementState;
        set => _currentMovementState = value;
    }

    /// <summary>
    /// The instance of the player movement state factories
    /// </summary>
    private PlayerMovementStateFactory _movementStates;

    [SerializeField] private Transform cam; 

    public Vector3 InputDir { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    // public Vector3 test;
    
    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();

        _movementStates = new PlayerMovementStateFactory(this);
        
        _currentMovementState = _movementStates.Grounded();
        _currentMovementState.Enter();
    }

    private void Update()
    {
        var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        InputDir = Vector3.ProjectOnPlane(cam.TransformDirection(input), Vector3.up);

        transform.forward = InputDir;

        // test = InputDir;

        // update state
        _currentMovementState.Update();
    }

    private void FixedUpdate()
    {
        _currentMovementState.FixedUpdate();
    }
}


