using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    private const string IS_RUNNING = "isRunning";
    private const string IS_CLIMBING = "isClimbing";
    private const string IS_JUMPING = "isOnGround";

    [SerializeField] Player player;

    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {

        animator.SetBool(IS_RUNNING, player.IsRunning());
        animator.SetBool(IS_CLIMBING, player.IsClimbing());
        animator.SetBool(IS_JUMPING, player.IsOnGround());

    }

}
