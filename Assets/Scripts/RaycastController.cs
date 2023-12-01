using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class RaycastController : MonoBehaviour {
  [FormerlySerializedAs("verticalRayCount")] [SerializeField] private int rayCount = 8;
  [SerializeField] private LayerMask collisionMask;
  [SerializeField] private float skinWidth = .015f;

  private BoxCollider2D _collider;
  private Rigidbody2D _rb;
  private RaycastOrigins _raycastOrigins;
  public Collisions collisions;
  private float _raySpacing;

  public virtual void Awake() {
    _collider = GetComponent<BoxCollider2D>();
    _rb = GetComponent<Rigidbody2D>();
    collisions.Reset();
  }

  private void Update() {
    GetCollisions();
  }

  void GetCollisions() {
    CalculateRaySpacing();
    collisions.Reset();
    float rayLength = skinWidth * 2;
    Vector2 verticalRayOffset = new Vector2(0, skinWidth);
    float[] spacing = new float[rayCount];
    for (int i = 0; i < rayCount; i++) {
      spacing[i] = _raySpacing * i;
    }

    // check below
    for (int i = 0; i < rayCount; i++) {
      Vector2 rayOriginBottom = _raycastOrigins.BottomLeft;
      rayOriginBottom += Vector2.right * spacing[i];
      RaycastHit2D hitBelow = Physics2D.Raycast(rayOriginBottom, Vector2.down, rayLength, collisionMask);
      // Debug.DrawRay(rayOriginBottom, Vector2.down * rayLength, Color.red);
      if (hitBelow) {
        collisions.Bottom = true;
        break;
      }
    }

    // check above
    for (int i = 0; i < rayCount; i++) {
      Vector2 rayOriginTop = _raycastOrigins.TopRight;
      rayOriginTop += Vector2.left * spacing[i];
      RaycastHit2D hitAbove = Physics2D.Raycast(rayOriginTop, Vector2.up, rayLength, collisionMask);
      // Debug.DrawRay(rayOriginTop, Vector2.up * rayLength, Color.red);
      if (hitAbove) {
        collisions.Top = true;
        break;
      }      
    }
    
    // check left
    for (int i = 0; i < rayCount; i++) {
      Vector2 rayOriginLeft = _raycastOrigins.BottomLeft + verticalRayOffset;
      rayOriginLeft += Vector2.up * spacing[i];
      RaycastHit2D hitLeft = Physics2D.Raycast(rayOriginLeft, Vector2.left, rayLength, collisionMask);
      // Debug.DrawRay(rayOriginLeft, Vector2.left * rayLength, Color.red);
      if (hitLeft) {
        collisions.Left = true;
        break;
      }      
    }
    
    // check right
    for (int i = 0; i < rayCount; i++) {
      Vector2 rayOriginRight = _raycastOrigins.TopRight + verticalRayOffset;
      rayOriginRight += Vector2.down * spacing[i];
      RaycastHit2D hitRight = Physics2D.Raycast(rayOriginRight, Vector2.right, rayLength, collisionMask);
      // Debug.DrawRay(rayOriginRight, Vector2.right * rayLength, Color.red);
      if (hitRight) {
        collisions.Right = true;
        break;
      }      
    }
    
    collisions.pinchedVertically = collisions.Bottom && collisions.Top;
    collisions.pinchedHorizontally = collisions.Left && collisions.Right;
    collisions.pinched = collisions.pinchedVertically || collisions.pinchedHorizontally;
  }
  
  public void CalculateRaySpacing() {
    Bounds bounds = _collider.bounds;
    bounds.Expand(skinWidth * -2);
    _raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y) + _rb.velocity * Time.deltaTime;
    _raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y)+ _rb.velocity * Time.deltaTime;
    _raySpacing = bounds.size.x / (rayCount - 1);
  }

  public struct RaycastOrigins {
    public Vector2 BottomLeft, TopRight;
  }
  
  public struct Collisions {
    public bool Top, Right, Bottom, Left, pinchedHorizontally, pinchedVertically, pinched;

    public void Reset() {
      Top = false;
      Right = false;
      Bottom = false;
      Left = false;
    }
  }
}