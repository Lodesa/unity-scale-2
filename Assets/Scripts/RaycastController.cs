using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour {
  [SerializeField] private int verticalRayCount = 8;
  [SerializeField] private LayerMask collisionMask;

  private const float skinWidth = .015f;
  private float verticalRaySpacing;
  private BoxCollider2D collider;
  public Vector2 raycastOrigin;
  public bool isGrounded;

  public virtual void Awake() {
    collider = GetComponent<BoxCollider2D>();
    isGrounded = false;
  }

  private void FixedUpdate() {
    SetGrounded();
  }

  void SetGrounded() {
    CalculateRaySpacing();
    float rayLength = skinWidth * 2;
    // float rayLength = skinWidth * 2;
    bool grounded = false;
    for (int i = 0; i < verticalRayCount; i++) {
      Vector2 rayOrigin = raycastOrigin;
      rayOrigin += Vector2.right * (verticalRaySpacing * i);
      RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, collisionMask);

      Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.red);

      if (hit) {
        print(hit.distance);
        grounded = true;
        break;
      }
    }

    isGrounded = grounded;
  }

  public void CalculateRaySpacing() {
    Bounds bounds = collider.bounds;
    bounds.Expand(skinWidth * -2);
    raycastOrigin = new Vector2(bounds.min.x, bounds.min.y);
    verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
  }
}