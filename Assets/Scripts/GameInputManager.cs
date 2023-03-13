using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour {
    private PlayerInputActions playerInputActions;

    public EventHandler OnJumpAction;
    public EventHandler OnInteractAction;
    public EventHandler OnRespawnAction;

    private void Awake() {
        playerInputActions = new PlayerInputActions(); 
        playerInputActions.Player.Enable();

        playerInputActions.Player.Jump.performed += JumpAction_Performed;
        playerInputActions.Player.Interact.performed += InteractAction_Performed;
        playerInputActions.Player.Respawn.performed += RespawnAction_Performed;
    }

    private void RespawnAction_Performed(InputAction.CallbackContext obj) {
        if (OnRespawnAction != null ) { OnRespawnAction.Invoke(this, EventArgs.Empty);}
        
    }
    private void JumpAction_Performed(InputAction.CallbackContext obj) {
        if (OnJumpAction != null) {
            OnJumpAction.Invoke(this, EventArgs.Empty);
        }
    }

    private void InteractAction_Performed(InputAction.CallbackContext obj) {
        if (OnInteractAction != null) {
            OnInteractAction.Invoke(this, EventArgs.Empty);
        }
    }

    public Vector2 GetHorizontalMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public Vector2 GetClimbingMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Climb.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        return inputVector; 
    }
}
