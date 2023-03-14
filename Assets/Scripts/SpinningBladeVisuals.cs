using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBladeVisuals : MonoBehaviour {

    [SerializeField] private Transform bladeTop;
    [SerializeField] private Transform SawBlade;
    [SerializeField] private Transform rotateAnchorPoint;

    private float bladeTopRotateSpeed = 300f;
    private float rotateAnchorPointSpeed = 100f;

    private void Update() {
        bladeTop.Rotate(0, 0, bladeTopRotateSpeed * Time.deltaTime);
        SawBlade.RotateAround(rotateAnchorPoint.position, Vector3.forward, rotateAnchorPointSpeed* Time.deltaTime);
    }
}
