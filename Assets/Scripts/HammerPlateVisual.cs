using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class HammerPlateVisual : MonoBehaviour {

    public event EventHandler OnLowestPoint;

    private float distanceToTravel = 1.5f;
    
    private float yBoundsDown;
    private float yBoundsUp;

    private bool isTravellingUp;
    
    private bool isAtLowestPoint;
    private bool isAtHighestPoint; 
    
    [SerializeField] private float moveSpeed = 10f;

    private void Start() {
        isAtHighestPoint = true;
        yBoundsDown = transform.position.y - distanceToTravel;
    }

    private void Update() {
        if (IsAtHighestPoint()) {
            TravelDown();
        }

        if (IsTravellingUp()) {
            TravelUp();
        }
    }

    private void TravelDown() {
        if (transform.position.y <= FloatToOneDecimalPlace(yBoundsDown)) {
            // Were at the bottom. 
            isAtHighestPoint = false;
            isAtLowestPoint = true;
            if (OnLowestPoint != null) {
                OnLowestPoint.Invoke(this, EventArgs.Empty);
            }

            StartCoroutine(DelayToWaitBeforeTravellingUp(0.375f));

            //Play the animation - wait for animation to finish, then call TravelUp(); 
        } else {
            transform.position += Vector3.down * (moveSpeed * Time.deltaTime);
        }
    }

    private void TravelUp() {
        float yBoundsUp = yBoundsDown + distanceToTravel;
        if (transform.position.y >= FloatToOneDecimalPlace(yBoundsUp)) {
            //Were at the top.
            isAtLowestPoint = false;
            isAtHighestPoint = true;
            isTravellingUp = false;
        } else {
            float upIncrement = 1f * moveSpeed;
            transform.position += new Vector3(0f, upIncrement * Time.deltaTime, 0f);
        }
    }

    private float FloatToOneDecimalPlace(float number) {
        float oneDecimalPlace = (number * 10.0f) / 10.0f;
        return oneDecimalPlace; 
    }

    private IEnumerator DelayToWaitBeforeTravellingUp(float delay) {
        yield return new WaitForSeconds(delay);
        isTravellingUp = true;
    }

    private bool IsTravellingUp() {
        return isTravellingUp;
    }

    private bool IsAtLowestPoint() {
        return isAtLowestPoint;
    }

    private bool IsAtHighestPoint() {
        return isAtHighestPoint; 
    }
}
