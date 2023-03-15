using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerPlateAnimator : MonoBehaviour {

    private const string SMOKE_ACTIVE = "SmokeActive";
    
    private Animator animator;
    
    
    [SerializeField] private HammerPlateVisual hammerPlateVisual; 

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        hammerPlateVisual.OnLowestPoint += HammerPlateVisual_OnLowestPointEvent;
    }

    private void HammerPlateVisual_OnLowestPointEvent(object sender, EventArgs e) {
        animator.SetTrigger(SMOKE_ACTIVE);
    }
}
