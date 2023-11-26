using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(RaycastController))]
[RequireComponent(typeof(CameraController))]
public class Player : MonoBehaviour {

  [SerializeField] private bool growEnabled = false;
  [SerializeField] private bool dashEnabled = false;
  [SerializeField] private float speedSmall = 9;
  [SerializeField] private float speedLarge = 10;
  [SerializeField] private float maxJumpVelocitySmall = 26;
  [SerializeField] private float maxJumpVelocityLarge = 40;
  [SerializeField] private float minJumpVelocitySmall = 5;
  [SerializeField] private float minJumpVelocityLarge = 10;
  [SerializeField] private float accelerationTimeAirborneSmall = .1f;
  [SerializeField] private float accelerationTimeAirborneLarge = .2f;
  [SerializeField] private float accelerationTimeGroundedSmall = .04f;
  [SerializeField] private float accelerationTimeGroundedLarge = .09f;
  [SerializeField] private int scaleFactor = 5;
  [SerializeField] private float scaleTime = 0.15f;
  [SerializeField] private float dashTime = 0.5f;
  [SerializeField] private float dashSpeed = 16;
  [SerializeField] private AudioSource audioJump;
  [SerializeField] private AudioSource audioLand;
  [SerializeField] private AudioSource audioScale;
  [SerializeField] private float gravityScale = 6;
  [SerializeField] private float fallingGravityScale = 5;
  
  [HideInInspector]public Vector2 movementInput;
  
  private Rigidbody2D _rb;
  private RaycastController _raycastController;
  private CameraController _cameraController;
  private float _speed;
  private float _maxJumpVelocity;
  private float _minJumpVelocity;
  private float _accelerationTimeAirborne = .1f;
  private float _accelerationTimeGrounded = .04f;  
  private float _velocityXSmoothing;
  private bool _isScaling;
  private float _scaleSmoothing;
  private RigidbodyConstraints2D _initialRBConstraints;
  private float _prevVelY;
  private bool _prevGrounded;
  private bool _isDashing;

  [HideInInspector] public bool unstable;
  [HideInInspector] public bool isSmall;
  
  void Awake() {
    _rb = GetComponent<Rigidbody2D>();
    _raycastController = GetComponent<RaycastController>();
    _cameraController = GetComponent<CameraController>();
    isSmall = true;
    _speed = speedSmall;
    _maxJumpVelocity = maxJumpVelocitySmall;
    _minJumpVelocity = minJumpVelocitySmall;
    _initialRBConstraints = _rb.constraints;
    _rb.gravityScale = gravityScale;
    growEnabled = false;
  }
  
  void Start() { }

  private void FixedUpdate() {
    ScaleAndTranslate();
    
    // movement
    float velY = _rb.velocity.y;
    float velX = Mathf.SmoothDamp(_rb.velocity.x, movementInput.x * (_isDashing ? dashSpeed : _speed), ref _velocityXSmoothing,
      _raycastController.collisions.Bottom ? _accelerationTimeGrounded : _accelerationTimeAirborne);
    
    // stop movement when pinched
    if (!isSmall) {
      if (_raycastController.collisions.pinchedVertically) {
        velX = 0;
      }
      if (_raycastController.collisions.pinchedHorizontally) {
        velY = 0;
        
        // fix for a bug that I don't understand... when pinched horizontally and not grounded player slooowly slides downward
        _rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
      }
      else {
        _rb.constraints = _initialRBConstraints;
      }
    }
    else {
      _rb.constraints = _initialRBConstraints;
    }

    // reduce gravity on downward phase of jump
    float unpinchedGravityScale = _rb.velocity.y < 0 && !_raycastController.collisions.Bottom
      ? fallingGravityScale
      : gravityScale;
    _rb.gravityScale = _raycastController.collisions.pinchedHorizontally ? 0 : unpinchedGravityScale;
    _rb.velocity = new Vector2(velX, velY);    
    
    // play jump landing sound
    if (!_prevGrounded && _raycastController.collisions.Bottom && _prevVelY < 0.5f) {
      audioLand.Play();
    }
    _prevGrounded = _raycastController.collisions.Bottom;
    _prevVelY = velY;
  }

