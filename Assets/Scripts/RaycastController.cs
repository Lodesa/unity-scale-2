using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour {
  [SerializeField] private int verticalRayCount = 8;
  [SerializeField] private LayerMask collisionMask;
  [SerializeField] private float skinWidth = .015f;

  private float verticalRaySpacing;
  private BoxCollider2D collider;
  public Vector2 raycastOrigin;
  public Collisions collisions;

  public virtual void Awake() {
    collider = GetComponent<BoxCollider2D>();
    collisions.Reset();
  }

  private void FixedUpdate() {
    VerticalCollisions();
  }

  void VerticalCollisions() {
    CalculateRaySpacing();
    float rayLength = skinWidth * 2;
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

    collisions.Below = grounded;
  }

  public void CalculateRaySpacing() {
    Bounds bounds = collider.bounds;
    bounds.Expand(skinWidth * -2);
    raycastOrigin = new Vector2(bounds.min.x, bounds.min.y);
    verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
  }

  public struct Collisions {
    public bool Above, Below, Right, Left;

    public void Reset() {
      Above = false;
      Below = false;
      Right = false;
      Left = false;
    }
  }
}