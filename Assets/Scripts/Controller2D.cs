using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Controller2D : RaycastController {
  public float maxSlopeAngle = 80;

  public CollisionInfo collisions;
  protected Vector2 playerInput;

  public override void Start() {
    base.Start();
    collisions.faceDir = 1;
  }

  public void Move(Vector2 moveAmount, bool standingOnPlatform = false, bool maintainFaceDir = false) {
    Move(moveAmount, Vector2.zero, standingOnPlatform, maintainFaceDir);
  }

  public void Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false, bool maintainFaceDir = false) {
    UpdateRaycastOrigins();

    collisions.Reset();
    collisions.moveAmountOld = moveAmount;
    playerInput = input;

    if (moveAmount.x != 0 && !maintainFaceDir) {
      collisions.faceDir = (int)Mathf.Sign(moveAmount.x);
    }

    HorizontalCollisions(ref moveAmount);
    if (moveAmount.y != 0) {
      VerticalCollisions(ref moveAmount);
    }

    transform.Translate(moveAmount);
  }

  void HorizontalCollisions(ref Vector2 moveAmount) {
    float directionX = collisions.faceDir;
    float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

    if (Mathf.Abs(moveAmount.x) < skinWidth) {
      rayLength = 2 * skinWidth;
    }

    for (int i = 0; i < horizontalRayCount; i++) {
      Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
      rayOrigin += Vector2.up * (horizontalRaySpacing * i);
      RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

      Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

      if (hit) {
        if (hit.distance == 0) {
          continue;
        }

        moveAmount.x = (hit.distance - skinWidth) * directionX;
        rayLength = hit.distance;

        collisions.left = directionX == -1;
        collisions.right = directionX == 1;
      }
    }
  }

  void VerticalCollisions(ref Vector2 moveAmount) {
    float directionY = Mathf.Sign(moveAmount.y);
    float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

    for (int i = 0; i < verticalRayCount; i++) {
      Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
      rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
      RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

      Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

      if (hit) {
        moveAmount.y = (hit.distance - skinWidth) * directionY;
        rayLength = hit.distance;

        collisions.below = directionY == -1;
        collisions.above = directionY == 1;
      }
    }
  }

  public void Scale(Vector3 factor, float time) {
    StartCoroutine(ScaleOverSeconds(factor, time));
  }

  private IEnumerator ScaleOverSeconds(Vector3 scaleTo, float seconds) {
    float elapsedTime = 0;
    Vector3 startingScale = transform.localScale;
    bool gettingLarger = scaleTo.x > startingScale.x;
    while (elapsedTime < seconds) {
      if (gettingLarger && ((collisions.below && collisions.above) || (collisions.left && collisions.right))) {
        transform.localScale = startingScale;
        yield break;
      }
      float scaleDifference = scaleTo.x - startingScale.x;
      Vector2 vel = Vector2.zero;
      if (gettingLarger && collisions.below) {
        vel += Vector2.up * Time.deltaTime * (scaleDifference);
      }
      Move(vel);
      
      transform.localScale = Vector3.Lerp(startingScale, scaleTo, (elapsedTime / seconds));
      elapsedTime += Time.deltaTime;
      yield return new WaitForEndOfFrame();
    }

    transform.localScale = scaleTo;
  }


  public struct CollisionInfo {
    public bool above, below;
    public bool left, right;

    public bool climbingSlope;
    public bool descendingSlope;
    public bool slidingDownMaxSlope;

    public float slopeAngle, slopeAngleOld;
    public Vector2 slopeNormal;
    public Vector2 moveAmountOld;
    public int faceDir;
    public bool fallingThroughPlatform;

    public void Reset() {
      above = below = false;
      left = right = false;
      climbingSlope = false;
      descendingSlope = false;
      slidingDownMaxSlope = false;
      slopeNormal = Vector2.zero;

      slopeAngleOld = slopeAngle;
      slopeAngle = 0;
    }
  }
}