  private void ScaleAndTranslate() {
    if (_isScaling) {
      if (!_raycastController.collisions.pinched || isSmall) {
        float prevScale = transform.localScale.x;
        float scale = Mathf.SmoothDamp(transform.localScale.x, isSmall ? 1 : scaleFactor, ref _scaleSmoothing, scaleTime);
        float scaleDiff = scale - prevScale;
        transform.localScale = new Vector3(scale, scale, 1);
        _isScaling = !(transform.localScale.x >= scaleFactor);

        // if scaling up...
        if (!isSmall) {
          
          float posXDelta = 0;
          float posYDelta = 0;
          
          //while colliding with a surface, translate away from that surface
          if (_raycastController.collisions.Bottom) {
            posYDelta = scaleDiff / 2;
          }
          else if (_raycastController.collisions.Top) {
            posYDelta = scaleDiff / -2;
          }

          if (_raycastController.collisions.Right) {
            posXDelta = scaleDiff / -2;
          }
          else if (_raycastController.collisions.Left) {
            posXDelta = scaleDiff / 2;
          }
          transform.position += new Vector3(posXDelta, posYDelta, 0);
        }
        
        // if scaling down...
        else {
          float posXDelta = 0;
          float posYDelta = 0;
          
          // if moving right translate towards player's right edge
          if (movementInput.x > 0) {
            posXDelta = -scaleDiff / (_raycastController.collisions.Right ? 2 : 4);
          }
          // if moving left translate towards player's left edge
          else if (movementInput.x < 0) {
            posXDelta = scaleDiff / (_raycastController.collisions.Left ? 2 : 4);
          }
          
          // if on the ground, translate towards the ground
          if (_raycastController.collisions.Bottom) {
            posYDelta = scaleDiff / 2;
          }
          // if in mid-air translate towards players upper edge
          else {
            posYDelta = scaleDiff / -2;
          }
          transform.position += new Vector3(posXDelta, posYDelta, 0);
        }
      }
    }    
  }

  public void OnJumpPerformed() {
    if (_raycastController.collisions.Bottom && !_raycastController.collisions.pinched) {
      audioJump.Play();

      if (!unstable || isSmall) {
        _rb.velocity = new Vector2(_rb.velocity.x, _maxJumpVelocity);
      }
      else {
        _rb.velocity = new Vector2(_rb.velocity.x, maxJumpVelocitySmall / 2);
        if (transform.parent != null && transform.parent.CompareTag("PlatformSinker")) {
          PlatformSinker platform = transform.parent.GetComponent<PlatformSinker>();
          platform.sunk = true;
        }
      }
    }
  }

  public void OnJumpCanceled() {
    if (_rb.velocity.y > minJumpVelocitySmall) {
      _rb.velocity = new Vector2(_rb.velocity.x, _minJumpVelocity);
    }
  }

  public void OnToggleSizePerformed() {
    if (growEnabled) {
      if (isSmall) {
        _cameraController.SwitchCamera(1);
        _maxJumpVelocity = maxJumpVelocityLarge;
        _minJumpVelocity = minJumpVelocityLarge;
        _speed = speedLarge;
        isSmall = false;
        _accelerationTimeAirborne = accelerationTimeAirborneLarge;
        _accelerationTimeGrounded = accelerationTimeGroundedLarge;
      }
      else {
        _cameraController.SwitchCamera(0);
        _maxJumpVelocity = maxJumpVelocitySmall;
        _minJumpVelocity = minJumpVelocitySmall;
        _speed = speedSmall;
        isSmall = true;
        _accelerationTimeAirborne = accelerationTimeAirborneSmall;
        _accelerationTimeGrounded = accelerationTimeGroundedSmall;
      }
      _isScaling = true;
      audioScale.Play();        
    }
  }

  public void OnDashPerformed() {
    if (dashEnabled && !_isDashing && isSmall && _raycastController.collisions.Bottom) {
      StartCoroutine(Dash());
    }
  }
  
  IEnumerator Dash() {
    _isDashing = true;
    yield return new WaitForSeconds(dashTime);
    _isDashing = false;
  }

  private void OnCollisionEnter2D(Collision2D other) {
    if (other.collider.CompareTag("PlatformSinker")) {
      transform.parent = other.transform;
      print("parented ");
    }
  }

  private void OnCollisionExit2D(Collision2D other) {
    if (other.collider.CompareTag("PlatformSinker")) {
      transform.parent = null;
      print("unparented ");
    }
  }
  
  
  
  public void Obtain(string itemName) {
    switch(itemName) 
    {
      case "POWER_GROW":
        growEnabled = true;
        break;
      default:
        break;
    }
  }
}