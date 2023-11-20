using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(RaycastController))]
[RequireComponent(typeof(CameraController))]
public class Player : MonoBehaviour {

  [SerializeField]private float speedSmall = 9;
  [SerializeField]private float speedLarge = 10;
  [SerializeField]private float maxJumpVelocitySmall = 26;
  [SerializeField]private float maxJumpVelocityLarge = 40;
  [SerializeField]private float minJumpVelocitySmall = 5;
  [SerializeField]private float minJumpVelocityLarge = 10;
  [SerializeField]private float accelerationTimeAirborneSmall = .1f;
  [SerializeField]private float accelerationTimeAirborneLarge = .2f;
  [SerializeField]private float accelerationTimeGroundedSmall = .04f;
  [SerializeField]private float accelerationTimeGroundedLarge = .09f;
  [SerializeField]private int scaleFactor = 5;
  [SerializeField]private float scaleTime = 0.15f;
  [SerializeField]private float dashTime = 0.5f;
  [SerializeField]private float dashSpeed = 16;
  [SerializeField] private AudioSource audioJump;
  [SerializeField] private AudioSource audioLand;
  [SerializeField] private AudioSource audioScale;
  
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
  private bool _isSmall;
  private bool _isScaling;
  private float _scaleSmoothing;
  private float _initialGravityScale;
  private float _prevVelY;
  private bool _prevGrounded;
  private bool _isDashing;

  void Awake() {
    _rb = GetComponent<Rigidbody2D>();
    _raycastController = GetComponent<RaycastController>();
    _cameraController = GetComponent<CameraController>();
    _isSmall = true;
    _speed = speedSmall;
    _maxJumpVelocity = maxJumpVelocitySmall;
    _minJumpVelocity = minJumpVelocitySmall;
    _initialGravityScale = _rb.gravityScale;
  }
  
  void Start() { }

  private void FixedUpdate() {
    // movement
    float velY = _rb.velocity.y;
    float velX = Mathf.SmoothDamp(_rb.velocity.x, movementInput.x * (_isDashing ? dashSpeed : _speed), ref _velocityXSmoothing,
      _raycastController.collisions.Bottom ? _accelerationTimeGrounded : _accelerationTimeAirborne);

    // scaling
    if (_isScaling) {
      if (!_raycastController.collisions.pinched || _isSmall) {
        float scale = Mathf.SmoothDamp(transform.localScale.x, _isSmall ? 1 : scaleFactor, ref _scaleSmoothing, scaleTime);
        transform.localScale = new Vector3(scale, scale, 1);
        _isScaling = !(transform.localScale.x >= scaleFactor);
      }
    }

    if (!_isSmall) {
      if (_raycastController.collisions.pinchedVertically) {
        velX = 0;
      }
      if (_raycastController.collisions.pinchedHorizontally) {
        velY = 0;
      }
    }

    _rb.gravityScale = _raycastController.collisions.pinchedHorizontally ? 0 : _initialGravityScale;
    _rb.velocity = new Vector2(velX, velY);    
    transform.rotation = Quaternion.identity;
    
    // jump landing sound
    if (!_prevGrounded && _raycastController.collisions.Bottom && _prevVelY < 0.5f) {
      audioLand.Play();
    }
    _prevGrounded = _raycastController.collisions.Bottom;
    _prevVelY = velY;
  }

  public void OnJumpPerformed() {
    if (_raycastController.collisions.Bottom) {
      audioJump.Play();
      _rb.velocity = new Vector2(_rb.velocity.x, _maxJumpVelocity);
    }
  }

  public void OnJumpCanceled() {
    if (_rb.velocity.y > minJumpVelocitySmall) {
      _rb.velocity = new Vector2(_rb.velocity.x, _minJumpVelocity);
    }
  }

  public void OnGrowPerformed() {
    if (_isSmall) {
      _cameraController.SwitchCamera(1);
      _maxJumpVelocity = maxJumpVelocityLarge;
      _minJumpVelocity = minJumpVelocityLarge;
      _speed = speedLarge;
      _isScaling = true;
      _isSmall = false;
      _accelerationTimeAirborne = accelerationTimeAirborneLarge;
      _accelerationTimeGrounded = accelerationTimeGroundedLarge;
      audioScale.Play();
    }
  }

  public void OnShrinkPerformed() {
    if (!_isSmall) {
      _cameraController.SwitchCamera(0);
      _maxJumpVelocity = maxJumpVelocitySmall;
      _minJumpVelocity = minJumpVelocitySmall;
      _speed = speedSmall;
      _isScaling = true;
      _isSmall = true;
      _accelerationTimeAirborne = accelerationTimeAirborneSmall;
      _accelerationTimeGrounded = accelerationTimeGroundedSmall;
      audioScale.Play();
    }
  }

  public void OnDashPerformed() {
    if (!_isDashing && _isSmall && _raycastController.collisions.Bottom) {
      StartCoroutine(Dash());
    }
  }
  
  IEnumerator Dash() {
    _isDashing = true;
    yield return new WaitForSeconds(dashTime);
    _isDashing = false;
  }
}