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
      _raycastController.collisions.Below ? _accelerationTimeGrounded : _accelerationTimeAirborne);
    _rb.velocity = new Vector2(velocityX, _rb.velocity.y);
  }

  public void OnJumpPerformed() {
    if (_raycastController.collisions.Below) {
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
      Scale(new Vector3(scaleFactor, scaleFactor, 1), 0.15f);
      _cameraController.SwitchCamera(1);
      _maxJumpVelocity = maxJumpVelocityLarge;
      _minJumpVelocity = minJumpVelocityLarge;
      _speed = speedLarge;
      _isSmall = false;
      _accelerationTimeAirborne = accelerationTimeAirborneLarge;
      _accelerationTimeGrounded = accelerationTimeGroundedLarge;
    }
  }

  public void OnShrinkPerformed() {
    if (!_isSmall) {
      Scale(Vector3.one, 0.15f);
      _cameraController.SwitchCamera(0);
      _maxJumpVelocity = maxJumpVelocitySmall;
      _minJumpVelocity = minJumpVelocitySmall;
      _speed = speedSmall;
      _isSmall = true;
      _accelerationTimeAirborne = accelerationTimeAirborneSmall;
      _accelerationTimeGrounded = accelerationTimeGroundedSmall;
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
      // if (gettingLarger && ((collisions.below && collisions.above) || (collisions.left && collisions.right))) {
      //   transform.localScale = startingScale;
      //   yield break;
      // }
      float scaleDifference = scaleTo.x - startingScale.x;
      Vector2 vel = Vector2.zero;
      if (gettingLarger && _raycastController.collisions.Below) {
        vel += Vector2.up * (Time.deltaTime * scaleDifference);
      }
      _rb.velocity = vel;
      
      transform.localScale = Vector3.Lerp(startingScale, scaleTo, (elapsedTime / seconds));
      elapsedTime += Time.deltaTime;
      yield return new WaitForEndOfFrame();
    }

    transform.localScale = scaleTo;
  }  
}