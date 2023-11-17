using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

  [HideInInspector]public Vector2 velocity;
  
  private Rigidbody2D _rb;

  void Awake() {
    _rb = GetComponent<Rigidbody2D>();
  }
  
  void Start() { }

  void Update() { }

  private void FixedUpdate() {
    _rb.velocity = new Vector2(velocity.x, velocity.y);
  }
}