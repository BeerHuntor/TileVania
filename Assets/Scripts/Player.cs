using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Player : MonoBehaviour {
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameInputManager gameInputManager;

    private Rigidbody2D rigidbody2D;

    private bool isRunning;
    private bool isJumping;

    private void Start() {
        gameInputManager.OnClimbAction += GameInput_OnClimbAction;
        gameInputManager.OnJumpAction += GameInput_OnJumpAction;

        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    private void Update() {
        HandleMovement();

    }

    private void GameInput_OnClimbAction(object sender, EventArgs e) {
        //Climb Movement implementation
        Debug.Log("Climbing!");
    }

    private void GameInput_OnJumpAction(object sender, EventArgs e) {
        //Jump Implementation.
        float jumpForce = 10f;
        float onGroundDistance = 0.1f;
        float playerHeightOffset = 0.5f; 

        RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - playerHeightOffset), Vector2.down, onGroundDistance);
        if (hitInfo) {
            if (hitInfo.transform.TryGetComponent(out StaticPlatform staticPlatform)) {
                rigidbody2D.AddForce(new Vector2(rigidbody2D.velocity.x, jumpForce), ForceMode2D.Impulse);
                isJumping = true; //TODO: Figure out the wording for seeing if the player is jumping or not. 
            }
        } else {
            // Not hit anything. 
            isJumping = false;
        }
    }
    
    private void HandleMovement() {
        Vector2 inputVector = gameInputManager.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0f);
        transform.position += moveDir * (moveSpeed * Time.deltaTime);

        isRunning = moveDir != Vector3.zero;
    }

    public bool IsRunning() {
        return isRunning; 
    }

    public bool IsJumping() {
        return isJumping; 
    }
    
}

