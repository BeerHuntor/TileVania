using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour {
    private PlayerInputActions playerInputActions;
    
    public EventHandler OnJumpAction;
    public EventHandler OnClimbAction;
    public EventHandler OnRespawnAction;

    private void Awake() {
        playerInputActions = new PlayerInputActions(); 
        playerInputActions.Player.Enable();

        playerInputActions.Player.Jump.performed += JumpAction_Performed;
        playerInputActions.Player.Climb.performed += ClimbAction_Performed;
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

    private void ClimbAction_Performed(InputAction.CallbackContext obj) {
        if (OnClimbAction != null) {
            OnClimbAction.Invoke(this, EventArgs.Empty);
        }
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
