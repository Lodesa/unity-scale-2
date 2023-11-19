using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(RaycastController))]
[RequireComponent(typeof(CameraController))]
public class Player : MonoBehaviour {

  [SerializeField]private float speedSmall = 6;
  [SerializeField]private float speedLarge = 10;
  [SerializeField]private float maxJumpVelocitySmall = 22;
  [SerializeField]private float maxJumpVelocityLarge = 40;
  [SerializeField]private float minJumpVelocitySmall = 5;
  [SerializeField]private float minJumpVelocityLarge = 10;
  [SerializeField]private float accelerationTimeAirborneSmall = .1f;
  [SerializeField]private float accelerationTimeAirborneLarge = .2f;
  [SerializeField]private float accelerationTimeGroundedSmall = .04f;
  [SerializeField]private float accelerationTimeGroundedLarge = .09f;
  [SerializeField]private int scaleFactor = 5;
  [SerializeField]private float scaleTime = 0.15f;
  
  [HideInInspector]public Vector2 movementInput;
  
  private Rigidbody2D _rb;
  private RaycastController _raycastController;
  private CameraController _cameraController;
  private float _speed;
  private float _maxJumpVelocity;
  private float _minJumpVelocity;
  private float _accelerationTimeAirborne = .1f;
  private float _accelerationTimeGrounded = .04f;  
  private bool _isSmall;
  private float _velocityXSmoothing;
  private float _scaleSmoothing;
  private bool _isScaling;

  void Awake() {
    _rb = GetComponent<Rigidbody2D>();
    _raycastController = GetComponent<RaycastController>();
    _cameraController = GetComponent<CameraController>();
    _isSmall = true;
    _speed = speedSmall;
    _maxJumpVelocity = maxJumpVelocitySmall;
    _minJumpVelocity = minJumpVelocitySmall;
  }
  
  void Start() { }

  private void FixedUpdate() {
    transform.rotation = Quaternion.identity;
    float velocityX = Mathf.SmoothDamp(_rb.velocity.x, movementInput.x * _speed, ref _velocityXSmoothing,
      _raycastController.collisions.Bottom ? _accelerationTimeGrounded : _accelerationTimeAirborne);
    _rb.velocity = _raycastController.collisions.pinched
      ? Vector2.zero
      : new Vector2(velocityX, _rb.velocity.y);
    
    if (_isScaling) {
      if (!_isSmall && _raycastController.collisions.pinched) { }
        
      else {
        float scale = Mathf.SmoothDamp(transform.localScale.x, _isSmall ? 1 : scaleFactor, ref _scaleSmoothing, scaleTime);
        transform.localScale = new Vector3(scale, scale, 1);
        _isScaling = !(transform.localScale.x >= scaleFactor);
      }
    }    
  }

  public void OnJumpPerformed() {
    if (_raycastController.collisions.Bottom) {
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
      // Scale(new Vector3(scaleFactor, scaleFactor, 1), 1);
      _cameraController.SwitchCamera(1);
      _maxJumpVelocity = maxJumpVelocityLarge;
      _minJumpVelocity = minJumpVelocityLarge;
      _speed = speedLarge;
      _isScaling = true;
      _isSmall = false;
      _accelerationTimeAirborne = accelerationTimeAirborneLarge;
      _accelerationTimeGrounded = accelerationTimeGroundedLarge;
    }
  }

  public void OnShrinkPerformed() {
    if (!_isSmall) {
      // Scale(Vector3.one, 0.15f);
      _cameraController.SwitchCamera(0);
      _maxJumpVelocity = maxJumpVelocitySmall;
      _minJumpVelocity = minJumpVelocitySmall;
      _speed = speedSmall;
      _isScaling = true;
      _isSmall = true;
      _accelerationTimeAirborne = accelerationTimeAirborneSmall;
      _accelerationTimeGrounded = accelerationTimeGroundedSmall;
    }
  }
}