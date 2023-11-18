using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour {
  [SerializeField] private int verticalRayCount = 8;
  [SerializeField] private LayerMask collisionMask;
  [SerializeField] private float skinWidth = .015f;

  private float _verticalRaySpacing;
  private float _horizontalRaySpacing;
  private BoxCollider2D _collider;
  private RaycastOrigins _raycastOrigins;
  
  public Collisions collisions;

  public virtual void Awake() {
    _collider = GetComponent<BoxCollider2D>();
    collisions.Reset();
  }

  private void FixedUpdate() {
    VerticalCollisions();
  }

  void VerticalCollisions() {
    CalculateRaySpacing();
    float rayLength = skinWidth * 2;
    bool below = false;
    bool above = false;

    for (int i = 0; i < verticalRayCount; i++) {
      // check below
      Vector2 rayOriginBottom = _raycastOrigins.BottomLeft;
      rayOriginBottom += Vector2.right * (_verticalRaySpacing * i);
      RaycastHit2D hitBelow = Physics2D.Raycast(rayOriginBottom, Vector2.down, rayLength, collisionMask);
      Debug.DrawRay(rayOriginBottom, Vector2.down * rayLength, Color.red);
      if (hitBelow) {
        below = true;
        break;
      }
    }
    collisions.Below = below;

    // check above
    for (int i = 0; i < verticalRayCount; i++) {
      Vector2 rayOriginTop = _raycastOrigins.TopRight;
      rayOriginTop += Vector2.left * (_verticalRaySpacing * i);
      RaycastHit2D hitAbove = Physics2D.Raycast(rayOriginTop, Vector2.up, rayLength, collisionMask);
      Debug.DrawRay(rayOriginTop, Vector2.up * rayLength, Color.red);
      if (hitAbove) {
        above = true;
        break;
      }      
    }
    collisions.Above = above;
    
    print("below: " + collisions.Below + ", above: " + collisions.Above);
  }

  public void CalculateRaySpacing() {
    Bounds bounds = _collider.bounds;
    bounds.Expand(skinWidth * -2);
    _raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
    _raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    _verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
  }

  public struct RaycastOrigins {
    public Vector2 BottomLeft, TopRight;
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