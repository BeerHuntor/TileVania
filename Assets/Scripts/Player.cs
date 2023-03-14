using System;
using System.Collections;
using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour {

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameInputManager gameInputManager;
    [SerializeField] private Transform raycastDownStartPosition; 

    private Rigidbody2D rb2D;

    private bool isRunning;
    private bool isOnGround = true;
    private bool isClimbing;

    private float playerWidthXOffset = 0.25f;
    private float defaultGravityScale; 

    private const string CLIMBING_LAYER_MASK = "Climbing";
    private LayerMask climbingLayerMask;

    private void Start() {
        gameInputManager.OnInteractAction += GameInput_OnInteractAction;
        gameInputManager.OnJumpAction += GameInput_OnJumpAction;
        gameInputManager.OnRespawnAction += GameInput_OnRespawnAction;

        climbingLayerMask = LayerMask.GetMask(CLIMBING_LAYER_MASK);

        rb2D = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb2D.gravityScale;
    }

    private void GameInput_OnRespawnAction(object sender, EventArgs e) {
        transform.position = spawnPoint.transform.position;
    }

    private void Update() {
        HandleMovement();
        HandleClimbing();

    }   
    #region EventImplementations
    private void GameInput_OnJumpAction(object sender, EventArgs e) {

        //Jump Implementation.
        float jumpForce = 20f;
        float onGroundDistance = 0.3f;
        float playerHeightOffset = 0.49f; // changed from box collider 2d to capsule collider 2d. Had to manually check every value in inspector. IDK why it messed up. 
        //float raycastStartYPosition = transform.position.y - playerHeightOffset;

        //RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(transform.position.x, raycastStartYPosition), Vector2.down, onGroundDistance);

        RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(raycastDownStartPosition.transform.position.x, raycastDownStartPosition.transform.position.y) , Vector2.down, onGroundDistance);
        Debug.Log(hitInfo.collider);
        Debug.DrawRay(raycastDownStartPosition.position, Vector3.down, Color.green);
        if (hitInfo) {
            //We have hit something
            if (hitInfo.transform.TryGetComponent(out StaticPlatform staticPlatform)) {
                Jump(jumpForce);
            }
        }
        else {
            // Not hit anything.
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        //Interact Movement implementation
        float interactDistance = 0.3f;
        float raycastStartXPosition = transform.position.x + playerWidthXOffset;

        RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(raycastStartXPosition, transform.position.y), Vector2.right, interactDistance);
        
        if(hitInfo) {
            //We have hit something
            Tilemap tilemap = hitInfo.transform.GetComponent<Tilemap>();
            
            if (hitInfo.transform.TryGetComponent(out Interactable interactable)) {
                //We have hit an interactable object. 
                TileBase tile = GetTileAtRaycastHitPoint(hitInfo, tilemap);
                
                /*if (GetTileName(tile) == "LadderTile") {
                    // We have hit a ladder tile
                    SetSelfPositionOfLadder(GetWorldPositionOfTileAtRaycastHitPoint(hitInfo,tilemap));
                    ClimbLadder(); 
                }*/
                
            }
            
        } else {
            //We havent hit anything
        }

    }
    #endregion

    #region TileMapMethods
    //Gets the Tile in the tilemap at raycast hitpoint. 
    private TileBase GetTileAtRaycastHitPoint(RaycastHit2D raycastHitInfo, Tilemap tilemap) {
        
        Vector3Int gridPosition = GetGridPositionOfTileAtRaycastHitPoint(raycastHitInfo, tilemap);
        
        TileBase tile = tilemap.GetTile(gridPosition);
        
        if (tile != null) {
            //We hit a valid tile.
            return tile;
        }
        return null;
    }

    private Vector3 GetWorldPositionOfTileAtRaycastHitPoint(RaycastHit2D raycastHitInfo, Tilemap tilemap) {
        Vector3Int gridPosition = GetGridPositionOfTileAtRaycastHitPoint(raycastHitInfo, tilemap);
        
        Vector3 worldPositionOfTile = tilemap.GetCellCenterWorld(gridPosition);

        return worldPositionOfTile;
    }

    private string GetTileName(TileBase tile) {
        return tile.name;
    }

    private Vector3Int GetGridPositionOfTileAtRaycastHitPoint(RaycastHit2D raycastHitInfo, Tilemap tilemap) {
        Vector3Int gridPostition = tilemap.WorldToCell(raycastHitInfo.point);
        return gridPostition;
    }
    #endregion

    #region Movement 
    private void HandleMovement() {
        Vector2 inputVector = gameInputManager.GetHorizontalMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0f);
        transform.position += moveDir * (moveSpeed * Time.deltaTime);

        isRunning = moveDir != Vector3.zero && IsOnGround();
    }

    private void Jump(float jumpForce) {
        isOnGround = false;
        rb2D.velocity = new Vector2(0, jumpForce);
    }
    private void HandleClimbing() {
        if (!rb2D.IsTouchingLayers(climbingLayerMask)) {
            isClimbing = false;
            SetPlayerGravityScale(defaultGravityScale);
            return;
        }

        Vector2 inputVector = gameInputManager.GetClimbingMovementVectorNormalized();
        float climbSpeed = 10f;

        Vector3 climbDir = new Vector3(inputVector.x, inputVector.y * climbSpeed, 0f);

        isClimbing = climbDir != Vector3.zero;

        SetPlayerGravityScale(0);
        transform.position += climbDir * Time.deltaTime;
    }

    #endregion


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.TryGetComponent(out StaticPlatform staticPlatform)) {
            isOnGround = true;
        }
    }

    private void SetPlayerGravityScale(float gravityValue) {
        rb2D.gravityScale = gravityValue;
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

