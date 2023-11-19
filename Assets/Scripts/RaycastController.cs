using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour {
  [FormerlySerializedAs("verticalRayCount")] [SerializeField] private int rayCount = 8;
  [SerializeField] private LayerMask collisionMask;
  [SerializeField] private float skinWidth = .015f;

  private float _raySpacing;
  private BoxCollider2D _collider;
  private RaycastOrigins _raycastOrigins;
  
  public Collisions collisions;

  public virtual void Awake() {
    _collider = GetComponent<BoxCollider2D>();
    collisions.Reset();
  }

  private void FixedUpdate() {
    GetCollisions();
  }

  void GetCollisions() {
    CalculateRaySpacing();
    collisions.Reset();
    float rayLength = skinWidth * 2;
    

    // check below
    for (int i = 0; i < rayCount; i++) {
      Vector2 rayOriginBottom = _raycastOrigins.BottomLeft;
      rayOriginBottom += Vector2.right * (_raySpacing * i);
      RaycastHit2D hitBelow = Physics2D.Raycast(rayOriginBottom, Vector2.down, rayLength, collisionMask);
      Debug.DrawRay(rayOriginBottom, Vector2.down * rayLength, Color.red);
      if (hitBelow) {
        collisions.Bottom = true;
        break;
      }
    }

    // check above
    for (int i = 0; i < rayCount; i++) {
      Vector2 rayOriginTop = _raycastOrigins.TopRight;
      rayOriginTop += Vector2.left * (_raySpacing * i);
      RaycastHit2D hitAbove = Physics2D.Raycast(rayOriginTop, Vector2.up, rayLength, collisionMask);
      Debug.DrawRay(rayOriginTop, Vector2.up * rayLength, Color.red);
      if (hitAbove) {
        collisions.Top = true;
        break;
      }      
    }
    
    // check left
    for (int i = 0; i < rayCount; i++) {
      Vector2 rayOriginLeft = _raycastOrigins.BottomLeft;
      rayOriginLeft += Vector2.up * (_raySpacing * i);
      RaycastHit2D hitLeft = Physics2D.Raycast(rayOriginLeft, Vector2.left, rayLength, collisionMask);
      Debug.DrawRay(rayOriginLeft, Vector2.left * rayLength, Color.red);
      if (hitLeft) {
        collisions.Left = true;
        break;
      }      
    }
    
    // check right
    for (int i = 0; i < rayCount; i++) {
      Vector2 rayOriginRight = _raycastOrigins.TopRight;
      rayOriginRight += Vector2.down * (_raySpacing * i);
      RaycastHit2D hitRight = Physics2D.Raycast(rayOriginRight, Vector2.right, rayLength, collisionMask);
      Debug.DrawRay(rayOriginRight, Vector2.right * rayLength, Color.red);
      if (hitRight) {
        collisions.Right = true;
        break;
      }      
    }
    
    print((collisions.Top?"1":"0") + (collisions.Right?"1":"0") + (collisions.Bottom?"1":"0") + (collisions.Left?"1":"0"));
  }
  
  public void CalculateRaySpacing() {
    Bounds bounds = _collider.bounds;
    bounds.Expand(skinWidth * -2);
    _raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
    _raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    _raySpacing = bounds.size.x / (rayCount - 1);
  }

  public struct RaycastOrigins {
    public Vector2 BottomLeft, TopRight;
  }
  
  public struct Collisions {
    public bool Top, Right, Bottom, Left;

    public void Reset() {
      Top = false;
      Right = false;
      Bottom = false;
      Left = false;
    }
  }
}