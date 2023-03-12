using System;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour {

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameInputManager gameInputManager;

    private Rigidbody2D rb2D;

    private bool isRunning;
    private bool isOnGround = true;
    private bool isClimbing;

    private void Start() {
        gameInputManager.OnClimbAction += GameInput_OnClimbAction;
        gameInputManager.OnJumpAction += GameInput_OnJumpAction;
        gameInputManager.OnRespawnAction += GameInput_OnRespawnAction;

        rb2D = GetComponent<Rigidbody2D>();
    }

    private void GameInput_OnRespawnAction(object sender, EventArgs e) {
        transform.position = spawnPoint.transform.position;
    }

    private void Update() {
        HandleMovement();

    }

    private void GameInput_OnClimbAction(object sender, EventArgs e) {
        //Climb Movement implementation
        float ladderInteractDistance = 0.1f;
        float playerWidthXOffset = 0.25f;
        float raycastStartXPosition = transform.position.x + playerWidthXOffset;

        RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(raycastStartXPosition, transform.position.y), Vector2.right, ladderInteractDistance);
        Debug.DrawRay(new Vector3(raycastStartXPosition, transform.position.y, 0f), Vector2.right, Color.green);

        if(hitInfo) {
            //We have hit something
            if (hitInfo.transform.TryGetComponent(out Interactable interactable)) {
                //We have hit an interactable object. 
                TileBase tile = GetTileAtPosition(hitInfo, hitInfo.transform.GetComponent<Tilemap>());

                if (GetTileName(tile) == "LadderTile") {
                    // We have hit a ladder tile
                }
            }
            
        } else {
            //We havent hit anything
        }

    }

    private TileBase GetTileAtPosition(RaycastHit2D raycastHitInfo, Tilemap tilemap) {
        Vector3Int gridPosition = tilemap.WorldToCell(raycastHitInfo.point);

        TileBase tile = tilemap.GetTile(gridPosition);
        if (tile != null) {
            //We hit a valid tile.
            return tile;
        }
        return null;
    }

    private string GetTileName(TileBase tile) {
        return tile.name;
    }



    private void GameInput_OnJumpAction(object sender, EventArgs e) {
        //Jump Implementation.
        float jumpForce = 10f;
        float onGroundDistance = 0.1f;
        float playerHeightOffset = 0.49f; // changed from box collider 2d to capsule collider 2d. Had to manually check every value in inspector. IDK why it messed up. 
        float raycastStartYPosition = transform.position.y - playerHeightOffset;

        RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(transform.position.x, raycastStartYPosition), Vector2.down, onGroundDistance);
        
        if (hitInfo) {
            //We have hit something
            if (hitInfo.transform.TryGetComponent(out StaticPlatform staticPlatform)) {

                Jump(jumpForce);

            }
        } else {
            // Not hit anything.
        }
    }
    
    private void HandleMovement() {
        Vector2 inputVector = gameInputManager.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0f);
        transform.position += moveDir * (moveSpeed * Time.deltaTime);

        isRunning = moveDir != Vector3.zero && IsOnGround();
    }

    private void Jump(float jumpForce) {
        isOnGround = false;
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.TryGetComponent(out StaticPlatform staticPlatform)) {
            isOnGround = true;
        }
    }

    public bool IsRunning() {
        return isRunning; 
    }

    public bool IsClimbing() {
        return isClimbing;
    }

    public bool IsOnGround() {
        return isOnGround; 
    }
    
}

