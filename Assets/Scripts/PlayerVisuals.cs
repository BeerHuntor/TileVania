using System;
using System.Collections;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour {

    [SerializeField] private GameInputManager gameInputManager;
    [SerializeField] private Transform playerDamageVisual;
    [SerializeField] private Player player;
    
    private float lastFacingDirX = 1f; //Default facing to the right. 

    private void Start() {
        player.OnPlayerDamage += Player_OnPlayerDamageEvent;
    }

    private void Player_OnPlayerDamageEvent(object sender, EventArgs e) {
        float timeToFlashVisual = 0.5f;
        StartCoroutine(FlashDamageVisualCoroutine(timeToFlashVisual));
    }

    private IEnumerator FlashDamageVisualCoroutine(float timeBeforeChangeBackToDefault) {
        ShowDamageVisual();
        yield return new WaitForSeconds(timeBeforeChangeBackToDefault);
        HideDamageVisual();
    }
    private void Update() {
        HandleSpriteRotationDirection();
    }

    private void HandleSpriteRotationDirection() {
        if (gameInputManager.GetHorizontalMovementVectorNormalized() != Vector2.zero) {
            lastFacingDirX = gameInputManager.GetHorizontalMovementVectorNormalized().x;
        }
        transform.localScale = new Vector2(Mathf.Sign(lastFacingDirX), 1f);
    }

    private void ShowDamageVisual() {
        playerDamageVisual.gameObject.SetActive(true);
    }

    private void HideDamageVisual() {
        playerDamageVisual.gameObject.SetActive(false);    
    }

}
