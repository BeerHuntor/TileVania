using UnityEngine;

public class PlayerSpriteDirection : MonoBehaviour {

    [SerializeField] private GameInputManager gameInputManager;

    private float lastFacingDirX = 1f; //Default facing to the right. 

    private void Update() {
        HandleSpriteRotationDirection();
    }

    private void HandleSpriteRotationDirection() {
        if (gameInputManager.GetHorizontalMovementVectorNormalized() != Vector2.zero) {
            lastFacingDirX = gameInputManager.GetHorizontalMovementVectorNormalized().x;
        }
        transform.localScale = new Vector2(Mathf.Sign(lastFacingDirX), 1f);
    }

}
