using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour {
  public LayerMask collisionMask;

  public const float skinWidth = .015f;
  [SerializeField]protected int horizontalRayCount;
  [SerializeField]protected int verticalRayCount;

  protected float horizontalRaySpacing;
  protected float verticalRaySpacing;

  protected BoxCollider2D myCollider;
  protected RaycastOrigins raycastOrigins;

  public virtual void Awake() {
    myCollider = GetComponent<BoxCollider2D>();
  }

  public virtual void Start() {
    CalculateRaySpacing();
  }

  public void UpdateRaycastOrigins() {
    CalculateRaySpacing();
    Bounds bounds = myCollider.bounds;
    bounds.Expand(skinWidth * transform.localScale.x * -2);

    raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
    raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
    raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
    raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
  }

  public void CalculateRaySpacing() {
    Bounds bounds = myCollider.bounds;
    bounds.Expand(skinWidth * transform.localScale.x * -2);

    float boundsWidth = bounds.size.x;
    float boundsHeight = bounds.size.y;

    horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
    verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
  }

  public struct RaycastOrigins {
    public Vector2 topLeft, topRight;
    public Vector2 bottomLeft, bottomRight;
  }
}