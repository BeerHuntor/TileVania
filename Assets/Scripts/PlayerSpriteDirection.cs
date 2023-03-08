using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteDirection : MonoBehaviour {

    [SerializeField] private GameInputManager gameInputManager;

    private float lastFacingDirX = 1f; //Default facing to the right. 

    private void Update() {
        HandleSpriteRotationDirection();
    }

    private void HandleSpriteRotationDirection() {
        if (gameInputManager.GetMovementVectorNormalized() != Vector2.zero) {
            lastFacingDirX = gameInputManager.GetMovementVectorNormalized().x;
        }
        transform.localScale = new Vector2(Mathf.Sign(lastFacingDirX), 1f);
    }

}
