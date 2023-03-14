using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBlade : MonoBehaviour{


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.TryGetComponent(out Player player)) {
            player.PlayerDeath();
        }
    }

}
