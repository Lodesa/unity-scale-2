using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(RaycastController))]
public class Player : MonoBehaviour {

  [SerializeField]private float speed = 6;
  [SerializeField]private float maxJumpVelocity = 15;
  [SerializeField]private float minJumpVelocity = 8;  
  [SerializeField]private float accelerationTimeAirborne = .1f;
  [SerializeField]private float accelerationTimeGrounded = .04f;
  
  [HideInInspector]public Vector2 movementInput;
  
  private Rigidbody2D _rb;
  private bool _grounded;
  private float _velocityXSmoothing;
  private RaycastController _raycastController;

  void Awake() {
    _rb = GetComponent<Rigidbody2D>();
    _raycastController = GetComponent<RaycastController>();
  }
  
  void Start() { }

  private void FixedUpdate() {
    float velocityX = Mathf.SmoothDamp(_rb.velocity.x, movementInput.x * speed, ref _velocityXSmoothing, 
      _grounded ? accelerationTimeGrounded : accelerationTimeAirborne);
    _rb.velocity = new Vector2(velocityX, _rb.velocity.y);
  }

  public void OnJumpPerformed() {
    if (_raycastController.isGrounded) {
      _rb.velocity = new Vector2(_rb.velocity.x, maxJumpVelocity);
    }
  }

  public void OnJumpCanceled() {
    if (_rb.velocity.y > minJumpVelocity) {
      _rb.velocity =new Vector2(_rb.velocity.x, minJumpVelocity);
    }
  }
}